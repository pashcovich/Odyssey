using Odyssey.Tools.ShaderGenerator.Model;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using Odyssey.Tools.ShaderGenerator.Shaders.Techniques;
using Odyssey.Tools.ShaderGenerator.ViewModel;
using ShaderGenerator.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ResourceManager manager = new ResourceManager();
            ShaderGen.Initialize();
            var data = manager.Load();

            var shaderList = new ObservableCollection<ShaderDescriptionViewModel>();
            foreach (var shader in data)
            {
                shaderList.Add(new ShaderDescriptionViewModel { ShaderDescriptionModel = shader });
            }
            var vmLocator = ((ViewModelLocator)Application.Current.FindResource("Locator"));
            vmLocator.Compilation.Shaders = shaderList;
        }


    }
}
