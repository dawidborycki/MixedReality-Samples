#region Using

using MediaControls.Video.Helpers;
using MixedReality.Common.Helpers;
using MixedReality.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.FaceAnalysis;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

#endregion

namespace MediaControls.Video.ViewModels
{
    public class VideoViewModel : BaseViewModel
    {        
        #region Properties

        public BitmapImage Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public bool IsPreviewActive
        {
            get => isPreviewActive;
            set => SetProperty(ref isPreviewActive, value);
        }

        public CameraCapture CameraCapture { get; private set; }    
        
        public ObservableCollection<Rectangle> Faces { get; private set; } = new ObservableCollection<Rectangle>();

        #endregion

        #region Fields

        private BitmapImage image;
        
        private bool isPreviewActive;        

        #endregion

        #region Constructor

        public VideoViewModel()
        {
            Image = GetImage();
        }

        #endregion

        #region Methods (Public)

        public async void PreviewStart()
        {
            // Initialize camera capture
            await InitializeCameraCapture();

            // Start preview
            await CameraCapture.Start();

            // Update UI
            IsPreviewActive = CameraCapture.IsPreviewActive;
        }

        public async void PreviewStop()
        {
            await CameraCapture.Stop();

            IsPreviewActive = CameraCapture.IsPreviewActive;

            Faces.Clear();
        }

        public async void DetectFaces()
        {
            // Capture and display bitmap
            var softwareBitmap = await CameraCapture.CapturePhotoToSoftwareBitmap();

            // Detect faces
            var detectedFaces = await ProcessFaceBitmap(softwareBitmap);

            // Display face rectangles
            DisplayFaceRectangles(detectedFaces);
        }

        #endregion

        #region Methods (Private)

        private BitmapImage GetImage()
        {
            // uncomment this line to get Lena image instead of american robin
            // var uri = "https://upload.wikimedia.org/wikipedia/en/2/24/Lenna.png";
            var uri = "http://bit.ly/american-robin";            

            return new BitmapImage(new Uri(uri));
        }

        private async Task<IList<DetectedFace>> ProcessFaceBitmap(SoftwareBitmap softwareBitmap)
        {
            // Initialize face detector
            var detector = await FaceDetector.CreateAsync();

            // Ensure that bitmap format is supported by the detector
            if (!FaceDetector.IsBitmapPixelFormatSupported(softwareBitmap.BitmapPixelFormat))
            {
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap,
                    FaceDetector.GetSupportedBitmapPixelFormats().First());
            }

            // Detect faces
            return await detector.DetectFacesAsync(softwareBitmap);
        }

        private void DisplayFaceRectangles(IList<DetectedFace> detectedFaces)
        {
            Faces.Clear();

            foreach (var detectedFace in detectedFaces)
            {
                Faces.Add(FaceBoxHelper.ToRectangle(detectedFace));
            }
        }

        private async Task InitializeCameraCapture()
        {
            CameraCapture = CameraCapture ?? new CameraCapture();

            await CameraCapture.Initialize();

            // Inform UI that the CameraCapture is ready 
            OnPropertyChanged("CameraCapture");
        }

        #endregion
    }
}