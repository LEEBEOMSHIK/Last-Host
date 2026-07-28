using LastHost.Prototype.Core;
using UnityEngine;

namespace LastHost.Prototype.RatHost2D
{
    public readonly struct RatHost2DVirusHudSnapshot
    {
        public RatHost2DVirusHudSnapshot(
            float stability,
            float startingStability,
            int collectedFragments,
            int requiredFragments,
            PrototypeGameMode mode,
            string exposureFeedback,
            bool isVirusGameplayActive,
            bool isFailureAwaitingConfirmation,
            bool isMutationSelectionHandoff)
        {
            Stability = stability;
            StartingStability = startingStability;
            CollectedFragments = collectedFragments;
            RequiredFragments = requiredFragments;
            Mode = mode;
            ExposureFeedback = exposureFeedback ?? string.Empty;
            IsVirusGameplayActive = isVirusGameplayActive;
            IsFailureAwaitingConfirmation = isFailureAwaitingConfirmation;
            IsMutationSelectionHandoff = isMutationSelectionHandoff;
        }

        public float Stability { get; }
        public float StartingStability { get; }
        public float NormalizedStability => StartingStability <= 0f
            ? 0f
            : Mathf.Clamp01(Stability / StartingStability);
        public int CollectedFragments { get; }
        public int RequiredFragments { get; }
        public PrototypeGameMode Mode { get; }
        public string ExposureFeedback { get; }
        public bool IsVirusGameplayActive { get; }
        public bool IsFailureAwaitingConfirmation { get; }
        public bool IsMutationSelectionHandoff { get; }
        public string ObjectiveText => IsMutationSelectionHandoff
            ? "변이 선택 단계로 인계"
            : IsFailureAwaitingConfirmation
                ? "면역 반응 돌파 실패 · 확인 후 숙주 복귀"
                : "변이 조각 수집 / 백혈구 회피";
    }
}
