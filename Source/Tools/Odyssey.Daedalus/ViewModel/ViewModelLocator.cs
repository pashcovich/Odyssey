/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Odyssey.Tools.ShaderGenerator"
                           x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Odyssey.Tools.ShaderGenerator.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CompilationViewModel>();
            SimpleIoc.Default.Register<DebugViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<CreateTechniqueViewModel>();
            SimpleIoc.Default.Register<PreviewViewModel>();
        }

        public CompilationViewModel Compilation { get { return ServiceLocator.Current.GetInstance<CompilationViewModel>(); } }

        public DebugViewModel Debug { get { return ServiceLocator.Current.GetInstance<DebugViewModel>(); } }

        public MainViewModel Main { get { return ServiceLocator.Current.GetInstance<MainViewModel>(); } }

        public SettingsViewModel Settings { get { return ServiceLocator.Current.GetInstance<SettingsViewModel>(); } }

        public CreateTechniqueViewModel CreateTechnique { get { return ServiceLocator.Current.GetInstance<CreateTechniqueViewModel>(); } }

        public PreviewViewModel Preview { get { return ServiceLocator.Current.GetInstance<PreviewViewModel>(); } }

        public static void Cleanup()
        {
            // TODO ClearShaderData the ViewModels
        }
    }
}