namespace VisionAssistant.BingSearch.Models
{
    public class ImageValue
    {
        #region Properties

        public string EncodingFormat { get; set; }
     
        public InsightsMetadata InsightsMetadata { get; set; }

        public string ContentUrl { get; set; }

        public string ContentSize { get; set; }

        public string DatePublished { get; set; }

        public string HostPageDisplayUrl { get; set; }

        public long Height { get; set; }
        
        public string HostPageUrl { get; set; }
        
        public Thumbnail Thumbnail { get; set; }

        public string WebSearchUrl { get; set; }

        public string Name { get; set; }

        public string ThumbnailUrl { get; set; }

        public long Width { get; set; }

        #endregion
    }
}
