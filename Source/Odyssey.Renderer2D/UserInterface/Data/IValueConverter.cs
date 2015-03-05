#region Using Directives

using System;

#endregion

namespace Odyssey.UserInterface.Data
{
    public interface IValueConverter
    {
        object Convert(object value, Type targetType, object parameter);
        object ConvertBack(object value, Type targetType, object parameter);
    }
}