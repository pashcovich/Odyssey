#region Using Directives

using System;
using Odyssey.Graphics.Drawing;
using Odyssey.UserInterface.Controls;

#endregion

namespace Odyssey.UserInterface.Data
{
    public class FigureKeyToDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter)
        {
            if (!TypeConverters.CanConvertFrom<Path>(value.GetType()))
                throw new NotSupportedException("value");

            var figureKey = (string) value;
            var overlay = (Overlay) parameter;
            return Path.FromFigure(overlay, figureKey);
        }

        public object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}