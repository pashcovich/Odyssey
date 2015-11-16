using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Odyssey.Daedalus.Model;
using Odyssey.Daedalus.View;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Daedalus.ViewModel
{
    public class ShaderDescriptionViewModel : ViewModelBase, IShaderViewModel
    {
        private ObservableCollection<TechniqueMappingViewModel> techniques;

        public Model.ShaderDescription ShaderDescriptionModel { get; internal set; }

        public bool IsEmpty
        {
            get
            {
                return techniques.Count == 1 && string.Equals(techniques[0].Name, "Unassigned");
            }
        }

        public ShaderDescriptionViewModel()
        {
            techniques = new ObservableCollection<TechniqueMappingViewModel>();
            AddUnassignedTechnique();
        }

        public string ColorizedSourceCode
        {
            get { return new ColorCode.CodeColorizer().Colorize(SourceCode, ColorCode.Languages.Cpp); }
        }

        public CompilationStatus CompilationStatus
        {
            get { return ShaderDescriptionModel.CompilationStatus; }
            set
            {
                ShaderDescriptionModel.CompilationStatus = value;
                RaisePropertyChanged("CompilationStatus");
            }
        }

        public string Name { get { return ShaderDescriptionModel.Name; } }

        public string SourceCode { get { return ShaderDescriptionModel.SourceCode; } }

        public FeatureLevel FeatureLevel
        {
            get { return ShaderDescriptionModel.FeatureLevel; }
            set
            {
                ShaderDescriptionModel.FeatureLevel = value;
                RaisePropertyChanged("FeatureLevel");
            }
        }

        public ShaderModel ShaderModel
        {
            get { return Shaders.Shader.FromFeatureLevel(ShaderDescriptionModel.FeatureLevel); }
            set
            {
                ShaderDescriptionModel.FeatureLevel = Shaders.Shader.FromShaderModel(value, Type);
                RaisePropertyChanged("ShaderModel");
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

        public ShaderType Type { get { return ShaderDescriptionModel.Type; } }

        internal TechniqueKey KeyPart { get { return ShaderDescriptionModel.KeyPart; } }

        public System.Windows.Input.ICommand ViewCodeCommand
        {
            get
            {
                return new RelayCommand<ShaderDescriptionViewModel>((sDesc) =>
                {
                    SourceView sourceView = new SourceView { DataContext = this };
                    sourceView.Show();
                });
            }
        }

        public void AssignTechnique(TechniqueMappingViewModel tMapping)
        {
            if (IsEmpty)
                techniques.Clear();
            techniques.Add(tMapping);
            var vmLocator = ((ViewModelLocator)System.Windows.Application.Current.FindResource("Locator"));
            RelayCommand cmd = (RelayCommand)vmLocator.Main.CompileShaderGraphCommand;
            cmd.RaiseCanExecuteChanged();
        }

        public void RemoveTechnique(TechniqueMappingViewModel tMapping)
        {
            Contract.Requires<ArgumentException>(IsAssignedTo(tMapping));
            techniques.Remove(tMapping);
            if (techniques.Count == 0)
                AddUnassignedTechnique();
        }

        [Pure]
        public bool IsAssignedTo(TechniqueMappingViewModel tMapping)
        {
            return techniques.Contains(tMapping);
        }

        public bool IsAssignedTo(string techniqueName)
        {
            return techniques.Any(tm => tm.Name == techniqueName);
        }

        private void AddUnassignedTechnique()
        {
            TechniqueMapping tMapping = new TechniqueMapping("Unassigned");
            techniques.Add(new TechniqueMappingViewModel { TechniqueMapping = tMapping });
        }
    }
}