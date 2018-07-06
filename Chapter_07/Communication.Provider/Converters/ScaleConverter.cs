#region Using

using System;
using Windows.UI.Xaml.Data;

#endregion

namespace Communication.Provider.Converters
{
    public class ScaleConverter : IValueConverter
    {
        #region Methods (Public)

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (double)value * GetScaleFactor(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (double)value / GetScaleFactor(parameter);
        }

        #endregion

        #region Methods (Private)

        private double GetScaleFactor(object parameter)
        {
            var result = 1.0;

            if (Double.TryParse(parameter.ToString(), out double scaleFactor))
            {
                result = scaleFactor;
            }

            return result;
        }

        #endregion
    }
}
