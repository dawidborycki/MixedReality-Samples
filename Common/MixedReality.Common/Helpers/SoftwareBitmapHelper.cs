#region Using

using MixedReality.Common.ArgumentValidation;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

#endregion

namespace MixedReality.Common.Helpers
{
    public static class SoftwareBitmapHelper
    {
        #region Methods (Public)

        public static WriteableBitmap ToWriteableBitmap(SoftwareBitmap softwareBitmap)
        {
            Check.IsNull(softwareBitmap, "softwareBitmap");

            var writeableBitmap = new WriteableBitmap(softwareBitmap.PixelWidth, softwareBitmap.PixelHeight);

            softwareBitmap.CopyToBuffer(writeableBitmap.PixelBuffer);

            return writeableBitmap;
        }

        public static async Task<Stream> GetBitmapStream(SoftwareBitmap softwareBitmap)
        {
            Check.IsNull(softwareBitmap, "softwareBitmap");

            var bitmapImageInMemoryRandomAccessStream = new InMemoryRandomAccessStream();

            var bitmapEncoder = await BitmapEncoder.CreateAsync(
                BitmapEncoder.BmpEncoderId, bitmapImageInMemoryRandomAccessStream);

            var stream = new InMemoryRandomAccessStream();
            
            bitmapEncoder.SetSoftwareBitmap(softwareBitmap);

            await bitmapEncoder.FlushAsync();

            return bitmapImageInMemoryRandomAccessStream.AsStream();
        }

        #endregion
    }
}
