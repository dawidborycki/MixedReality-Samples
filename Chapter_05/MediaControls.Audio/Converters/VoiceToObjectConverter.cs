#region Using

using System;
using Windows.UI.Xaml.Data;

#endregion

namespace MediaControls.Audio.Converters
{
    public class VoiceToObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
