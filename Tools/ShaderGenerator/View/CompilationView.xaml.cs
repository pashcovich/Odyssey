using Odyssey.Content.Shaders;
using Odyssey.Tools.ShaderGenerator.ViewModel;
using System;
using System.Collections.Generic;
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

namespace Odyssey.Tools.ShaderGenerator.View
{
    /// <summary>
    /// Interaction logic for CompilationView.xaml
    /// </summary>
    public partial class CompilationView : UserControl
    {
        public CompilationView()
        {
            InitializeComponent();
        }

        private void ShaderList_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            var item = ItemsControl.ContainerFromElement(ShaderList, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                ShaderDescriptionViewModel sVm = (ShaderDescriptionViewModel)item.Content;
                if (e.Data.GetDataPresent(typeof(TechniqueMappingViewModel)) && !sVm.Techniques.Contains(e.Data.GetData(typeof(TechniqueMappingViewModel))))
                    e.Effects = DragDropEffects.Link;
            }
        }

        private void ShaderList_Drop(object sender, DragEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(ShaderList, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null && e.Data.GetDataPresent(typeof(TechniqueMappingViewModel)))
            {
                TechniqueMappingViewModel tMapping = (TechniqueMappingViewModel)e.Data.GetData(typeof(TechniqueMappingViewModel));
                ShaderDescriptionViewModel sVM = ((ShaderDescriptionViewModel)item.Content);

                var vmLocator = ((ViewModelLocator)Application.Current.FindResource("Locator"));
                foreach (var shader in vmLocator.Compilation.Shaders)
                {
                    if (shader.Name != sVM.Name && shader.Type == sVM.Type && shader.Techniques.Contains(tMapping))
                        shader.RemoveTechnique(tMapping);
                }
                sVM.AssignTechnique(tMapping);
            }
        }
    }
}
