using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Odyssey.Content.Shaders;
using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using Odyssey.Tools.ShaderGenerator.Model;
using Odyssey.Tools.ShaderGenerator.Shaders;
using Odyssey.Tools.ShaderGenerator.View;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public class ShaderCodeViewModel : ViewModelBase, IShaderViewModel
    {
        ObservableCollection<TechniqueMappingViewModel> techniques;
        CompilationStatus compilationStatus;
        string name;
        string sourceCode;
        FeatureLevel featureLevel;
        ShaderType shaderType;

        
        public ShaderCodeViewModel()
        {
            techniques = new ObservableCollection<TechniqueMappingViewModel>();
            TechniqueMapping tMapping = new TechniqueMapping("Unassigned");
            techniques.Add(new TechniqueMappingViewModel { TechniqueMapping = tMapping });
        }

        public bool IsEmpty { get { return false; } }

        public string ColorizedSourceCode
        {
            get { return new ColorCode.CodeColorizer().Colorize(SourceCode, ColorCode.Languages.Cpp); }
        }

        public CompilationStatus CompilationStatus
        {
            get { return compilationStatus; }
            set
            {
                compilationStatus = value;
                RaisePropertyChanged("CompilationStatus");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        public string SourceCode
        {
            get { return sourceCode; }
            set
            {
                sourceCode = value;
                RaisePropertyChanged("SourceCode");
            }
        }
        public FeatureLevel FeatureLevel { get { return featureLevel; } }
        public ObservableCollection<TechniqueMappingViewModel> Techniques
        {
            get { return techniques; }
            set
            {
                techniques = value;
                RaisePropertyChanged("Techniques");
            }
        }

        public ShaderType Type { get { return shaderType; } }

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
