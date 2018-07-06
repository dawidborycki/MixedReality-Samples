#region Using

using Newtonsoft.Json;

#endregion

namespace VisionAssistant.BingSearch.Models
{
    public class WebPages
    {
        #region Properties

        [JsonProperty("value")]
        public WebPage[] Items { get; set; }
        
        public long TotalEstimatedMatches { get; set; }

        public string WebSearchUrl { get; set; }

        #endregion
    }
}
