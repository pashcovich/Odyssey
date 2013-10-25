using Odyssey.Tools.ShaderGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Odyssey.Tools.ShaderGenerator.View.Converters
{
    public class CategoryToIconConverter : IValueConverter
    {
        const string root = "pack://application:,,,/Resources/Icons/";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Category category = (Category)value;
            string iconPath = string.Empty;

            switch (category)
            {
                default:
                    iconPath = "Error.png";
                    break;

                case Category.Warning:
                    iconPath = "Warning.png";
                    break;

                case Category.NodeException:
                    iconPath = "NodeException.png";
                    break;
            }

            return string.Format("{0}{1}", root, iconPath);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
