using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Odyssey.Content.Shaders;
using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Model;
using Odyssey.Tools.ShaderGenerator.Properties;
using Odyssey.Tools.ShaderGenerator.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;
using Odyssey.Tools.ShaderGenerator.View;
using Odyssey.Tools.ShaderGenerator.ViewModel.Messages;
using Odyssey.Utils.Logging;
using ShaderGenerator.Data;
using SharpDX.D3DCompiler;
using SharpYaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public class CompilationViewModel : ViewModelBase
    {
        const string NewTechnique = "Technique";
        ObservableCollection<IShaderViewModel> shaderList;
        ObservableCollection<TechniqueMappingViewModel> techniques;
        ShaderCompiler compiler;
        DebugViewModel vmDebug;
        ShaderCollection shaderCollection;

        internal ShaderCollection ShaderCollection { get { return shaderCollection; } }

        public CompilationViewModel()
        {
            compiler = new ShaderCompiler();
            shaderCollection = new ShaderCollection("Untitled");
            var vmLocator = ((ViewModelLocator)Application.Current.FindResource("Locator"));
            techniques = new ObservableCollection<TechniqueMappingViewModel>();
            vmDebug = vmLocator.Debug;
            shaderList = new ObservableCollection<IShaderViewModel>();
            //TechniqueMappingViewModel tmDefault = new TechniqueMappingViewModel { Index = 1, TechniqueMapping = new TechniqueMapping("Default") };
            //RegisterTechnique(tmDefault);

            //TechniqueMappingViewModel tmShadows = new TechniqueMappingViewModel { Index = 2, TechniqueMapping = new TechniqueMapping("DiffuseMap") };
            //RegisterTechnique(tmShadows);

            //TechniqueMappingViewModel tmDiffuse = new TechniqueMappingViewModel { Index = 3, TechniqueMapping = new TechniqueMapping("Shadows") };
            //RegisterTechnique(tmDiffuse);

            Messenger.Default.Register<TechniqueMessage> (this, (message) => ReceiveTechniqueMessage(message));
        }

        private void ReceiveTechniqueMessage(TechniqueMessage message)
        {
            switch (message.Action)
            {
                case TechniqueAction.Remove:
                    if (techniques.Contains(message.Technique))
                        techniques.Remove(message.Technique);
                    break;

                case TechniqueAction.Unassign:
                    foreach (var shaderVM in shaderList.OfType<ShaderDescriptionViewModel>())
                    {
                        if (shaderVM.IsAssignedTo(message.Technique))
                            shaderVM.RemoveTechnique(message.Technique);
                    }
                    break;
            }
        }

        public ObservableCollection<TechniqueMappingViewModel> Techniques
        {
            get { return techniques; }
            set
            {
                techniques = value;
                RaisePropertyChanged("Techniques");
            }
        }

        public int TechniqueCount { get { return techniques.Count; } }

        public ObservableCollection<IShaderViewModel> Shaders
        {
            get {return shaderList;}
            set
            {
                shaderList = value;
                RaisePropertyChanged("Shaders");
            }
        }

        

        public void CompileAllCode()
        {
            ObservableCollection<ErrorViewModel> errorList = new ObservableCollection<ErrorViewModel>();
            IEnumerable<ErrorViewModel> errors;
            int shaderCount = 0;

            foreach (var shaderVM in shaderList.OfType<ShaderCodeViewModel>())
            {
                bool compilationResult = compiler.CompileShader(shaderVM, out errors);
                if (!compilationResult)
                {
                    foreach (var error in errors)
                        errorList.Add(error);
                }
                else
                    shaderCount++;
            }
            vmDebug.Errors = errorList;

            if (shaderCount > 0)
            {
                string outputPath = Odyssey.Tools.ShaderGenerator.Properties.Settings.Default.OutputPath;
                string fullPath = Path.Combine(outputPath, string.Format("{0}.{1}", shaderCollection.Name, "ofx"));
                ResourceManager.Serialize(fullPath, shaderCollection);
                DisplayCompletionMessage(shaderCount, errorList.Count);
            }
        }

        public void CompileAll()
        {
            ObservableCollection<ErrorViewModel> errorList = new ObservableCollection<ErrorViewModel>();
            IEnumerable<ErrorViewModel> errors;
            int shaderCount=0;
            foreach (var vmTm in Techniques)
            {
                var techniqueShaders = from shader in shaderList.OfType<ShaderDescriptionViewModel>()
                                       where shader.IsAssignedTo(vmTm.Name)
                                       select shader;

                if (techniqueShaders.Count() == 0)
                    continue;

                foreach (var vmShader in techniqueShaders)
                {
                    ShaderObject shaderObject;
                    if (compiler.CompileShader(vmTm.Name, vmShader, out shaderObject, out errors))
                    {
                        shaderCollection.Add(vmTm.Name, shaderObject);
                        shaderCount++;
                        continue;
                    }
                    else
                    {
                        foreach (var error in errors)
                            errorList.Add(error);
                    }
                }

                if (errorList.Count == 0)
                {
                    vmTm.UpdateKey(techniqueShaders);
                    vmTm.UpdateToolTip();
                }
            }

            
            vmDebug.Errors = errorList;

            if (shaderCount > 0)
            {
                string outputPath = Odyssey.Tools.ShaderGenerator.Properties.Settings.Default.OutputPath;
                string fullPath = Path.Combine(outputPath, string.Format("{0}.{1}", shaderCollection.Name, "ofx"));
                ResourceManager.Serialize(fullPath, shaderCollection);
                DisplayCompletionMessage(shaderCount, errorList.Count);
            }
        }

        void DisplayCompletionMessage(int shaders, int errors)
        {
            string message = shaders == 1 
                ? string.Format("The shader was saved in {0}", Odyssey.Tools.ShaderGenerator.Properties.Settings.Default.OutputPath)
                : string.Format("A total of {0} shaders were saved in {1}",shaders, Odyssey.Tools.ShaderGenerator.Properties.Settings.Default.OutputPath);

            string errorMessage = errors > 0
                ? string.Format("\n{0} failed to compile. See build output for details.", errors)
                : string.Empty;

            MessageBox.Show(message + errorMessage, "Compilation completed", MessageBoxButton.OK, 
                errors > 0 ? MessageBoxImage.Warning : MessageBoxImage.Information);                
        }

        public bool Import()
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "HLSL source code|*.hlsl;*.fx|All Files|*.*",
                Multiselect = true
            };
            bool? result = openDialog.ShowDialog();
            if (result == false)
                return false;
            shaderList.Clear(); 
            IncludeHandler.BaseDirectory =  Path.GetDirectoryName(openDialog.FileNames.First());
            foreach (string file in openDialog.FileNames)
            {
                string sourceCode = string.Empty;
                using (StreamReader sr = new StreamReader(file))
                    sourceCode = sr.ReadToEnd();

                ShaderCodeViewModel shaderVM = new ShaderCodeViewModel
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    ShaderModel = Odyssey.Tools.ShaderGenerator.Properties.Settings.Default.DefaultShaderModel,
                    SourceCode = sourceCode
                };
                shaderList.Add(shaderVM);
            }
            return true;
        }

        public bool Open()
        {
            OpenFileDialog openDialog = new OpenFileDialog { Filter = "Odyssey Shader Graph|*.osg" };
            bool? result = openDialog.ShowDialog();
            if (result == false)
                return false;
            YamlSerializer serializer = new YamlSerializer();
            Dictionary<Shader, string[]> shaderGraph = null;
            try
            {
                shaderGraph = serializer.DeserializeGraph(openDialog.FileName);
            }
            catch (YamlException ex)
            {
                string errorMsg = "Failed to open file {0}";
                MessageBox.Show(string.Format(errorMsg, Path.GetFileName(openDialog.FileName)), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Daedalus.Error(ex);
                return false;
            }

            if (shaderGraph.Count() == 0)
            {
                MessageBox.Show("No techniques were found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            shaderList.Clear();

            var techniques = shaderGraph.Values.SelectMany(s => s).Distinct().Select((t, index) => new TechniqueMappingViewModel { Index = index + 1, TechniqueMapping = new TechniqueMapping(t) });
            foreach (var technique in techniques)
                if (!shaderCollection.Contains(technique.Name))
                    RegisterTechnique(technique);

            foreach (var kvp in shaderGraph)
            {
                ShaderDescriptionViewModel sVM = new ShaderDescriptionViewModel
                {
                    ShaderDescriptionModel = new Odyssey.Tools.ShaderGenerator.Model.ShaderDescription { Shader = kvp.Key },
                    Techniques = new ObservableCollection<TechniqueMappingViewModel>(from t in techniques where kvp.Value.Contains(t.Name) select t)
                };
                shaderList.Add(sVM);
            }

            var vmLocator = ((ViewModelLocator)Application.Current.FindResource("Locator"));
            vmLocator.Main.ShaderCollectionName = Path.GetFileNameWithoutExtension(openDialog.FileName);

            RelayCommand cmd = (RelayCommand)vmLocator.Main.CompileShaderGraphCommand;
            cmd.RaiseCanExecuteChanged();
            return true;

        }

        public void Save()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Odyssey Shader Graph|*.osg";
            bool? result = saveDialog.ShowDialog();
            if (result == false)
                return;

            Mouse.SetCursor(Cursors.Wait);

            var derivedTypes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                from lType in lAssembly.GetTypes()
                                where lType.IsSubclassOf(typeof(Shader))
                                select lType).ToArray();


            var shaderGraph = (from s in shaderList.OfType<ShaderDescriptionViewModel>()
                        let mappedTo = (from t in techniques where s.IsAssignedTo(t) select t.Name)
                        select new { Shader = s.ShaderDescriptionModel.Shader, Techniques = mappedTo })
                        .ToDictionary(s => s.Shader, s => s.Techniques.ToArray());

            YamlSerializer serializer = new YamlSerializer();
            serializer.SerializeGraph(shaderGraph, saveDialog.FileName);
            Mouse.SetCursor(Cursors.Arrow);
            //ResourceManager.Serialize(saveDialog.FileName, shaderGraph, derivedTypes);
        }

        public bool CanCompile()
        {
            if (Shaders == null)
                return false;

            foreach (var shader in Shaders)
                if (shader.IsEmpty)
                    return false;
            
            return true;
        }

        void RegisterTechnique(TechniqueMappingViewModel techniqueVM)
        {
            techniques.Add(techniqueVM);
            shaderCollection.Add(techniqueVM.TechniqueMapping);
        }

        
        public void CreateTechnique()
        {
            CreateTechniqueView view = new CreateTechniqueView();
            view.ShowDialog();
            if(!view.DialogResult.HasValue)
                return;

            var vmCT = (CreateTechniqueViewModel)view.DataContext;
            string techniqueName = string.IsNullOrEmpty(vmCT.Name)
                ? string.Format("{0}{1}", NewTechnique, techniques.Count(t => t.Name.StartsWith(NewTechnique)) + 1)
                : vmCT.Name;
            TechniqueMappingViewModel vmTM = new TechniqueMappingViewModel
            {
                TechniqueMapping = new TechniqueMapping(techniqueName)
                { Key = vmCT.Key },
                Name = techniqueName,
                Index = techniques.Count + 1,
                
            };
            RegisterTechnique(vmTM);

            //CustomDialog dialog = new CustomDialog { Title="Create new", Header= "Please name the new Technique:"};
            //if (dialog.ShowDialog() == true)
            //{
            //    TechniqueMapping tMapping = new TechniqueMapping(dialog.Result);
            //    TechniqueMappingViewModel tMappingVM = new TechniqueMappingViewModel { TechniqueMapping =tMapping, Index = techniques.Count+1};
            //    techniques.Add(tMappingVM);
            //    shaderCollection.Add(tMapping);
            //}
        }
    }
}
