using Windows.Networking;

namespace MixedReality.Common.Helpers
{
    public static class Settings
    {
        #region Cognitive services

        // Type your keys and endpoints here
        public static string VisionServiceClientKey { get; } = "dc15f728291f45b296dccce4f705a2a5";
        public static string VisionServiceEndPoint { get; } = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0";

        public static string BingSearchServiceClientKey { get; } = "71b0eb71a37b4900b13b35842cc1ebc9";
        public static string BingSearchEndPoint { get; set; } = "https://api.cognitive.microsoft.com/bing/v7.0/";

        #endregion

        #region Communication

        public static HostName HostName { get; } = new HostName("169.254.56.115");

        public static string Port { get; } = "9898";

        #endregion
    }
}
