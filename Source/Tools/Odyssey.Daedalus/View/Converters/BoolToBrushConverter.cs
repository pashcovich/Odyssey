using System.Windows.Data;
using System.Windows.Media;

namespace Odyssey.Daedalus.View.Converters
{

    public class BoolToBrushConverter : IValueConverter
    {
        public Brush TrueValue { get; set; }
        public Brush FalseValue { get; set; }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool boolValue = (bool)value;
            return boolValue ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}