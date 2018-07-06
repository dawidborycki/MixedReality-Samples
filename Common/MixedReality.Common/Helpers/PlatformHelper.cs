#region Using

using Windows.System.Profile;

#endregion

namespace MixedReality.Common.Helpers
{
    public static class PlatformHelper
    {
        public static bool IsHolographic()
        {
            return AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Holographic";
        }        
    }
}
