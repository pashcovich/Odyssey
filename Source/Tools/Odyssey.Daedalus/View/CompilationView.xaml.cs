using Odyssey.Tools.ShaderGenerator.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                ShaderDescriptionViewModel sVm = item.Content as ShaderDescriptionViewModel;
                if (sVm == null)
                {
                    e.Effects = DragDropEffects.None;
                    return;
                }
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

                var vmLocator = ((ViewModelLocator)System.Windows.Application.Current.FindResource("Locator"));
                foreach (var shader in vmLocator.Compilation.Shaders.OfType<ShaderDescriptionViewModel>())
                {
                    if (shader.Name != sVM.Name && shader.Type == sVM.Type && shader.Techniques.Contains(tMapping))
                        shader.RemoveTechnique(tMapping);
                }
                sVM.AssignTechnique(tMapping);
            }
        }
    }
}
