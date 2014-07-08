using System;
using System.Windows.Data;

namespace Odyssey.Tools.ShaderGenerator.View.Converters
{
    public class LineColumnValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int line = (int)values[0];
            int column = (int)values[1];

            if (line == 0 && column == 0)
                return string.Empty;
            else return string.Format("({0},{1})", line, column);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
