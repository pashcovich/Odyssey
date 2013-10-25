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
    /// Interaction logic for TechniquePanelView.xaml
    /// </summary>
    public partial class TechniquePanelView : UserControl
    {
        public TechniquePanelView()
        {
            InitializeComponent();
        }

        private void techniquePanel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            var item = ItemsControl.ContainerFromElement(techniquePanel, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                DragDrop.DoDragDrop(item, (TechniqueMappingViewModel)item.Content, DragDropEffects.Link);
            }
        }
    }
}
