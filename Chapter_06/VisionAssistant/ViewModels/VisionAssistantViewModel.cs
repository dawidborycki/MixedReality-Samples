#region Using

using Microsoft.ProjectOxford.Vision;
using MixedReality.Common.Enums;
using MixedReality.Common.Events;
using MixedReality.Common.Helpers;
using MixedReality.Common.ViewModels;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VisionAssistant.BingSearch;
using VisionAssistant.BingSearch.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

#endregion

namespace VisionAssistant.ViewModels
{
    public class VisionAssistantViewModel : BaseViewModel
    {
        #region Properties      

        public bool IsDescribeImageButtonEnabled
        {
            get => isDescribeImageButtonEnabled;
            set => SetProperty(ref isDescribeImageButtonEnabled, value);
        }

        public bool IsSearchButtonEnabled
        {
            get => isSearchButtonEnabled;
            set => SetProperty(ref isSearchButtonEnabled, value);
        }

        public string ImageDescription
        {
            get => imageDescription;
            set => SetProperty(ref imageDescription, value);
        }

        public WriteableBitmap Image 
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public WebPage WebPage
        {
            get => webPage;
            set => SetProperty(ref webPage, value);
        }

        #endregion

        #region Fields

        #region Data binding

        private bool isDescribeImageButtonEnabled = true;
        private bool isSearchButtonEnabled = false;

        private string imageDescription;

        private WriteableBitmap image;

        private WebPage webPage;

        #endregion

        #region Camera capture, speech synthesis and recognition

        private CameraCapture cameraCapture = new CameraCapture();

        private Synthesizer synthesizer = new Synthesizer();

        private Recognizer recognizer = new Recognizer();

        #endregion 

        #region MCS Clients

        private VisionServiceClient visionServiceClient = new VisionServiceClient(
            Settings.VisionServiceClientKey, 
            Settings.VisionServiceEndPoint);

        private BingSearchServiceClient bingSearchServiceClient = new BingSearchServiceClient(
            Settings.BingSearchServiceClientKey, 
            Settings.BingSearchEndPoint);

        #endregion               

        #endregion

        #region Methods (Public)

        public async Task Initialize()
        {
            if (!cameraCapture.IsPreviewActive)
            {
                await cameraCapture.Initialize();

                // Dummy capture element
                var captureElement = new CaptureElement()
                {
                    Source = cameraCapture.MediaCapture
                };

                await cameraCapture.Start();

                // Initialize speech recognizer
                recognizer.VoiceCommandRecognized += Recognizer_VoiceCommandRecognized;

                await recognizer.BeginVoiceCommandRecognition();
            }
        }

        private async void Recognizer_VoiceCommandRecognized(object sender,
            VoiceCommandRecognizedEventArgs args)
        {
            switch (args.VoiceCommand)
            {
                case VoiceCommand.WhatISee:
                    await ThreadHelper.InvokeOnMainThread(DescribeImage);
                    break;

                case VoiceCommand.LookUp:
                    await ThreadHelper.InvokeOnMainThread(Search);
                    break;
            }
        }

        public async void DescribeImage()
        {
            IsDescribeImageButtonEnabled = false;

            await NotifyUser("OK");

            // Capture bitmap
            var bitmapStream = await CaptureAndDisplayBitmap();

            // Submit an image for analysis
            var descriptionResult = await visionServiceClient.DescribeAsync(bitmapStream);

            // Retrieve the first caption
            ImageDescription = descriptionResult.Description.Captions.FirstOrDefault().Text;

            await NotifyUser($"You see: {ImageDescription}");

            // Update buttons
            IsDescribeImageButtonEnabled = true;
            IsSearchButtonEnabled = !string.IsNullOrEmpty(ImageDescription);
        }

        public async void Search()
        {
            IsSearchButtonEnabled = false;

            var queryResults = await bingSearchServiceClient.
                Search($"{ImageDescription} + Wikipedia");

            WebPage = queryResults.WebPages.Items.FirstOrDefault();

            await NotifyUser($"Here is what I found: {WebPage.Snippet}");

            IsSearchButtonEnabled = true;
        }

        #endregion

        #region Methods (Private)

        private async Task NotifyUser(string message)
        {
            await synthesizer.Speak(message);
        }

        private async Task<Stream> CaptureAndDisplayBitmap()
        {
            // Capture and display bitmap
            var softwareBitmap = await cameraCapture.CapturePhotoToSoftwareBitmap();

            // Display bitmap
            Image = SoftwareBitmapHelper.ToWriteableBitmap(softwareBitmap);

            // Return bitmap stream
            return await SoftwareBitmapHelper.GetBitmapStream(softwareBitmap);
        }

        #endregion
    }
}
