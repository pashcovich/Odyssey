using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Odyssey.Content.Shaders;
using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Model;
using Odyssey.Tools.ShaderGenerator.Properties;
using Odyssey.Tools.ShaderGenerator.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.View;
using Odyssey.Utils.Logging;
using ShaderGenerator.Data;
using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public class CompilationViewModel : ViewModelBase
    {
        ObservableCollection<ShaderDescriptionViewModel> shaderList;
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
            TechniqueMappingViewModel tmDefault = new TechniqueMappingViewModel { Index = 1, TechniqueMapping = new TechniqueMapping("Default") };
            techniques.Add(tmDefault);
            shaderCollection.Add(tmDefault.TechniqueMapping);

            TechniqueMappingViewModel tmShadows = new TechniqueMappingViewModel { Index = 2, TechniqueMapping = new TechniqueMapping("DiffuseMap") };
            techniques.Add(tmShadows);
            shaderCollection.Add(tmShadows.TechniqueMapping);

            TechniqueMappingViewModel tmDiffuse = new TechniqueMappingViewModel { Index = 3, TechniqueMapping = new TechniqueMapping("Shadows") };
            techniques.Add(tmDiffuse);
            shaderCollection.Add(tmDiffuse.TechniqueMapping);
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

        public ObservableCollection<ShaderDescriptionViewModel> Shaders
        {
            get {return shaderList;}
            set
            {
                shaderList = value;
                RaisePropertyChanged("Shaders");
            }
        }

        public void CompileShader(string techniqueName, ShaderDescriptionViewModel vmShader, out IEnumerable<ErrorViewModel> errors)
        {
            List<ErrorViewModel> errorList = new List<ErrorViewModel>();
            
            CompilationResult compilationResult;
            bool result = compiler.Compile(vmShader, out compilationResult);
            vmShader.CompilationStatus = result == true ? CompilationStatus.Successful : CompilationStatus.Failed;
            if (!result)
            {
                foreach (ErrorModel error in compiler.CompilationErrors)
                    errorList.Add(new ErrorViewModel { ErrorModel = error });
                errors = errorList;
            }
            else
            {
                errors = null;
                var references = vmShader.ShaderDescriptionModel.Shader.References;
                var textureReferences = vmShader.ShaderDescriptionModel.Shader.TextureReferences;
                var samplerReferences = vmShader.ShaderDescriptionModel.Shader.SamplerReferences;
                ShaderObject shaderObject = new ShaderObject(vmShader.Name, vmShader.Type, vmShader.FeatureLevel,
                    compilationResult.Bytecode, references, textureReferences, samplerReferences);
                
                shaderCollection.Add(techniqueName, shaderObject);
            }
        }

        public void CompileAll()
        {
            ObservableCollection<ErrorViewModel> errorList = new ObservableCollection<ErrorViewModel>();
            IEnumerable<ErrorViewModel> errors;

            foreach (var vmTm in Techniques)
            {
                var techniqueShaders = from shader in shaderList
                                       where shader.IsAssignedTo(vmTm.Name)
                                       select shader;

                if (techniqueShaders.Count()==0)
                    continue;

                foreach (var vmShader in techniqueShaders)
                {
                    CompileShader(vmTm.Name, vmShader, out errors);

                    if (errors == null)
                    {
                        vmShader.UpdateKeyPart();
                        continue;
                    }
                    foreach (var error in errors)
                        errorList.Add(error);
                }

                if (errorList.Count == 0)
                    vmTm.CreateKey(techniqueShaders);
            }

            Uri uriPath = new Uri(Odyssey.Tools.ShaderGenerator.Properties.Settings.Default.OutputPath);
            string fullPath = string.Format("{0}\\{1}.{2}", uriPath.LocalPath, shaderCollection.Name, "ofx");
            ResourceManager.Serialize(fullPath, shaderCollection);
            vmDebug.Errors = errorList;
        }

        public void Open()
        {
            OpenFileDialog openDialog = new OpenFileDialog { Filter = "Odyssey Shader Graph|*.osg" };
            bool? result = openDialog.ShowDialog();
            if (result == false)
                return;
            var derivedTypes = Shader.KnownTypes().Concat(NodeBase.KnownTypes()).ToArray();
            try
            {
                var shaders = ResourceManager.Deserialize<Dictionary<Shader,string[]>>(openDialog.FileName, derivedTypes);
                if (shaders.Count() == 0)
                {
                    MessageBox.Show("No techniques found in file.", "Error", MessageBoxButton.OK);
                    return;
                }
                shaderList.Clear();

                var techniques = shaders.Values.SelectMany(s => s).Distinct().Select((t, index) => new TechniqueMappingViewModel { Index = index + 1, TechniqueMapping = new TechniqueMapping(t) });
                                         

                foreach (var kvp in shaders)
                {
                    ShaderDescriptionViewModel sVM = new ShaderDescriptionViewModel
                    {
                        ShaderDescriptionModel = new Odyssey.Tools.ShaderGenerator.Model.ShaderDescription { Shader = kvp.Key },
                        Techniques = new ObservableCollection<TechniqueMappingViewModel>(from t in techniques where kvp.Value.Contains(t.Name) select t)
                    };
                    shaderList.Add(sVM);
                }

                var vmLocator = ((ViewModelLocator)Application.Current.FindResource("Locator"));
                RelayCommand cmd = (RelayCommand)vmLocator.Main.CompileCommand;
                cmd.RaiseCanExecuteChanged();
            }
            catch (SerializationException ex)
            {
                LogEvent.Tool.Error("Failed to open file {0}.", openDialog.FileName);
                return;
            }
        }

        public void Save()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Odyssey Shader Graph|*.osg";
            bool? result = saveDialog.ShowDialog();
            if (result == false)
                return;

            var derivedTypes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                from lType in lAssembly.GetTypes()
                                where lType.IsSubclassOf(typeof(Shader))
                                select lType).ToArray();


            var shaderGraph = (from s in shaderList
                        let mappedTo = (from t in techniques where s.IsAssignedTo(t) select t.Name)
                        select new { Shader = s.ShaderDescriptionModel.Shader, Techniques = mappedTo }).ToDictionary(s => s.Shader, s => s.Techniques.ToArray());
            
            ResourceManager.Serialize(saveDialog.FileName, shaderGraph, derivedTypes);
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

        
        public void CreateTechnique()
        {
            CustomDialog dialog = new CustomDialog { Title="Create new", Header= "Please name the new Technique:"};
            if (dialog.ShowDialog() == true)
            {
                TechniqueMapping tMapping = new TechniqueMapping(dialog.Result);
                TechniqueMappingViewModel tMappingVM = new TechniqueMappingViewModel { TechniqueMapping =tMapping, Index = techniques.Count+1};
                techniques.Add(tMappingVM);
                shaderCollection.Add(tMapping);
            }
        }
    }
}
