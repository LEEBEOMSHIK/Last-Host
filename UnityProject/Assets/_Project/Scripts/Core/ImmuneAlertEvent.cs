namespace LastHost.Prototype.Core
{
    public readonly struct ImmuneAlertEvent
    {
        public ImmuneAlertEvent(ImmuneAlertCauseType causeType, string feedbackLabel)
        {
            CauseType = causeType;
            FeedbackLabel = feedbackLabel ?? string.Empty;
        }

        public ImmuneAlertCauseType CauseType { get; }
        public string FeedbackLabel { get; }
    }
}
