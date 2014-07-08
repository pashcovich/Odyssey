using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Odyssey.Tools.ShaderGenerator.View.Controls
{
    public class ImageButton : Button
    {
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageButton),
            new PropertyMetadata(null));

        public ImageSource Image
        {
            get { return (ImageSource) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value);}
        }


    }
}
