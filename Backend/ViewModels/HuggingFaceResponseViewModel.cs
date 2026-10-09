namespace Backend.ViewModels
{
    public class HuggingFaceResponseViewModel
    {
        public string Action { get; set; }
        public string Guideline { get; set; }
        public string Result { get; set; }
        public double Confidence { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
