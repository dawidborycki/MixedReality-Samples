#region Using

using Newtonsoft.Json;

#endregion

namespace VisionAssistant.BingSearch.Models
{
    public class SearchResult
    {
        #region Properties

        public Images Images { get; set; }
     
        public RankingResponse RankingResponse { get; set; }

        [JsonProperty("_type")]
        public string Type { get; set; }

        public QueryContext QueryContext { get; set; }

        public RelatedSearches RelatedSearches { get; set; }

        public WebPages WebPages { get; set; }

        #endregion
    }    
}
