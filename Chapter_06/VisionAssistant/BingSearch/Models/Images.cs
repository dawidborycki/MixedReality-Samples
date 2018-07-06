#region Using

using Newtonsoft.Json;

#endregion

namespace VisionAssistant.BingSearch.Models
{
    public class Images
    {
        #region Properties

        public bool IsFamilyFriendly { get; set; }

        [JsonProperty("value")]
        public ImageValue[] Value { get; set; }
        
        public string Id { get; set; }
        
        public string ReadLink { get; set; }
        
        public string WebSearchUrl { get; set; }

        #endregion
    }
}
