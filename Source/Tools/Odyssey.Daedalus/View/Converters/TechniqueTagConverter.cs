using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Odyssey.Tools.ShaderGenerator.View.Converters
{
    public class TechniqueTagConverter : IValueConverter
    {
        readonly Brush[] brushes = new[] {
            new SolidColorBrush(Colors.DimGray),
            new SolidColorBrush (Colors.RoyalBlue ),
            new SolidColorBrush (Colors.DarkRed),
            new SolidColorBrush(Colors.DarkGreen),
            new SolidColorBrush(Colors.Purple),
            new SolidColorBrush(Colors.Goldenrod),
            new SolidColorBrush(Colors.DarkSlateGray), 
        };

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int index = (int)value;
            return brushes[index];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
