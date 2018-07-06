#region Using

using MixedReality.Common.CustomExceptions;
using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

#endregion

namespace MixedReality.Common.Helpers
{
    public class CameraCapture
    {
        #region Properties

        public MediaCapture MediaCapture { get; private set; } = new MediaCapture();

        public bool IsPreviewActive { get; private set; } = false;

        public bool IsInitialized { get; private set; } = false;

        #endregion

        #region Methods (Public)

        public async Task Initialize()
        {
            if (!IsInitialized)
            {
                var settings = new MediaCaptureInitializationSettings()
                {
                    StreamingCaptureMode = StreamingCaptureMode.Video
                };

                try
                {                    
                    await MediaCapture.InitializeAsync(settings);
                    
                    IsInitialized = true;
                }
                catch (Exception)
                {
                    IsInitialized = false;
                }
            }
        }

        public async Task Start()
        {
            CheckInitialization();

            if (!IsPreviewActive)
            {
                await MediaCapture.StartPreviewAsync();

                IsPreviewActive = true;
            }
        }

        public async Task Stop()
        {
            CheckInitialization();

            if (IsPreviewActive)
            {
                await MediaCapture.StopPreviewAsync();

                IsPreviewActive = false;
            }
        }        

        public async Task<SoftwareBitmap> CapturePhotoToSoftwareBitmap()
        {
            CheckInitialization();

            // Create bitmap-encoded image
            var imageEncodingProperties = ImageEncodingProperties.CreateBmp();

            // Capture photo
            var memoryStream = new InMemoryRandomAccessStream();
            await MediaCapture.CapturePhotoToStreamAsync(imageEncodingProperties, memoryStream);

            // Decode stream to bitmap
            var bitmapDecoder = await BitmapDecoder.CreateAsync(memoryStream);

            return await bitmapDecoder.GetSoftwareBitmapAsync();
        }

        #endregion

        #region Method (Private)        

        private void CheckInitialization()
        {
            if (!IsInitialized)
            {
                throw new NotInitializedException();
            }
        }

        #endregion
    }
}
