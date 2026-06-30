using UnityEngine;

namespace LastHost.Prototype.Immune
{
    public sealed class ImmuneAlertModel
    {
        private readonly float maxValue;
        private readonly float baseGainPerSecond;
        private readonly float riskEventGain;
        private bool thresholdDispatched;

        public ImmuneAlertModel(float maxValue, float baseGainPerSecond, float riskEventGain)
        {
            this.maxValue = Mathf.Max(1f, maxValue);
            this.baseGainPerSecond = Mathf.Max(0f, baseGainPerSecond);
            this.riskEventGain = Mathf.Max(0f, riskEventGain);
            Value = 0f;
        }

        public float Value { get; private set; }
        public float MaxValue => maxValue;
        public float NormalizedValue => Mathf.Clamp01(Value / maxValue);

        public bool Tick(float deltaTime, float rateMultiplier)
        {
            var gain = baseGainPerSecond * Mathf.Max(0f, deltaTime) * Mathf.Max(0f, rateMultiplier);
            return AddRawAmount(gain);
        }

        public bool AddRiskEvent(float severityMultiplier)
        {
            return AddRawAmount(riskEventGain * Mathf.Max(0f, severityMultiplier));
        }

        public bool AddRawAmount(float amount)
        {
            if (amount <= 0f)
            {
                return false;
            }

            Value = Mathf.Clamp(Value + amount, 0f, maxValue);
            if (thresholdDispatched || Value < maxValue)
            {
                return false;
            }

            thresholdDispatched = true;
            return true;
        }

        public void ResetAfterInternalBattle(float returnValue)
        {
            Value = Mathf.Clamp(returnValue, 0f, maxValue);
            thresholdDispatched = Value >= maxValue;
        }
    }
}
