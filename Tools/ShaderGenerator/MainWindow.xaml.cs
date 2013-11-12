using Odyssey.Tools.ShaderGenerator.Model;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using Odyssey.Tools.ShaderGenerator.Shaders.Techniques;
using Odyssey.Tools.ShaderGenerator.Shaders.Yaml;
using Odyssey.Tools.ShaderGenerator.ViewModel;
using Odyssey.Utils.Logging;
using ShaderGenerator.Data;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Odyssey.Tools.ShaderGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            SharpDX.Configuration.EnableObjectTracking = true;
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            SharpDX.Configuration.EnableObjectTracking = true;
            this.Title += string.Format(" v{0}", version);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ResourceManager manager = new ResourceManager();
            Daedalus.Initialize();
            //var data = manager.Load();

            //var shaderList = new ObservableCollection<IShaderViewModel>();
            //foreach (var shader in data)
            //{
            //    shaderList.Add(new ShaderDescriptionViewModel { ShaderDescriptionModel = shader });
            //}
            //var vmLocator = ((ViewModelLocator)Application.Current.FindResource("Locator"));
            //vmLocator.Compilation.Shaders = shaderList;

        }

        private void tbName_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                // Move to a parent that can take focus
                FrameworkElement parent = (FrameworkElement)tbName.Parent;
                while (parent != null && parent is IInputElement && !((IInputElement)parent).Focusable)
                    parent = (FrameworkElement)parent.Parent;

                DependencyObject scope = FocusManager.GetFocusScope(tbName);
                FocusManager.SetFocusedElement(scope, parent as IInputElement);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Close();
        }

    }

    
}
