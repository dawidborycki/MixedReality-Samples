#region Using

using MixedReality.Common.ArgumentValidation;
using System;
using Windows.Devices.Sensors;

#endregion

namespace MixedReality.Common.Sensors
{    
public class AccReading
{
    #region Properties

    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    #endregion

    #region Methods (Public)

    public override string ToString()
    {            
        return $"X: {X:F4}, Y: {Y:F4}, Z: {Z:F4}, Timestamp: {Timestamp}";
    }

    public static AccReading FromNativeAccelerometerReading(
        AccelerometerReading accelerometerReading)
    {
        Check.IsNull(accelerometerReading, "accelerometerReading");

        return new AccReading()
        {
            X = accelerometerReading.AccelerationX,
            Y = accelerometerReading.AccelerationY,
            Z = accelerometerReading.AccelerationZ,
            Timestamp = accelerometerReading.Timestamp
        };
    }

    #endregion
}    
}
