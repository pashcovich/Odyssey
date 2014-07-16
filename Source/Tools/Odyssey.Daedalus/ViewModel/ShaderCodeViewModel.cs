using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Odyssey.Daedalus.Model;
using Odyssey.Daedalus.View;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using System.Collections.ObjectModel;
using Shader = Odyssey.Daedalus.Shaders.Shader;

namespace Odyssey.Daedalus.ViewModel
{
    public class ShaderCodeViewModel : ViewModelBase, IShaderViewModel
    {
        private CompilationStatus compilationStatus;
        private FeatureLevel featureLevel;
        private string name;
        private ShaderType shaderType;
        private string sourceCode;
        private ObservableCollection<TechniqueMappingViewModel> techniques;

        public ShaderCodeViewModel()
        {
            techniques = new ObservableCollection<TechniqueMappingViewModel>();
            TechniqueMapping tMapping = new TechniqueMapping("Unassigned");
            techniques.Add(new TechniqueMappingViewModel { TechniqueMapping = tMapping });
        }

        public string ColorizedSourceCode
        {
            get { return new ColorCode.CodeColorizer().Colorize(SourceCode, ColorCode.Languages.Cpp); }
        }
        public CompilationStatus CompilationStatus
        {
            get
            {
                return compilationStatus;
            }
            set
            {
                compilationStatus = value;
                RaisePropertyChanged("CompilationStatus");
            }
        }
        public FeatureLevel FeatureLevel { get { return featureLevel; } set { featureLevel = value; RaisePropertyChanged("FeatureLevel"); } }
        public bool IsEmpty { get { return false; } }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        public ShaderModel ShaderModel
        {
            get { return Shaders.Shader.FromFeatureLevel(featureLevel); }
            set
            {
                featureLevel = Shaders.Shader.FromShaderModel(value, Type);
                RaisePropertyChanged("ShaderModel");
            }
        }

        public string SourceCode
        {
            get
            {
                return sourceCode;
            }
            set
            {
                sourceCode = value;
                RaisePropertyChanged("SourceCode");
            }
        }
        public ObservableCollection<TechniqueMappingViewModel> Techniques
        {
            get
            {
                return techniques;
            }
            set
            {
                techniques = value;
                RaisePropertyChanged("Techniques");
            }
        }

        public ShaderType Type
        {
            get { return shaderType; }
            set
            {
                shaderType = value; 
                RaisePropertyChanged("Type");
                FeatureLevel = Shaders.Shader.FromShaderModel(ShaderModel, shaderType);
            }
        }

        public System.Windows.Input.ICommand ViewCodeCommand
        {
            get
            {
                return new RelayCommand<ShaderCodeViewModel>((sDesc) =>
                {
                    SourceView sourceView = new SourceView { DataContext = this };
                    sourceView.Show();
                });
            }
        }
    }
}