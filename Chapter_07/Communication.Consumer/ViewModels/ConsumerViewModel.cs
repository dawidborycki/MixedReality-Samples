#region Using

using MixedReality.Common.Helpers;
using MixedReality.Common.Sensors;
using MixedReality.Common.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.UI;
using Windows.UI.Xaml.Media;

#endregion

namespace Communication.Consumer.ViewModels
{
    public class ConsumerViewModel : SocketCommunicationBaseViewModel
    {
        #region Properties

        public TranslateTransform EllipseTransform
        {
            get => translateTransform;
            set => SetProperty(ref translateTransform, value);
        }

        public Brush RectangleBrush
        {
            get => rectangleBrush;
            set => SetProperty(ref rectangleBrush, value);
        }

        public Size EllipseSize
        {
            get => ellipseSize;
        }

        public Size RectangleSize
        {
            get => rectangleSize;
        }

        public double RectangleStrokeThickness
        {
            get => rectangleStrokeThickness;
        }

        #endregion

        #region Fields

        #region Communication

        private DatagramSocket consumerDatagramSocket = new DatagramSocket();

        #endregion

        #region Control appearance

        private TranslateTransform translateTransform = new TranslateTransform();

        private Rect frameBounds = new Rect();

        private Size ellipseSize = new Size(100, 100);
        private Size rectangleSize = new Size(125, 125);

        private double rectangleStrokeThickness = 10.0;

        private Brush rectangleBrush;

        private Brush greenBrush = new SolidColorBrush(Colors.YellowGreen);
        private Brush redBrush = new SolidColorBrush(Colors.Red);

        #endregion

        #endregion

        #region Constants

        private const double shiftScaleFactor = 50.0;

        #endregion

        #region Finalizer

        ~ConsumerViewModel()
        {
            consumerDatagramSocket.Dispose();
        }

        #endregion

        #region Methods (Public)

        public async Task Initialize()
        {
            consumerDatagramSocket.MessageReceived += ConsumerDatagramSocket_MessageReceived;

            await consumerDatagramSocket.BindServiceNameAsync(Settings.Port);

            UpdateRectangleBrush();
        }

        public void Update(Rect frameBounds)
        {
            if (frameBounds != null)
            {
                this.frameBounds = frameBounds;
            }
        }

        #endregion

        #region Methods (Private)

        private async void ConsumerDatagramSocket_MessageReceived(
            DatagramSocket sender,
            DatagramSocketMessageReceivedEventArgs args)
        {
            try
            {
                var accReading = ReceiveAccelerometerReading(args.GetDataStream());

                await UpdateEllipseTransform(accReading);

                await ThreadHelper.InvokeOnMainThread(UpdateRectangleBrush);

                Debug.WriteLine(accReading);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void UpdateRectangleBrush()
        {
            var horizontalMargin = (rectangleSize.Width - ellipseSize.Width
                - rectangleStrokeThickness) / 2.0;

            var verticalMargin = (rectangleSize.Height - ellipseSize.Height
                - rectangleStrokeThickness) / 2.0;

            if (Math.Abs(EllipseTransform.X) <= horizontalMargin
                && Math.Abs(EllipseTransform.Y) <= verticalMargin)
            {
                RectangleBrush = greenBrush;
            }
            else
            {
                RectangleBrush = redBrush;
            }
        }

        private async Task UpdateEllipseTransform(AccReading accReading)
        {
            // Z accelerometer reading is used to update Y component 
            // of the ellipse transform
            var horizontalShift = accReading.X * shiftScaleFactor;
            var verticalShift = accReading.Z * shiftScaleFactor;

            await ThreadHelper.InvokeOnMainThread(() =>
            {
                if (Math.Abs(EllipseTransform.X + horizontalShift)
                    <= (frameBounds.Width - ellipseSize.Width) / 2.0)
                {
                    EllipseTransform.X += horizontalShift;
                }

                if (Math.Abs(EllipseTransform.Y + verticalShift)
                    <= (frameBounds.Height - ellipseSize.Height) / 2.0)
                {
                    EllipseTransform.Y += verticalShift;
                }
            });
        }

        #endregion
    }
}