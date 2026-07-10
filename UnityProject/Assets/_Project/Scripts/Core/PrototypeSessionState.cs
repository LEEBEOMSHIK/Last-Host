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
        private bool hasRecordedCurrentInternalFailureExperience;

        public PrototypeSessionState()
            : this(new PrototypeConfig())
        {
        }

        public PrototypeSessionState(PrototypeConfig config)
        {
            Config = config;
            ImmuneAlert = new ImmuneAlertModel(config.MaxImmuneAlert, config.BaseAlertPerSecond, config.RiskAlertBonus);
            VirusRun = new VirusMinigameModel(config.VirusRequiredFragments, config.VirusStartingStability, config.VirusWhiteBloodCellDamage);
            SignalSuppressionRun = new ImmuneSignalSuppressionModel(
                config.SignalSuppressionTotalSignals,
                config.SignalSuppressionRequiredSuppressions,
                config.SignalSuppressionStartingStability,
                config.SignalSuppressionMistakeDamage,
                config.SignalSuppressionAccurateWindowSeconds,
                config.SignalSuppressionSignalIntervalSeconds);
            Mutations = new MutationLoadout();
            HostHealth = config.HostMaxHealth;
            Mode = PrototypeGameMode.RatHost;
            CurrentInternalMinigameType = config.DefaultInternalMinigameType;
        }

        public PrototypeConfig Config { get; }
        public ImmuneAlertModel ImmuneAlert { get; }
        public VirusMinigameModel VirusRun { get; }
        public ImmuneSignalSuppressionModel SignalSuppressionRun { get; }
        public MutationLoadout Mutations { get; }
        public PrototypeGameMode Mode { get; private set; }
        public InternalVirusMinigameType CurrentInternalMinigameType { get; private set; }
        public int ImmuneResponseExperience { get; private set; }
        public float HostHealth { get; private set; }
        public bool IsRatRiskInteractionAvailable { get; private set; }
        public string RatRiskInteractionPrompt { get; private set; } = string.Empty;
        public string LastImmuneAlertFeedbackLabel { get; private set; } = string.Empty;
        public float LastImmuneAlertFeedbackDelta { get; private set; }
        public bool HasImmuneAlertFeedback => !string.IsNullOrEmpty(LastImmuneAlertFeedbackLabel) && LastImmuneAlertFeedbackDelta > 0f;
        public string LastImmuneAlertFeedbackText => HasImmuneAlertFeedback
            ? $"{LastImmuneAlertFeedbackLabel} +{LastImmuneAlertFeedbackDelta:0.##}"
            : string.Empty;
        public string LastVirusPatternExposureLabel { get; private set; } = string.Empty;
        public float LastVirusPatternExposureDelta { get; private set; }
        public float VirusPatternExposureTotal { get; private set; }
        public bool HasVirusPatternExposureFeedback => !string.IsNullOrEmpty(LastVirusPatternExposureLabel) && LastVirusPatternExposureDelta > 0f;
        public bool HasVirusPatternExposure => VirusPatternExposureTotal > 0f;
        public string LastVirusPatternExposureFeedbackText => HasVirusPatternExposureFeedback
            ? $"{LastVirusPatternExposureLabel} +{LastVirusPatternExposureDelta:0.##}"
            : string.Empty;
        public string VirusPatternExposureSummaryText => HasVirusPatternExposureFeedback && HasVirusPatternExposure
            ? $"{LastVirusPatternExposureLabel} 흔적 +{VirusPatternExposureTotal:0.##}"
            : string.Empty;
        public bool IsRatHostRiskZoneGraceActive => Mode == PrototypeGameMode.RatHost && ratHostRiskZoneGraceRemainingSeconds > 0f;
        public float ActiveVirusStability => CurrentInternalMinigameType == InternalVirusMinigameType.ImmuneSignalSuppression
            ? SignalSuppressionRun.Stability
            : VirusRun.Stability;
        public float ActiveVirusStartingStability => CurrentInternalMinigameType == InternalVirusMinigameType.ImmuneSignalSuppression
            ? SignalSuppressionRun.StartingStability
            : VirusRun.StartingStability;
        public float ActiveVirusNormalizedStability => CurrentInternalMinigameType == InternalVirusMinigameType.ImmuneSignalSuppression
            ? SignalSuppressionRun.NormalizedStability
            : VirusRun.NormalizedStability;
        public float WhiteBloodCellSpeedMultiplier
        {
            get
            {
                var experienceBeyondFirstResponse = Mathf.Max(0, ImmuneResponseExperience - 1);
                var multiplier = 1f + experienceBeyondFirstResponse * Mathf.Max(0f, Config.WhiteBloodCellSpeedMultiplierPerExperience);
                return Mathf.Min(Mathf.Max(1f, Config.MaxWhiteBloodCellSpeedMultiplier), multiplier);
            }
        }
        public bool IsSignalSuppressionCueActive => SignalSuppressionCueIntensity > 0f;
        public float SignalSuppressionCueIntensity
        {
            get
            {
                if (CurrentInternalMinigameType != InternalVirusMinigameType.ImmuneSignalSuppression)
                {
                    return 0f;
                }

                var timeUntilSignal = SignalSuppressionRun.TimeUntilSignal;
                var accurateWindow = Mathf.Max(0.01f, Config.SignalSuppressionAccurateWindowSeconds);
                if (Mathf.Abs(timeUntilSignal) <= accurateWindow)
                {
                    return 1f;
                }

                if (timeUntilSignal < 0f)
                {
                    return 0f;
                }

                var cueLead = Mathf.Max(accurateWindow, Config.SignalSuppressionCueLeadSeconds);
                if (timeUntilSignal > cueLead || cueLead <= accurateWindow)
                {
                    return 0f;
                }

                return 1f - Mathf.Clamp01((timeUntilSignal - accurateWindow) / (cueLead - accurateWindow));
            }
        }
        public string InternalMinigameProgressText => CurrentInternalMinigameType == InternalVirusMinigameType.ImmuneSignalSuppression
            ? $"신호 차단 {SignalSuppressionRun.SuppressedSignals}/{SignalSuppressionRun.RequiredSuppressions} / {SignalSuppressionTimingText}"
            : $"변이 조각 {VirusRun.CollectedFragments}/{VirusRun.RequiredFragments}";
        public string SignalSuppressionTimingText
        {
            get
            {
                if (CurrentInternalMinigameType != InternalVirusMinigameType.ImmuneSignalSuppression)
                {
                    return string.Empty;
                }

                var timeUntilSignal = SignalSuppressionRun.TimeUntilSignal;
                if (Mathf.Abs(timeUntilSignal) <= Config.SignalSuppressionAccurateWindowSeconds)
                {
                    return "지금 차단";
                }

                if (timeUntilSignal > 0f && IsSignalSuppressionCueActive)
                {
                    return $"신호 접근 {timeUntilSignal:0.0}초";
                }

                return timeUntilSignal > 0f
                    ? $"다음 신호 {timeUntilSignal:0.0}초"
                    : "늦음 위험";
            }
        }
        public string InternalMinigameObjectiveText
        {
            get
            {
                if (CurrentInternalMinigameType != InternalVirusMinigameType.ImmuneSignalSuppression)
                {
                    return "변이 조각 수집 / 백혈구 회피";
                }

                switch (SignalSuppressionRun.LastJudgement)
                {
                    case ImmuneSignalSuppressionJudgement.Accurate:
                        return $"신호 차단 {SignalSuppressionRun.SuppressedSignals}/{SignalSuppressionRun.RequiredSuppressions}";
                    case ImmuneSignalSuppressionJudgement.Early:
                    case ImmuneSignalSuppressionJudgement.Late:
                    case ImmuneSignalSuppressionJudgement.Missed:
                        return $"경보 증폭 {SignalSuppressionRun.MissedSignals}";
                    default:
                        return "면역 신호 억제";
                }
            }
        }

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

            EnterVirusMinigame(SelectInternalMinigameTypeForAlertCause(feedbackLabel));
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

            EnterVirusMinigame(SelectInternalMinigameTypeForAlertCause(feedbackLabel));
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
            EnterVirusMinigame(Config.DefaultInternalMinigameType);
        }

        public void EnterVirusMinigame(InternalVirusMinigameType minigameType)
        {
            var startsNewInternalResponse = Mode != PrototypeGameMode.InternalVirus;
            ClearRatHostRiskZoneGrace();
            SetRatRiskInteractionAffordance(false, string.Empty);
            ClearVirusPatternExposure();
            CurrentInternalMinigameType = minigameType;
            VirusRun.ResetRun();
            SignalSuppressionRun.ResetRun();
            if (startsNewInternalResponse)
            {
                RecordInternalResponseEntry();
            }

            Mode = PrototypeGameMode.InternalVirus;
        }

        public VirusMinigameOutcome ResolveVirusFrame(bool collectedFragment, bool hitByWhiteBloodCell)
        {
            if (Mode != PrototypeGameMode.InternalVirus || CurrentInternalMinigameType != InternalVirusMinigameType.WhiteBloodCellEvasion)
            {
                return VirusRun.Outcome;
            }

            if (hitByWhiteBloodCell)
            {
                RecordVirusPatternExposure(Config.VirusPatternExposureFeedbackLabel, Config.VirusPatternExposurePerWhiteBloodCellHit);
            }

            var outcome = VirusRun.ResolveFrame(collectedFragment, hitByWhiteBloodCell);
            if (outcome == VirusMinigameOutcome.Success)
            {
                Mode = PrototypeGameMode.MutationSelection;
            }
            else if (outcome == VirusMinigameOutcome.Failed)
            {
                RecordInternalResponseFailure();
                Mode = PrototypeGameMode.VirusFailed;
            }

            return outcome;
        }

        public VirusMinigameOutcome TickSignalSuppression(float deltaTime)
        {
            if (Mode != PrototypeGameMode.InternalVirus || CurrentInternalMinigameType != InternalVirusMinigameType.ImmuneSignalSuppression)
            {
                return SignalSuppressionRun.Outcome;
            }

            var outcome = SignalSuppressionRun.Tick(deltaTime);
            ApplyInternalMinigameOutcome(outcome);
            return outcome;
        }

        public ImmuneSignalSuppressionJudgement ResolveSignalSuppressionInput()
        {
            if (Mode != PrototypeGameMode.InternalVirus || CurrentInternalMinigameType != InternalVirusMinigameType.ImmuneSignalSuppression)
            {
                return ImmuneSignalSuppressionJudgement.Ignored;
            }

            var judgement = SignalSuppressionRun.ResolveInput();
            ApplyInternalMinigameOutcome(SignalSuppressionRun.Outcome);
            return judgement;
        }

        public ImmuneSignalSuppressionJudgement ResolveSignalSuppressionMissedSignal()
        {
            if (Mode != PrototypeGameMode.InternalVirus || CurrentInternalMinigameType != InternalVirusMinigameType.ImmuneSignalSuppression)
            {
                return ImmuneSignalSuppressionJudgement.Ignored;
            }

            var judgement = SignalSuppressionRun.ResolveMissedSignal();
            ApplyInternalMinigameOutcome(SignalSuppressionRun.Outcome);
            return judgement;
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
            ClearVirusPatternExposure();
            VirusRun.ResetRun();
            SignalSuppressionRun.ResetRun();
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
            var returnAlert = Config.AlertAfterMutationReturn + VirusPatternExposureTotal;
            ImmuneAlert.ResetAfterInternalBattle(returnAlert);
            ClearImmuneAlertFeedback();
            ClearVirusPatternExposure();
            VirusRun.ResetRun();
            SignalSuppressionRun.ResetRun();
            Mode = PrototypeGameMode.RatHost;
            StartRatHostRiskZoneGrace();
            return true;
        }

        private void ApplyInternalMinigameOutcome(VirusMinigameOutcome outcome)
        {
            if (outcome == VirusMinigameOutcome.Success)
            {
                Mode = PrototypeGameMode.MutationSelection;
            }
            else if (outcome == VirusMinigameOutcome.Failed)
            {
                RecordInternalResponseFailure();
                Mode = PrototypeGameMode.VirusFailed;
            }
        }

        private InternalVirusMinigameType SelectInternalMinigameTypeForAlertCause(string feedbackLabel)
        {
            if (string.IsNullOrWhiteSpace(feedbackLabel))
            {
                return Config.DefaultInternalMinigameType;
            }

            var normalizedLabel = feedbackLabel.Trim();
            if (IsWhiteBloodCellCauseLabel(normalizedLabel))
            {
                return InternalVirusMinigameType.WhiteBloodCellEvasion;
            }

            if (IsSignalSuppressionCauseLabel(normalizedLabel))
            {
                return InternalVirusMinigameType.ImmuneSignalSuppression;
            }

            return Config.DefaultInternalMinigameType;
        }

        private static bool IsSignalSuppressionCauseLabel(string label)
        {
            return label.Contains("강제 조종")
                || label.Contains("소음")
                || label.Contains("조직 자극")
                || label.Contains("면역 신호")
                || label.Contains("경보");
        }

        private static bool IsWhiteBloodCellCauseLabel(string label)
        {
            return label.Contains("오염")
                || label.Contains("면역 포착")
                || label.Contains("바이러스 흔적");
        }

        private void RecordInternalResponseEntry()
        {
            ImmuneResponseExperience++;
            hasRecordedCurrentInternalFailureExperience = false;
        }

        private void RecordInternalResponseFailure()
        {
            if (hasRecordedCurrentInternalFailureExperience)
            {
                return;
            }

            ImmuneResponseExperience++;
            hasRecordedCurrentInternalFailureExperience = true;
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

        private void RecordVirusPatternExposure(string label, float delta)
        {
            if (delta <= 0f || string.IsNullOrWhiteSpace(label))
            {
                return;
            }

            var trimmedLabel = label.Trim();
            LastVirusPatternExposureDelta = HasVirusPatternExposureFeedback && LastVirusPatternExposureLabel == trimmedLabel
                ? LastVirusPatternExposureDelta + delta
                : delta;
            LastVirusPatternExposureLabel = trimmedLabel;
            VirusPatternExposureTotal += delta;
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

        private void ClearVirusPatternExposure()
        {
            LastVirusPatternExposureLabel = string.Empty;
            LastVirusPatternExposureDelta = 0f;
            VirusPatternExposureTotal = 0f;
        }
    }
}
