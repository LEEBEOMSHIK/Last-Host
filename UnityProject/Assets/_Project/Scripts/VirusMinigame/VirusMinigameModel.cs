using UnityEngine;

namespace LastHost.Prototype.VirusMinigame
{
    public sealed class VirusMinigameModel
    {
        private readonly int requiredFragments;
        private readonly float startingStability;
        private readonly float whiteBloodCellDamage;

        public VirusMinigameModel(int requiredFragments, float startingStability, float whiteBloodCellDamage)
        {
            this.requiredFragments = Mathf.Max(1, requiredFragments);
            this.startingStability = Mathf.Max(1f, startingStability);
            this.whiteBloodCellDamage = Mathf.Max(0f, whiteBloodCellDamage);
            ResetRun();
        }

        public int RequiredFragments => requiredFragments;
        public int CollectedFragments { get; private set; }
        public float Stability { get; private set; }
        public float StartingStability => startingStability;
        public float NormalizedStability => Mathf.Clamp01(Stability / startingStability);
        public VirusMinigameOutcome Outcome { get; private set; }

        public VirusMinigameOutcome ResolveFrame(bool collectedFragment, bool hitByWhiteBloodCell)
        {
            if (Outcome != VirusMinigameOutcome.Running)
            {
                return Outcome;
            }

            if (collectedFragment && CollectedFragments < requiredFragments)
            {
                CollectedFragments++;
            }

            if (hitByWhiteBloodCell)
            {
                Stability = Mathf.Max(0f, Stability - whiteBloodCellDamage);
            }

            if (CollectedFragments >= requiredFragments)
            {
                Outcome = VirusMinigameOutcome.Success;
            }
            else if (Stability <= 0f)
            {
                Outcome = VirusMinigameOutcome.Failed;
            }

            return Outcome;
        }

        public void ResetRun()
        {
            CollectedFragments = 0;
            Stability = startingStability;
            Outcome = VirusMinigameOutcome.Running;
        }
    }
}
