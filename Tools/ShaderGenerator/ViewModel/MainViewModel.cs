using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Odyssey.Tools.ShaderGenerator.Model;
using Odyssey.Tools.ShaderGenerator.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        ButtonViewModel compileVM;
        bool isShaderGraphMode;
        DebugViewModel vmDebug;
        CompilationViewModel vmCompilation;
        ObservableCollection<ButtonViewModel> commands;


        public ObservableCollection<ButtonViewModel> Commands
        {
            get { return commands; }
        }

        public string ShaderCollectionName { get { return vmCompilation.ShaderCollection.Name; }
            set {
                vmCompilation.ShaderCollection.Name = value;
                RaisePropertyChanged("ShaderCollectionName");
            }
        }

        internal ICommand CompileShaderGraphCommand
        {
            get { return new RelayCommand(() => vmCompilation.CompileAll(), () => IsShaderGraphMode && vmCompilation.CanCompile()); }
        }

        internal ICommand CompileShaderCodeCommand
        {
            get { return new RelayCommand(() => vmCompilation.CompileAllCode(), () => !IsShaderGraphMode); }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(() => vmCompilation.Save(), vmCompilation.CanCompile);}
        }

        public ICommand OpenSettingsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Settings settings = new Settings();
                    settings.ShowDialog();
                });
            }
        }

        public bool IsShaderGraphMode
        {
            get { return isShaderGraphMode; }
            set
            {
                isShaderGraphMode = value;
                RaisePropertyChanged("IsShaderGraphMode");
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            var vmLocator = ((ViewModelLocator)Application.Current.FindResource("Locator"));
            vmDebug = vmLocator.Debug;
            vmCompilation = vmLocator.Compilation;
            IsShaderGraphMode = true;

            compileVM = new ButtonViewModel{ 
                    Label = "Compile",
                    Tooltip = "Compile shaders", 
                    Content = (UIElement)Application.Current.FindResource("iPlay"),
                    Command = CompileShaderGraphCommand
                };
            commands = new ObservableCollection<ButtonViewModel>
            {
                new ButtonViewModel{ 
                    Label = "Open",
                    Tooltip = "Open shader graph", 
                    Content = (UIElement)Application.Current.FindResource("iOpen"),
                    Command = new RelayCommand(() => IsShaderGraphMode = vmCompilation.Open())
                },
                new ButtonViewModel{ 
                    Label = "Import",
                    Tooltip = "Import HLSL source code", 
                    Content = (UIElement)Application.Current.FindResource("iImport"),
                    Command = new RelayCommand(LaunchImport)
                },
                compileVM,
                new ButtonViewModel{ 
                    Label = "Define",
                    Tooltip = "Define a new technique", 
                    Content = (UIElement)Application.Current.FindResource("iNewTechnique"),
                    Command = new RelayCommand(() => vmCompilation.CreateTechnique(), () => IsShaderGraphMode)
                },

            };
            
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}


        }

        void LaunchImport()
        {
            IsShaderGraphMode = !vmCompilation.Import();
            compileVM.Command = IsShaderGraphMode ? CompileShaderGraphCommand : CompileShaderCodeCommand;
            ((RelayCommand)compileVM.Command).RaiseCanExecuteChanged();
        }
        
    }
}