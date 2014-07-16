#region Using Directives

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Odyssey.Daedalus.View;
using Odyssey.Daedalus.ViewModel.Messages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WindowsApplication = System.Windows.Application;

#endregion Using Directives

namespace Odyssey.Daedalus.ViewModel
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
        private readonly ObservableCollection<ButtonViewModel> commands;
        private readonly ButtonViewModel compileVM;
        private readonly CompilationViewModel vmCompilation;
        private bool isShaderGraphMode;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            var vmLocator = ((ViewModelLocator)WindowsApplication.Current.FindResource("Locator"));
            vmCompilation = vmLocator.Compilation;
            IsShaderGraphMode = true;

            compileVM = new ButtonViewModel
            {
                Tooltip = "Compile shaders",
                Content = (UIElement)WindowsApplication.Current.FindResource("iPlay"),
                MainCommand = new CommandViewModel { Label = "Compile", Command = CompileShaderGraphCommand },
            };
            commands = new ObservableCollection<ButtonViewModel>
            {
                new ButtonViewModel
                {
                    Tooltip = "Open shader graph",
                    Content = (UIElement) WindowsApplication.Current.FindResource("iOpen"),
                    MainCommand =
                        new CommandViewModel
                        {
                            Label = "Open",
                            Command = new RelayCommand(() => { if (vmCompilation.Open()) IsShaderGraphMode = true; })
                        },
                    SecondaryCommands =
                        new List<CommandViewModel>
                        {
                            new CommandViewModel
                            {
                                Label = "Add",
                                Command =
                                    new RelayCommand(() => { if (vmCompilation.Open(false)) IsShaderGraphMode = true; })
                            }
                        },
                },
                new ButtonViewModel
                {
                    MainCommand = new CommandViewModel {Label = "Import", Command = new RelayCommand(LaunchImport)},
                    Tooltip = "Import HLSL source code",
                    Content = (UIElement) WindowsApplication.Current.FindResource("iImport"),
                },
                new ButtonViewModel
                {
                    MainCommand = new CommandViewModel {Label = "Save", Command = SaveCommand},
                    Tooltip = "Save shader graph",
                    Content = (UIElement) WindowsApplication.Current.FindResource("iSave"),
                },
                compileVM,
                new ButtonViewModel
                {
                    MainCommand =
                        new CommandViewModel
                        {
                            Label = "Define",
                            Command = new RelayCommand(() => vmCompilation.CreateTechnique(), () => IsShaderGraphMode)
                        },
                    Tooltip = "Define a new technique",
                    Content = (UIElement) WindowsApplication.Current.FindResource("iNewTechnique"),
                },
                new ButtonViewModel
                {
                    MainCommand = new CommandViewModel
                    {
                        Label = "Settings",
                        Command = new RelayCommand(() =>
                        {
                            Settings settings = new Settings();
                            settings.ShowDialog();
                        })
                    },
                    Tooltip = "Open Settings",
                    Content = (UIElement) WindowsApplication.Current.FindResource("iSettings"),
                },
            };

            MessengerInstance.Register<TechniqueMessage>(this, ReceiveMessage);

            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        public ObservableCollection<ButtonViewModel> Commands
        {
            get { return commands; }
        }

        public string ShaderCollectionName
        {
            get { return vmCompilation.ShaderCollection.Name; }
            set
            {
                vmCompilation.ShaderCollection.Name = value;
                RaisePropertyChanged("ShaderCollectionName");
            }
        }

        internal ICommand CompileShaderGraphCommand
        {
            get
            {
                return new RelayCommand(() => vmCompilation.CompileAll(),
                    () => IsShaderGraphMode && vmCompilation.CanCompile());
            }
        }

        internal ICommand CompileShaderCodeCommand
        {
            get { return new RelayCommand(() => vmCompilation.CompileAllCode(), () => !IsShaderGraphMode); }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(() => vmCompilation.Save(), vmCompilation.CanCompile); }
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

        private void LaunchImport()
        {
            if (vmCompilation.Import())
                IsShaderGraphMode = false;
            compileVM.MainCommand.Command = IsShaderGraphMode ? CompileShaderGraphCommand : CompileShaderCodeCommand;
            ((RelayCommand)compileVM.MainCommand.Command).RaiseCanExecuteChanged();
        }

        private void ReceiveMessage(TechniqueMessage techniqueMessage)
        {
            switch (techniqueMessage.Action)
            {
                case TechniqueAction.Preview:
                    var vmTechnique = techniqueMessage.Technique;

                    PreviewViewModel.DisplayTechnique(vmCompilation.ShaderCollection, vmTechnique.TechniqueMapping.Name);
                    break;
            }
        }
    }
}