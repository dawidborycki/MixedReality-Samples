#region Using

using System;
using System.Linq;
using Windows.UI.Xaml.Data;

#endregion

namespace MediaControls.Audio.Converters
{
    public class AudioSourceToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;

            if (value is Uri uri)
            {
                result = $"Now playing: {uri.Segments.Last()}";
            }
            
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
