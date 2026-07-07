using LastHost.Prototype.Immune;
using LastHost.Prototype.Mutations;
using LastHost.Prototype.VirusMinigame;
using UnityEngine;

namespace LastHost.Prototype.Core
{
    public sealed class PrototypeSessionState
    {
        private float immuneAlertFeedbackRemainingSeconds;
        private float ratHostRiskZoneGraceRemainingSeconds;

        public PrototypeSessionState()
            : this(new PrototypeConfig())
        {
        }

        public PrototypeSessionState(PrototypeConfig config)
        {
            Config = config;
            ImmuneAlert = new ImmuneAlertModel(config.MaxImmuneAlert, config.BaseAlertPerSecond, config.RiskAlertBonus);
            VirusRun = new VirusMinigameModel(config.VirusRequiredFragments, config.VirusStartingStability, config.VirusWhiteBloodCellDamage);
            Mutations = new MutationLoadout();
            HostHealth = config.HostMaxHealth;
            Mode = PrototypeGameMode.RatHost;
        }

        public PrototypeConfig Config { get; }
        public ImmuneAlertModel ImmuneAlert { get; }
        public VirusMinigameModel VirusRun { get; }
        public MutationLoadout Mutations { get; }
        public PrototypeGameMode Mode { get; private set; }
        public float HostHealth { get; private set; }
        public bool IsRatRiskInteractionAvailable { get; private set; }
        public string RatRiskInteractionPrompt { get; private set; } = string.Empty;
        public string LastImmuneAlertFeedbackLabel { get; private set; } = string.Empty;
        public float LastImmuneAlertFeedbackDelta { get; private set; }
        public bool HasImmuneAlertFeedback => !string.IsNullOrEmpty(LastImmuneAlertFeedbackLabel) && LastImmuneAlertFeedbackDelta > 0f;
        public string LastImmuneAlertFeedbackText => HasImmuneAlertFeedback
            ? $"{LastImmuneAlertFeedbackLabel} +{LastImmuneAlertFeedbackDelta:0.##}"
            : string.Empty;
        public bool IsRatHostRiskZoneGraceActive => Mode == PrototypeGameMode.RatHost && ratHostRiskZoneGraceRemainingSeconds > 0f;

        public bool TickRatMode(float deltaTime)
        {
            if (Mode != PrototypeGameMode.RatHost)
            {
                return false;
            }

            TickRatHostRiskZoneGrace(deltaTime);
            TickImmuneAlertFeedback(deltaTime);

            if (!ImmuneAlert.Tick(deltaTime, Mutations.ImmuneAlertRateMultiplier))
            {
                return false;
            }

            EnterVirusMinigame();
            return true;
        }

        public bool AddRiskAlert(float severityMultiplier)
        {
            return AddRiskAlert(severityMultiplier, string.Empty);
        }

        public bool AddRiskAlert(float severityMultiplier, string feedbackLabel)
        {
            if (Mode != PrototypeGameMode.RatHost)
            {
                return false;
            }

            var previousAlert = ImmuneAlert.Value;
            var reachedThreshold = ImmuneAlert.AddRiskEvent(severityMultiplier);
            RecordImmuneAlertFeedback(feedbackLabel, ImmuneAlert.Value - previousAlert);

            if (!reachedThreshold)
            {
                return false;
            }

            EnterVirusMinigame();
            return true;
        }

        public bool AddImmuneAlertAmount(float amount)
        {
            return AddImmuneAlertAmount(amount, string.Empty);
        }

        public bool AddImmuneAlertAmount(float amount, string feedbackLabel)
        {
            if (Mode != PrototypeGameMode.RatHost)
            {
                return false;
            }

            var previousAlert = ImmuneAlert.Value;
            var reachedThreshold = ImmuneAlert.AddRawAmount(amount);
            RecordImmuneAlertFeedback(feedbackLabel, ImmuneAlert.Value - previousAlert);

            if (!reachedThreshold)
            {
                return false;
            }

            EnterVirusMinigame();
            return true;
        }

        public void DamageHost(float amount)
        {
            HostHealth = Mathf.Clamp(HostHealth - Mathf.Max(0f, amount), 0f, Config.HostMaxHealth);
        }

        public bool SetRatRiskInteractionAffordance(bool isAvailable, string prompt)
        {
            var nextAvailable = isAvailable && !string.IsNullOrWhiteSpace(prompt);
            var nextPrompt = nextAvailable ? prompt : string.Empty;
            if (IsRatRiskInteractionAvailable == nextAvailable && RatRiskInteractionPrompt == nextPrompt)
            {
                return false;
            }

            IsRatRiskInteractionAvailable = nextAvailable;
            RatRiskInteractionPrompt = nextPrompt;
            return true;
        }

        public void EnterVirusMinigame()
        {
            ClearRatHostRiskZoneGrace();
            SetRatRiskInteractionAffordance(false, string.Empty);
            VirusRun.ResetRun();
            Mode = PrototypeGameMode.InternalVirus;
        }

        public VirusMinigameOutcome ResolveVirusFrame(bool collectedFragment, bool hitByWhiteBloodCell)
        {
            if (Mode != PrototypeGameMode.InternalVirus)
            {
                return VirusRun.Outcome;
            }

            var outcome = VirusRun.ResolveFrame(collectedFragment, hitByWhiteBloodCell);
            if (outcome == VirusMinigameOutcome.Success)
            {
                Mode = PrototypeGameMode.MutationSelection;
            }
            else if (outcome == VirusMinigameOutcome.Failed)
            {
                Mode = PrototypeGameMode.VirusFailed;
            }

            return outcome;
        }

        public bool RetryVirusMinigame()
        {
            return ReturnToRatHostAfterVirusFailure();
        }

        public bool ReturnToRatHostAfterVirusFailure()
        {
            if (Mode != PrototypeGameMode.VirusFailed)
            {
                return false;
            }

            ImmuneAlert.ResetAfterInternalBattle(Config.AlertAfterVirusFailureReturn);
            ClearImmuneAlertFeedback();
            VirusRun.ResetRun();
            Mode = PrototypeGameMode.RatHost;
            StartRatHostRiskZoneGrace();
            return true;
        }

        public bool SelectMutation(MutationType type)
        {
            if (Mode != PrototypeGameMode.MutationSelection)
            {
                return false;
            }

            Mutations.Apply(type);
            ImmuneAlert.ResetAfterInternalBattle(Config.AlertAfterMutationReturn);
            ClearImmuneAlertFeedback();
            VirusRun.ResetRun();
            Mode = PrototypeGameMode.RatHost;
            StartRatHostRiskZoneGrace();
            return true;
        }

        private void RecordImmuneAlertFeedback(string label, float delta)
        {
            if (delta <= 0f)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(label))
            {
                ClearImmuneAlertFeedback();
                return;
            }

            var duration = Mathf.Max(0f, Config.ImmuneAlertFeedbackSeconds);
            if (duration <= 0f)
            {
                ClearImmuneAlertFeedback();
                return;
            }

            var trimmedLabel = label.Trim();
            LastImmuneAlertFeedbackDelta = HasImmuneAlertFeedback && LastImmuneAlertFeedbackLabel == trimmedLabel
                ? LastImmuneAlertFeedbackDelta + delta
                : delta;
            LastImmuneAlertFeedbackLabel = trimmedLabel;
            immuneAlertFeedbackRemainingSeconds = duration;
        }

        private void TickImmuneAlertFeedback(float deltaTime)
        {
            if (!HasImmuneAlertFeedback)
            {
                return;
            }

            immuneAlertFeedbackRemainingSeconds -= Mathf.Max(0f, deltaTime);
            if (immuneAlertFeedbackRemainingSeconds <= 0f)
            {
                ClearImmuneAlertFeedback();
            }
        }

        private void StartRatHostRiskZoneGrace()
        {
            ratHostRiskZoneGraceRemainingSeconds = Mathf.Max(0f, Config.RiskZoneGraceAfterMutationReturnSeconds);
        }

        private void TickRatHostRiskZoneGrace(float deltaTime)
        {
            if (ratHostRiskZoneGraceRemainingSeconds <= 0f)
            {
                return;
            }

            ratHostRiskZoneGraceRemainingSeconds -= Mathf.Max(0f, deltaTime);
            if (ratHostRiskZoneGraceRemainingSeconds < 0f)
            {
                ratHostRiskZoneGraceRemainingSeconds = 0f;
            }
        }

        private void ClearRatHostRiskZoneGrace()
        {
            ratHostRiskZoneGraceRemainingSeconds = 0f;
        }

        private void ClearImmuneAlertFeedback()
        {
            LastImmuneAlertFeedbackLabel = string.Empty;
            LastImmuneAlertFeedbackDelta = 0f;
            immuneAlertFeedbackRemainingSeconds = 0f;
        }
    }
}
