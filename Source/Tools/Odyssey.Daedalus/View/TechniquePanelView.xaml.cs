using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Odyssey.Daedalus.View
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
            if (e.LeftButton != MouseButtonState.Pressed || Mouse.DirectlyOver is Button)
                return;
            var item = ItemsControl.ContainerFromElement(techniquePanel, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                DragDrop.DoDragDrop(item, item.Content, DragDropEffects.Link);
                item.IsSelected = false;
            }
            e.Handled = false; 
        }
    }
}
