#region Using

using MixedReality.Common.Helpers;
using MixedReality.Common.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.Networking.Sockets;

#endregion

namespace Communication.Provider.ViewModels
{
    public class ProviderViewModel : SocketCommunicationBaseViewModel
    {
        #region Properties

        public AccelerometerReading AccelerometerReading
        {
            get => accelerometerReading;
            set => SetProperty(ref accelerometerReading, value);
        }

        #endregion

        #region Fields

        private AccelerometerReading accelerometerReading;

        private Accelerometer accelerometer;        

        private DatagramSocket providerDatagramSocket = new DatagramSocket();

        #endregion

        #region Constructor
        
        public ProviderViewModel()
        {
            accelerometer = Accelerometer.GetDefault();

            if (accelerometer == null)
            {
                throw new Exception("Cannot access accelerometer");
            }
            
            accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        #endregion

        #region Finalizer

        ~ProviderViewModel()
        {
            providerDatagramSocket.Dispose();
        }

        #endregion

        #region Methods (Public)

        public async Task Initialize()
        {
            await providerDatagramSocket.ConnectAsync(
                Settings.HostName, Settings.Port);
        }

        #endregion

        #region Methods (Private)

        private async void Accelerometer_ReadingChanged(Accelerometer sender,
            AccelerometerReadingChangedEventArgs args)
        {
            await ThreadHelper.InvokeOnMainThread(() =>
            {                
                AccelerometerReading = args.Reading;
            });
            
            SendAccelerometerReading(AccelerometerReading,
                providerDatagramSocket.OutputStream);
        }

        #endregion
    }
}
