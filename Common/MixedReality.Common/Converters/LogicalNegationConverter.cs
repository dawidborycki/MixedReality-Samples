#region Using

using System;
using Windows.UI.Xaml.Data;

#endregion

namespace MixedReality.Common.Converters
{
    public class LogicalNegationConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !System.Convert.ToBoolean(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}