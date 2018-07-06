namespace VisionAssistant.BingSearch.Models
{
    public class WebPage
    {
        #region Properties

        public string DisplayUrl { get; set; }

        public string DateLastCrawled { get; set; }

        public About[] About { get; set; }

        public DeepLinks[] DeepLinks { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }

        public string Snippet { get; set; }

        public string Url { get; set; }

        #endregion
    }
}
