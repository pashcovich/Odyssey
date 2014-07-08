#region Using Directives

using Odyssey.Tools.ShaderGenerator.ViewModel;
using Odyssey.Utilities.Logging;
using ShaderGenerator.Data;
using SharpDX;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#endregion Using Directives

namespace Odyssey.Tools.ShaderGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Configuration.EnableObjectTracking = true;
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Configuration.EnableObjectTracking = true;
            Title += string.Format(" v{0}", version);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ResourceManager manager = new ResourceManager();
            Daedalus.Initialize();
            var data = manager.Load();

            var shaderList = new ObservableCollection<IShaderViewModel>();
            foreach (var shader in data)
            {
                shaderList.Add(new ShaderDescriptionViewModel { ShaderDescriptionModel = shader });
            }
            var vmLocator = ((ViewModelLocator)System.Windows.Application.Current.FindResource("Locator"));
            vmLocator.Compilation.Shaders = shaderList;
        }

        private void tbName_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                // Move to a parent that can take focus
                FrameworkElement parent = (FrameworkElement)tbName.Parent;
                while (!((IInputElement)parent).Focusable)
                    parent = (FrameworkElement)parent.Parent;

                DependencyObject scope = FocusManager.GetFocusScope(tbName);
                FocusManager.SetFocusedElement(scope, parent as IInputElement);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Log.Close();
            if (Daedalus.DirectXWindow != null)
            {
                Daedalus.DirectXWindow.Shutdown();
            }
        }
    }
}