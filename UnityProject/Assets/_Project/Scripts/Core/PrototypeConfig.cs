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
        public int VirusRequiredFragments { get; set; } = 3;
        public float VirusStartingStability { get; set; } = 100f;
        public float VirusWhiteBloodCellDamage { get; set; } = 34f;
    }
}
