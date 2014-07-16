#region Using Directives

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Odyssey.Daedalus.View.Controls;
using Odyssey.Daedalus.Viewer;
using Odyssey.Daedalus.ViewModel.Messages;
using Odyssey.Graphics.Shaders;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WindowsApplication = System.Windows.Application;

#endregion Using Directives

namespace Odyssey.Daedalus.ViewModel
{
    public class PreviewViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ButtonViewModel> commands;

        public PreviewViewModel()
        {
            commands = new ObservableCollection<ButtonViewModel>()
            {
                new ButtonViewModel
                {
                    Tooltip = "Capture Frame",
                    Content = (UIElement) WindowsApplication.Current.FindResource("iCamera"),
                    MainCommand = new CommandViewModel
                    {
                        Label = "Capture",
                        Command = SaveSnapshot
                    },
                },
                new ButtonViewModel
                {
                    Tooltip = "Switch to a Cube model",
                    Content = (UIElement) WindowsApplication.Current.FindResource("iCube"),
                    MainCommand = new CommandViewModel
                    {
                        Label = "Cube",
                        Command = SaveSnapshot
                    },
                },
                new ButtonViewModel
                {
                    Tooltip = "Switch to a Sphere model",
                    Content = (UIElement) WindowsApplication.Current.FindResource("iSphere"),
                    MainCommand = new CommandViewModel
                    {
                        Label = "Sphere",
                        Command = SaveSnapshot
                    },
                },
            };
        }

        public ObservableCollection<ButtonViewModel> Commands
        {
            get { return commands; }
        }

        internal DirectXWindow DirectXWindow { get; set; }

        public ICommand SaveSnapshot
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var mViewer = new ViewerMessage(ViewerAction.CaptureFrame);
                    MessengerInstance.Send(mViewer);
                });
            }
        }

        public static void DisplayTechnique(ShaderCollection shaderCollection, string techniqueKey)
        {
            using (new WaitCursor())
            {
                var vmPreview = new PreviewViewModel();
                DirectXWindow dxWindow = new DirectXWindow() { DataContext = vmPreview };
                dxWindow.SetTechnique(shaderCollection, techniqueKey);
                dxWindow.Start();
            }
        }
    }
}