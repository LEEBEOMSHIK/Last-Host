using UnityEngine;

namespace LastHost.Prototype.VirusMinigame
{
    public sealed class ImmuneSignalSuppressionModel
    {
        private static readonly float[] StageTwoIntervalMultipliers = { 1f, 0.7f, 1.2f, 0.65f, 1f, 0.8f };
        private static readonly bool[] StageTwoFastSignals = { false, true, false, true, false, true };

        private readonly int totalSignals;
        private readonly int requiredSuppressions;
        private readonly float startingStability;
        private readonly float mistakeDamage;
        private readonly float accurateWindowSeconds;
        private readonly float signalIntervalSeconds;
        private readonly float missWindowSeconds;

        public ImmuneSignalSuppressionModel(
            int totalSignals,
            int requiredSuppressions,
            float startingStability,
            float mistakeDamage,
            float accurateWindowSeconds,
            float signalIntervalSeconds)
        {
            this.totalSignals = Mathf.Max(1, totalSignals);
            this.requiredSuppressions = Mathf.Clamp(requiredSuppressions, 1, this.totalSignals);
            this.startingStability = Mathf.Max(1f, startingStability);
            this.mistakeDamage = Mathf.Max(0f, mistakeDamage);
            this.accurateWindowSeconds = Mathf.Max(0.01f, accurateWindowSeconds);
            this.signalIntervalSeconds = Mathf.Max(this.accurateWindowSeconds, signalIntervalSeconds);
            missWindowSeconds = this.accurateWindowSeconds * 3f;
            ResetRun();
        }

        public int TotalSignals => totalSignals;
        public int RequiredSuppressions => requiredSuppressions;
        public int ResolvedSignals { get; private set; }
        public int SuppressedSignals { get; private set; }
        public int MissedSignals { get; private set; }
        public float Stability { get; private set; }
        public float StartingStability => startingStability;
        public float NormalizedStability => Mathf.Clamp01(Stability / startingStability);
        public float TimeUntilSignal { get; private set; }
        public int RhythmStage { get; private set; } = 1;
        public float CurrentSignalIntervalSeconds => GetSignalIntervalSeconds(ResolvedSignals);
        public bool IsNextSignalFast => RhythmStage >= 2 && StageTwoFastSignals[ResolvedSignals % StageTwoFastSignals.Length];
        public ImmuneSignalSuppressionJudgement LastJudgement { get; private set; }
        public VirusMinigameOutcome Outcome { get; private set; }

        public VirusMinigameOutcome Tick(float deltaTime)
        {
            if (Outcome != VirusMinigameOutcome.Running)
            {
                return Outcome;
            }

            TimeUntilSignal -= Mathf.Max(0f, deltaTime);
            if (TimeUntilSignal < -missWindowSeconds)
            {
                ResolveMissedSignal();
            }

            return Outcome;
        }

        public ImmuneSignalSuppressionJudgement ResolveInput()
        {
            if (Outcome != VirusMinigameOutcome.Running)
            {
                return ImmuneSignalSuppressionJudgement.Ignored;
            }

            var offset = -TimeUntilSignal;
            if (Mathf.Abs(offset) <= accurateWindowSeconds)
            {
                return ResolveSignal(ImmuneSignalSuppressionJudgement.Accurate);
            }

            return ResolveSignal(TimeUntilSignal > 0f
                ? ImmuneSignalSuppressionJudgement.Early
                : ImmuneSignalSuppressionJudgement.Late);
        }

        public ImmuneSignalSuppressionJudgement ResolveMissedSignal()
        {
            if (Outcome != VirusMinigameOutcome.Running)
            {
                return ImmuneSignalSuppressionJudgement.Ignored;
            }

            return ResolveSignal(ImmuneSignalSuppressionJudgement.Missed);
        }

        public void ResetRun()
        {
            ResetRun(RhythmStage);
        }

        public void ResetRun(int rhythmStage)
        {
            RhythmStage = Mathf.Max(1, rhythmStage);
            ResolvedSignals = 0;
            SuppressedSignals = 0;
            MissedSignals = 0;
            Stability = startingStability;
            TimeUntilSignal = CurrentSignalIntervalSeconds;
            LastJudgement = ImmuneSignalSuppressionJudgement.None;
            Outcome = VirusMinigameOutcome.Running;
        }

        private ImmuneSignalSuppressionJudgement ResolveSignal(ImmuneSignalSuppressionJudgement judgement)
        {
            ResolvedSignals++;
            LastJudgement = judgement;

            if (judgement == ImmuneSignalSuppressionJudgement.Accurate)
            {
                SuppressedSignals++;
            }
            else
            {
                MissedSignals++;
                Stability = Mathf.Max(0f, Stability - mistakeDamage);
            }

            UpdateOutcome();
            if (Outcome == VirusMinigameOutcome.Running)
            {
                TimeUntilSignal = CurrentSignalIntervalSeconds;
            }

            return judgement;
        }

        private float GetSignalIntervalSeconds(int signalIndex)
        {
            if (RhythmStage < 2)
            {
                return signalIntervalSeconds;
            }

            var multiplier = StageTwoIntervalMultipliers[signalIndex % StageTwoIntervalMultipliers.Length];
            return Mathf.Max(accurateWindowSeconds, signalIntervalSeconds * multiplier);
        }

        private void UpdateOutcome()
        {
            if (SuppressedSignals >= requiredSuppressions)
            {
                Outcome = VirusMinigameOutcome.Success;
                return;
            }

            var remainingSignals = totalSignals - ResolvedSignals;
            if (Stability <= 0f || ResolvedSignals >= totalSignals || SuppressedSignals + remainingSignals < requiredSuppressions)
            {
                Outcome = VirusMinigameOutcome.Failed;
            }
        }
    }
}
