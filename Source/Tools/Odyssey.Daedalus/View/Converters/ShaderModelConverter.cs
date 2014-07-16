using System;
using System.Windows.Data;
using Odyssey.Graphics.Effects;

namespace Odyssey.Daedalus.View.Converters
{
    public class ShaderModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ShaderModel shaderModel = value == null ? ShaderModel.SM_4_0_Level_9_1 : (ShaderModel)value;

            switch (shaderModel)
            {
                default:
                case ShaderModel.SM_2_0:
                    return "2.0 (Direct3D 9)";
                case ShaderModel.SM_2_A:
                    return "2a (Direct3D 9)";
                case ShaderModel.SM_2_B:
                    return "2b (Direct3D 9)";
                case ShaderModel.SM_3_0:
                    return "3.0 (Direct3D 9)";
                case ShaderModel.SM_4_0_Level_9_1:
                    return "4.0 (Direct3D 9.1)";
                case ShaderModel.SM_4_0_Level_9_3:
                    return "4.0 (Direct3D 9.3)";
                case ShaderModel.SM_4_0:
                    return "4.0 (Direct3D 10.0)";
                case ShaderModel.SM_4_1:
                    return "4.1 (Direct3D 10.1)";
                case ShaderModel.SM_5_0:
                    return "5.0 (Direct3D 11.0)";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ShaderModel shaderModel = value == null ? ShaderModel.SM_4_0_Level_9_1 : (ShaderModel)value;
            System.Console.WriteLine(shaderModel);
            return shaderModel;
        }
    }
}
