namespace LastHost.Prototype.Core
{
    public sealed class PrototypeConfig
    {
        public float MaxImmuneAlert { get; set; } = 100f;
        public float BaseAlertPerSecond { get; set; } = 0f;
        public float RiskAlertBonus { get; set; } = 20f;
        public float AlertAfterMutationReturn { get; set; } = 25f;
        public float AlertAfterVirusFailureReturn { get; set; } = 60f;
        public float RiskZoneGraceAfterMutationReturnSeconds { get; set; } = 1.5f;
        public float ImmuneAlertFeedbackSeconds { get; set; } = 2f;
        public float HostMaxHealth { get; set; } = 100f;
        public float RatHostInstinctResistance { get; set; } = 1f;
        public InternalVirusMinigameType DefaultInternalMinigameType { get; set; } = InternalVirusMinigameType.WhiteBloodCellEvasion;
        public int VirusRequiredFragments { get; set; } = 3;
        public float VirusStartingStability { get; set; } = 100f;
        public float VirusWhiteBloodCellDamage { get; set; } = 34f;
        public float VirusPatternExposurePerWhiteBloodCellHit { get; set; } = 8f;
        public string VirusPatternExposureFeedbackLabel { get; set; } = "면역 포착";
        public int SignalSuppressionTotalSignals { get; set; } = 12;
        public int SignalSuppressionRequiredSuppressions { get; set; } = 8;
        public float SignalSuppressionStartingStability { get; set; } = 100f;
        public float SignalSuppressionMistakeDamage { get; set; } = 20f;
        public float SignalSuppressionAccurateWindowSeconds { get; set; } = 0.12f;
        public float SignalSuppressionSignalIntervalSeconds { get; set; } = 1f;
    }
}
