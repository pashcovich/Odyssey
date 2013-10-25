using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Odyssey.Tools.ShaderGenerator.Model;
using Odyssey.Tools.ShaderGenerator.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
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
        DebugViewModel vmDebug;
        CompilationViewModel vmCompilation;

        ObservableCollection<ShaderDescriptionViewModel> shaderList;

        public string ShaderCollectionName { get { return vmCompilation.ShaderCollection.Name; }
            set {
                vmCompilation.ShaderCollection.Name = value;
                RaisePropertyChanged("ShaderCollectionName");
            }
        }

        public ObservableCollection<ShaderDescriptionViewModel> ShaderList { get { return shaderList; }
            set
            {
                shaderList = value;
                RaisePropertyChanged("ShaderList");
            }
        }

        public ICommand CompileCommand
        {
            get { return new RelayCommand(() => vmCompilation.CompileAll(), vmCompilation.CanCompile); }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(() => vmCompilation.Save(), vmCompilation.CanCompile);}
        }

        public ICommand OpenCommand
        {
            get { return new RelayCommand(() => vmCompilation.Open()); }
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

        public ICommand CreateTechniqueCommand
        {
            get
            {
                return new RelayCommand(() => vmCompilation.CreateTechnique());
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

            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }
        
    }
}