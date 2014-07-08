using Odyssey.Tools.ShaderGenerator.Model;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Odyssey.Tools.ShaderGenerator.View.Converters
{
    public class CompilationStatusToBrushConverter : IValueConverter
    {
        public Brush Uncompiled { get; set; }
        public Brush Successful { get; set; }
        public Brush Failed { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CompilationStatus status = (CompilationStatus)value;

            switch (status)
            {
                default:
                    return Uncompiled;

                case CompilationStatus.Successful:
                    return Successful;

                case CompilationStatus.Failed:
                    return Failed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
