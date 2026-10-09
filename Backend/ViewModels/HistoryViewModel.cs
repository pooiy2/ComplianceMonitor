using Backend.Enums;

namespace Backend.ViewModels
{
    public class HistoryViewModel
    {
        public long Id { get; set; }

        public string? Action { get; set; }

        public string? Guideline { get; set; }

        public double? Confidence { get; set; }

        public DateTime Timestamp { get; set; }

        public string? Result { get; set; }

    }
}
