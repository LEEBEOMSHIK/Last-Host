using LastHost.Prototype.Core;
using UnityEngine;

namespace LastHost.Prototype.RatHost2D
{
    public readonly struct RatHost2DHudSnapshot
    {
        public RatHost2DHudSnapshot(
            float hostHealth,
            float hostMaxHealth,
            float immuneAlert,
            float immuneAlertMax,
            PrototypeGameMode mode,
            string immuneAlertFeedback,
            bool isHostHudVisible)
        {
            HostHealth = hostHealth;
            HostMaxHealth = hostMaxHealth;
            ImmuneAlert = immuneAlert;
            ImmuneAlertMax = immuneAlertMax;
            Mode = mode;
            ImmuneAlertFeedback = immuneAlertFeedback ?? string.Empty;
            IsHostHudVisible = isHostHudVisible;
        }

        public float HostHealth { get; }
        public float HostMaxHealth { get; }
        public float HostHealthNormalized => HostMaxHealth <= 0f
            ? 0f
            : Mathf.Clamp01(HostHealth / HostMaxHealth);
        public float ImmuneAlert { get; }
        public float ImmuneAlertMax { get; }
        public float ImmuneAlertNormalized => ImmuneAlertMax <= 0f
            ? 0f
            : Mathf.Clamp01(ImmuneAlert / ImmuneAlertMax);
        public PrototypeGameMode Mode { get; }
        public string ImmuneAlertFeedback { get; }
        public bool IsHostHudVisible { get; }
        public string ModeLabel => Mode == PrototypeGameMode.RatHost
            ? "쥐 숙주"
            : "내부 면역 반응";
    }
}
