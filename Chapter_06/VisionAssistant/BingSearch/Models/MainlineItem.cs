namespace VisionAssistant.BingSearch.Models
{
    public class MainlineItem
    {
        #region Properties

        public long? ResultIndex { get; set; }

        public string AnswerType { get; set; }
        
        public MainlineItemValue Value { get; set; }

        #endregion
    }
}
