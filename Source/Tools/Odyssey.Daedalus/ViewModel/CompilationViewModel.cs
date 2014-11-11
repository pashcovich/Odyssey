#region Using Directives

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Odyssey.Daedalus.Data;
using Odyssey.Daedalus.Model;
using Odyssey.Daedalus.Serialization;
using Odyssey.Daedalus.View;
using Odyssey.Daedalus.ViewModel.Messages;
using Odyssey.Graphics.Shaders;
using Odyssey.Utilities.Logging;
using Odyssey.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Settings = Odyssey.Daedalus.Properties.Settings;
using ShaderDescription = Odyssey.Graphics.Shaders.ShaderDescription;

#endregion Using Directives

namespace Odyssey.Daedalus.ViewModel
{
    public class CompilationViewModel : ViewModelBase
    {
        private const string NewTechnique = "Technique";
        private readonly ShaderCompiler compiler;
        private readonly ShaderCollection shaderCollection;
        private readonly DebugViewModel vmDebug;
        private ObservableCollection<IShaderViewModel> shaderList;
        private ObservableCollection<TechniqueMappingViewModel> techniques;

        public CompilationViewModel()
        {
            compiler = new ShaderCompiler();
            shaderCollection = new ShaderCollection("Untitled");
            var vmLocator = ((ViewModelLocator)System.Windows.Application.Current.FindResource("Locator"));
            techniques = new ObservableCollection<TechniqueMappingViewModel>();
            vmDebug = vmLocator.Debug;
            shaderList = new ObservableCollection<IShaderViewModel>();

            Messenger.Default.Register<TechniqueMessage>(this, ReceiveTechniqueMessage);
        }

        internal ShaderCollection ShaderCollection
        {
            get { return shaderCollection; }
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

        public int TechniqueCount
        {
            get { return techniques.Count; }
        }

        public ObservableCollection<IShaderViewModel> Shaders
        {
            get { return shaderList; }
            set
            {
                shaderList = value;
                RaisePropertyChanged("Shaders");
            }
        }

        private void ReceiveTechniqueMessage(TechniqueMessage message)
        {
            switch (message.Action)
            {
                case TechniqueAction.Remove:
                    if (techniques.Contains(message.Technique))
                        techniques.Remove(message.Technique);
                    foreach (var shader in shaderList.Where(shader => shader.Techniques.Contains(message.Technique)))
                        shader.Techniques.Remove(message.Technique);
                    break;

                case TechniqueAction.Unassign:
                    foreach (var shaderVM in shaderList
                        .OfType<ShaderDescriptionViewModel>()
                        .Where(shaderVM => shaderVM.IsAssignedTo(message.Technique)))
                    {
                        shaderVM.RemoveTechnique(message.Technique);
                    }
                    break;
            }
        }

        public void CompileAllCode()
        {
            ObservableCollection<ErrorViewModel> errorList = new ObservableCollection<ErrorViewModel>();
            int shaderCount = 0;

            foreach (var shaderVM in shaderList.OfType<ShaderCodeViewModel>())
            {
                IEnumerable<ErrorViewModel> errors;
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
                string outputPath = Properties.Settings.Default.OutputPath;
                string fullPath = Path.Combine(outputPath, string.Format("{0}.{1}", shaderCollection.Name, "ofx"));
                ResourceManager.Serialize(fullPath, shaderCollection);
                DisplayCompletionMessage(shaderCount, errorList.Count);
            }
        }

        public void CompileAll()
        {
            ObservableCollection<ErrorViewModel> errorList = new ObservableCollection<ErrorViewModel>();
            int shaderCount = 0;
            Dictionary<string, ShaderDescription> compiledShaders = new Dictionary<string, ShaderDescription>();
            foreach (var vmTm in Techniques)
            {
                string techniqueName = vmTm.Name;
                var techniqueShaders = from shader in shaderList.OfType<ShaderDescriptionViewModel>()
                                       where shader.IsAssignedTo(techniqueName)
                                       select shader;

                var shaderDescriptionViewModels = techniqueShaders as ShaderDescriptionViewModel[] ??
                                                  techniqueShaders.ToArray();
                if (!shaderDescriptionViewModels.Any())
                    continue;

                foreach (var vmShader in shaderDescriptionViewModels)
                {
                    ShaderDescription shaderObject;
                    IEnumerable<ErrorViewModel> errors;
                    if (compiledShaders.ContainsKey(vmShader.Name))
                        shaderCollection.Add(vmTm.Name, compiledShaders[vmShader.Name]);
                    else if (compiler.CompileShader(vmTm.Name, vmShader, out shaderObject, out errors))
                    {
                        shaderCollection.Add(vmTm.Name, shaderObject);
                        shaderCount++;
                        compiledShaders.Add(vmShader.Name, shaderObject);
                    }
                    else
                    {
                        foreach (var error in errors)
                            errorList.Add(error);
                    }
                }

                if (errorList.Count == 0)
                {
                    vmTm.UpdateKey(shaderDescriptionViewModels);
                    vmTm.UpdateToolTip();
                }

                // Allow Shader Preview
                var allowPreview = shaderDescriptionViewModels.All(shader => shader.CompilationStatus == CompilationStatus.Successful);
                if (allowPreview)
                    MessengerInstance.Send(new TechniqueMessage(TechniqueAction.AllowPreview, vmTm));
            }

            vmDebug.Errors = errorList;

            if (shaderCount > 0)
            {
                string outputPath = Properties.Settings.Default.OutputPath;
                string fullPath = Path.Combine(outputPath, string.Format("{0}.{1}", shaderCollection.Name, "ofx"));
                ResourceManager.Serialize(fullPath, shaderCollection);
                Graphics.Shaders.ShaderCollection sc;
                ResourceManager.Read(fullPath, out sc);
                DisplayCompletionMessage(shaderCount, errorList.Count);
            }
        }

        private void DisplayCompletionMessage(int shaders, int errors)
        {
            string message = shaders == 1
                ? string.Format("The shader was saved in {0}", Properties.Settings.Default.OutputPath)
                : string.Format("A total of {0} shaders were saved in {1}", shaders, Properties.Settings.Default.OutputPath);

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
            IncludeHandler.BaseDirectory = Path.GetDirectoryName(openDialog.FileNames.First());
            foreach (string file in openDialog.FileNames)
            {
                string sourceCode = string.Empty;
                using (StreamReader sr = new StreamReader(file))
                    sourceCode = sr.ReadToEnd();

                ShaderCodeViewModel shaderVM = new ShaderCodeViewModel
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    ShaderModel = Properties.Settings.Default.DefaultShaderModel,
                    SourceCode = sourceCode
                };
                shaderList.Add(shaderVM);
            }
            return true;
        }

        public bool Open(bool clear = true)
        {
            OpenFileDialog openDialog = new OpenFileDialog { Filter = "Odyssey Shader Graph|*.osg" };
            bool? result = openDialog.ShowDialog();
            if (result == false)
                return false;

            List<TechniqueDescription> shaderGraph;

            try
            {
                using (FileStream fs = new FileStream(openDialog.FileName, FileMode.Open))
                {
                    ShaderGraphSerializer sgSerializer = new ShaderGraphSerializer(fs, SerializerMode.Read);
                    shaderGraph = new List<TechniqueDescription>(sgSerializer.Load(fs));
                }
            }
            catch (IOException ex)
            {
                const string errorMsg = "Failed to open file {0}";
                MessageBox.Show(string.Format(errorMsg, Path.GetFileName(openDialog.FileName)), "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Daedalus.Error(ex);
                return false;
            }

            if (!shaderGraph.Any())
            {
                MessageBox.Show("No techniques were found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var techniques = shaderGraph.Select((td, index) =>
                new TechniqueMappingViewModel
                {
                    Index = index + 1,
                    TechniqueMapping = new TechniqueMapping(td.Name) { Key = td.Key }
                }).ToList();

            foreach (var technique in techniques.Where(technique => !shaderCollection.Contains(technique.Name)))
                RegisterTechnique(technique);

            var parsedShaders = (from td in shaderGraph from s in td.Shaders group s by s.Name into shaderGroup select shaderGroup.First());

            var shaderVMs = from shader in parsedShaders
                            select new ShaderDescriptionViewModel()
                            {
                                ShaderDescriptionModel = new Model.ShaderDescription { Shader = shader },
                                Techniques = new ObservableCollection<TechniqueMappingViewModel>(from td in shaderGraph
                                                                                                 where td.Shaders.Any(s => s.Name == shader.Name)
                                                                                                 select techniques.Find(t => t.Name == td.Name))
                            };
            if (clear)
                shaderList.Clear();

            foreach (var shaderVM in shaderVMs.Where(shaderVM => shaderList.All(s => s.Name != shaderVM.Name)))
                shaderList.Add(shaderVM);

            var vmLocator = ((ViewModelLocator)System.Windows.Application.Current.FindResource("Locator"));
            vmLocator.Main.ShaderCollectionName = Path.GetFileNameWithoutExtension(openDialog.FileName);

            RelayCommand cmd = (RelayCommand)vmLocator.Main.CompileShaderGraphCommand;
            cmd.RaiseCanExecuteChanged();
            return true;
        }

        public void Save()
        {
            SaveFileDialog saveDialog = new SaveFileDialog { Filter = "Odyssey Shader Graph|*.osg" };
            bool? result = saveDialog.ShowDialog();
            if (result == false)
                return;

            Mouse.SetCursor(Cursors.Wait);
            try
            {
                var techniqueGraph = (from t in techniques
                                      select new TechniqueDescription(t.Name, t.TechniqueMapping.Key,
                                          from s in shaderList.OfType<ShaderDescriptionViewModel>()
                                          where s.IsAssignedTo(t)
                                          select s.ShaderDescriptionModel.Shader)).ToArray();

                foreach (var shader in techniqueGraph.SelectMany(t => t.Shaders))
                    shader.Build();

                using (FileStream fs = new FileStream(saveDialog.FileName, FileMode.Create))
                {
                    ShaderGraphSerializer sgSerializer = new ShaderGraphSerializer(fs, SerializerMode.Write);
                    sgSerializer.Save(techniqueGraph);
                }
            }
            catch (Exception e)
            {
                LogEvent.Tool.Error(e);
            }

            Mouse.SetCursor(Cursors.Arrow);
        }

        public bool CanCompile()
        {
            return Shaders != null && Shaders.All(shader => !shader.IsEmpty);
        }

        private void RegisterTechnique(TechniqueMappingViewModel techniqueVM)
        {
            techniques.Add(techniqueVM);
            shaderCollection.Add(techniqueVM.TechniqueMapping);
        }

        public void CreateTechnique()
        {
            CreateTechniqueView view = new CreateTechniqueView();
            view.ShowDialog();
            if (!view.DialogResult.HasValue || !view.DialogResult.Value)
                return;

            var vmCT = (CreateTechniqueViewModel)view.DataContext;
            string techniqueName = string.IsNullOrEmpty(vmCT.Name)
                ? string.Format("{0}{1}", NewTechnique, techniques.Count(t => t.Name.StartsWith(NewTechnique)) + 1)
                : vmCT.Name;
            TechniqueMappingViewModel vmTM = new TechniqueMappingViewModel
            {
                TechniqueMapping = new TechniqueMapping(techniqueName) { Key = vmCT.Key },
                Name = techniqueName,
                Index = techniques.Count + 1,
            };
            RegisterTechnique(vmTM);
        }
    }
}