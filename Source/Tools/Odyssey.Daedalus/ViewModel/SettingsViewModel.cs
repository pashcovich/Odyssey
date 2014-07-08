using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Odyssey.Tools.ShaderGenerator.View.Controls;
using System.Windows;
using System.Windows.Input;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{


    public class SettingsViewModel : ViewModelBase
    {

        public ICommand SaveSettingsCommand
        {
            get
            {
                return new RelayCommand<Window>((window) =>
                {
                    Properties.Settings.Default.Save();
                    window.Close();
                });
            }
        }

        public ICommand BrowseFolderCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var dialog = new FolderBrowserDialog();
                    dialog.ShowDialog();
                    if (string.IsNullOrEmpty(dialog.FileName))
                        return;
                    Properties.Settings.Default.OutputPath = dialog.FileName;
                });
            }
        }
    }
}
