using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Odyssey.Tools.ShaderGenerator.View.Converters
{
    public class ShaderModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ShaderModel shaderModel = value == null ? ShaderModel.SM20_level_9_1 : (ShaderModel)value;

            switch (shaderModel)
            {
                default:
                case ShaderModel.SM20_level_9_1:
                    return "SM 2.0 (9.1)";
                case ShaderModel.SM20_level_9_3:
                    return "SM 2.0 (9.3)";
                case ShaderModel.SM30:
                    return "SM 3.0";
                case ShaderModel.SM40:
                    return "SM 4.0";
                case ShaderModel.SM41:
                    return "SM 4.1";
                case ShaderModel.SM50:
                    return "SM 5.0";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
