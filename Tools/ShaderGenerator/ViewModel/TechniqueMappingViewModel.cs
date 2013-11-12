using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Odyssey.Content.Shaders;
using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.View;
using Odyssey.Tools.ShaderGenerator.ViewModel.Messages;
using Odyssey.Tools.ShaderGenerator.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Odyssey.Graphics.Rendering;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    public class TechniqueMappingViewModel : ViewModelBase
    {
        int index;
        string toolTip;
        public TechniqueMapping TechniqueMapping { get; internal set; }
        public int Index { get { return index; } set { index = value; RaisePropertyChanged("Index"); } }
        public string Name { get { return TechniqueMapping.Name; } set { TechniqueMapping.Name = value; RaisePropertyChanged("Name"); } }

        public string ToolTip
        {
            get
            {
                return string.IsNullOrEmpty(toolTip) ? Name : toolTip;
            }
            set
            {
                toolTip = value;
                RaisePropertyChanged("ToolTip");
            }
        }

        public bool ContainsShader(string shaderName)
        {
            return TechniqueMapping.Contains(shaderName);
        }

        public System.Windows.Input.ICommand RemoveTechniqueCommand
        {
            get
            {
                return new RelayCommand<TechniqueMappingViewModel>((vm) =>
                {
                    var msg = new TechniqueMessage(TechniqueAction.Remove, vm);
                    MessengerInstance.Send<TechniqueMessage>(msg);
                });
            }
        }

        public System.Windows.Input.ICommand UnassignTechniqueCommand
        {
            get
            {
                return new RelayCommand<TechniqueMappingViewModel>((vm) =>
                {
                    var msg = new TechniqueMessage(TechniqueAction.Unassign, vm);
                    MessengerInstance.Send<TechniqueMessage>(msg);
                });
            }
        }

        public void UpdateKey(IEnumerable<ShaderDescriptionViewModel> shaders)
        {
            var shaderList = shaders.ToList();
            var vertexShader = shaderList.Where(s => s.Type == ShaderType.Vertex).FirstOrDefault();
            var pixelShader = shaderList.Where(s => s.Type == ShaderType.Pixel).FirstOrDefault();

            TechniqueMapping.Key = new TechniqueKey(vs: vertexShader.KeyPart.VertexShader, ps: pixelShader.KeyPart.PixelShader,
                sm: Odyssey.Tools.ShaderGenerator.Shaders.Shader.FromFeatureLevel(vertexShader.FeatureLevel));
        }

        public void UpdateToolTip()
        {
            TechniqueKey key = TechniqueMapping.Key;
            StringBuilder sb = new StringBuilder();
            if (key.VertexShader != VertexShaderFlags.None)
            {
                sb.AppendLine("Vertex Shader flags:");
                sb.AppendLine(key.VertexShader.WriteValues());
            }

            if (key.PixelShader != PixelShaderFlags.None)
            {
                sb.AppendLine("Pixel Shader flags:");
                sb.AppendLine(key.PixelShader.WriteValues());
            }
            ToolTip = sb.ToString();
        }
    }
}
