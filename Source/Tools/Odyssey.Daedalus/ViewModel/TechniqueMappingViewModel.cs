#region Using Directives

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Odyssey.Daedalus.ViewModel.Messages;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Daedalus.Utilities;
using Shader = Odyssey.Daedalus.Shaders.Shader;

#endregion

namespace Odyssey.Daedalus.ViewModel
{
    public class TechniqueMappingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ButtonViewModel> commands;
        private bool canPreview;
        private int index;
        private string toolTip;

        public TechniqueMappingViewModel()
        {
            var vmLocator = ((ViewModelLocator) System.Windows.Application.Current.FindResource("Locator"));
            var vmCompilation = vmLocator.Compilation;

            commands = new ObservableCollection<ButtonViewModel>
            {
                new ButtonViewModel
                {
                    Tooltip = "Preview Technique",
                    Content = (UIElement) System.Windows.Application.Current.FindResource("iCubes"),
                    MainCommand =
                        new CommandViewModel
                        {
                            Label = "Preview",
                            Command = PreviewTechniqueCommand
                        },
                },
                new ButtonViewModel
                {
                    MainCommand = new CommandViewModel
                    {
                        Label = "Edit",
                        Command = new RelayCommand(vmCompilation.CreateTechnique)
                    },
                    Tooltip = "Edit this technique",
                    Content = (UIElement) System.Windows.Application.Current.FindResource("iEdit"),
                },
                new ButtonViewModel
                {
                    MainCommand = new CommandViewModel {Label = "Delete", Command = RemoveTechniqueCommand,},
                    Tooltip = "Delete this technique",
                    Content = (UIElement) System.Windows.Application.Current.FindResource("iDelete"),
                },
            };
            MessengerInstance.Register<TechniqueMessage>(this, ReceiveTechniqueMessage);
        }

        public TechniqueMapping TechniqueMapping { get; internal set; }

        public bool CanPreview
        {
            get { return canPreview; }
            set
            {
                canPreview = value;
                RaisePropertyChanged("CanPreview");
            }
        }

        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                RaisePropertyChanged("Index");
            }
        }

        public string Name
        {
            get { return TechniqueMapping.Name; }
            set
            {
                TechniqueMapping.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public ObservableCollection<ButtonViewModel> Commands
        {
            get { return commands; }
        }

        public string ToolTip
        {
            get { return string.IsNullOrEmpty(toolTip) ? Name : toolTip; }
            set
            {
                toolTip = value;
                RaisePropertyChanged("ToolTip");
            }
        }

        public ICommand PreviewTechniqueCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var msg = new TechniqueMessage(TechniqueAction.Preview, this);
                    MessengerInstance.Send(msg);
                }, () => CanPreview);
            }
        }

        public ICommand RemoveTechniqueCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var msg = new TechniqueMessage(TechniqueAction.Remove, this);
                    MessengerInstance.Send(msg);
                });
            }
        }

        public ICommand UnassignTechniqueCommand
        {
            get
            {
                return new RelayCommand<TechniqueMappingViewModel>(vm =>
                {
                    var msg = new TechniqueMessage(TechniqueAction.Unassign, vm);
                    MessengerInstance.Send(msg);
                });
            }
        }

        private void ReceiveTechniqueMessage(TechniqueMessage message)
        {
            switch (message.Action)
            {
                case TechniqueAction.AllowPreview:
                    CanPreview = true;
                    break;
            }
        }

        public bool ContainsShader(string shaderName)
        {
            return TechniqueMapping.Contains(shaderName);
        }

        public void UpdateKey(IEnumerable<ShaderDescriptionViewModel> shaders)
        {
            var shaderList = shaders.ToList();
            var vertexShader = shaderList.FirstOrDefault(s => s.Type == ShaderType.Vertex);
            var pixelShader = shaderList.FirstOrDefault(s => s.Type == ShaderType.Pixel);

            TechniqueMapping.Key = new TechniqueKey(vertexShader.KeyPart.VertexShader, pixelShader.KeyPart.PixelShader,
                Shaders.Shader.FromFeatureLevel(vertexShader.FeatureLevel),
                TechniqueMapping.Key.RasterizerState, TechniqueMapping.Key.BlendState,
                TechniqueMapping.Key.DepthStencilState);
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