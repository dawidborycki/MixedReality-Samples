using System;
using Windows.UI.Xaml.Data;

namespace UWP.Basics.Converters
{
    public class ToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var strValue = value as string;

            if (!string.IsNullOrEmpty(strValue))
            {
                strValue = strValue.ToUpper();
            }

            return strValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
