using System.Windows;

namespace Odyssey.Tools.ShaderGenerator.View
{
    /// <summary>
    /// Interaction logic for CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog : Window
    {
        public CustomDialog()
        {
            InitializeComponent();
        }

        public string Header { get { return title.Text; } set { title.Text = value; } }
        public string Result { get { return response.Text; } }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            response.Focus();
        }
    }
}
