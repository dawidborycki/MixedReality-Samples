#region Using

using MixedReality.Common.Sensors;
using System.IO;
using System.Runtime.Serialization;
using Windows.Devices.Sensors;
using Windows.Storage.Streams;

#endregion

namespace MixedReality.Common.ViewModels
{
    public class SocketCommunicationBaseViewModel : BaseViewModel
    {
        #region Fields

        protected DataContractSerializer dataSerializer =
            new DataContractSerializer(typeof(AccReading));

        #endregion

        #region Methods (Protected)

        protected void SendAccelerometerReading(
            AccelerometerReading accelerometerReading,
            IOutputStream outputStream)
        {
            if (accelerometerReading != null)
            {
                var accReading = AccReading.FromNativeAccelerometerReading(accelerometerReading);
                
                dataSerializer.WriteObject(outputStream.AsStreamForWrite(), accReading);
            }
        }

        protected AccReading ReceiveAccelerometerReading(IInputStream inputStream)
        {
            var receivedObject = dataSerializer.ReadObject(inputStream.AsStreamForRead());

            return receivedObject as AccReading;
        }

        #endregion
    }
}
