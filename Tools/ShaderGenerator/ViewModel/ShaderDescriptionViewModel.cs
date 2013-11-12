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
    public class ShaderDescriptionViewModel : ViewModelBase, IShaderViewModel
    {
        ObservableCollection<TechniqueMappingViewModel> techniques;
        public ShaderDescription ShaderDescriptionModel { get; internal set; }

        public bool IsEmpty
        {
            get
            {
                if (techniques.Count == 1)
                    return string.Equals(techniques[0].Name, "Unassigned");
                else
                    return false;

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
            get { return Shader.FromFeatureLevel(ShaderDescriptionModel.FeatureLevel); }
            set
            {
                ShaderDescriptionModel.FeatureLevel = Shader.FromShaderModel(value, Type);
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
            var vmLocator = ((ViewModelLocator)Application.Current.FindResource("Locator"));
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

        public bool IsAssignedTo(TechniqueMappingViewModel tMapping)
        {
            return techniques.Contains(tMapping);
        }

        public bool IsAssignedTo(string techniqueName)
        {
            return techniques.Any(tm => tm.Name == techniqueName);
        }


        void AddUnassignedTechnique()
        {
            TechniqueMapping tMapping = new TechniqueMapping("Unassigned");
            techniques.Add(new TechniqueMappingViewModel { TechniqueMapping = tMapping });
        }

        static VertexShaderFlags GetVertexShaderFeature(ShaderDescriptionViewModel vmShader)
        {
            var attFeature = Attribute.GetCustomAttributes(vmShader.ShaderDescriptionModel.Shader.GetType(), typeof(VertexShaderAttribute));
            var vsAttributes = (from att in attFeature.OfType<VertexShaderAttribute>() select att.Features);
            if (!vsAttributes.Any())
                return VertexShaderFlags.None;
            VertexShaderFlags features = vsAttributes.Aggregate((a, b) => a | b);
            return features;
        }

        static PixelShaderFlags GetPixelShaderFeature(ShaderDescriptionViewModel vmShader)
        {
            var attFeature = Attribute.GetCustomAttributes(vmShader.ShaderDescriptionModel.Shader.GetType(), typeof(PixelShaderAttribute));
            var psAttributes = (from att in attFeature.OfType<PixelShaderAttribute>() select att.Features);
            if (!psAttributes.Any())
                return PixelShaderFlags.None;
            PixelShaderFlags features = psAttributes.Aggregate((a, b) => a | b);
            return features;
        }


    }
}
