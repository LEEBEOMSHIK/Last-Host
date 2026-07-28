using System;
using System.Collections.Generic;
using LastHost.Prototype.Core;
using LastHost.Prototype.Input;
using LastHost.Prototype.VirusMinigame;
using UnityEngine;

namespace LastHost.Prototype.RatHost2D
{
    public sealed class RatHost2DSessionController : MonoBehaviour
    {
        public const float ContaminationAlertPerSecond = 12f;
        public const float ContaminationHealthDamagePerSecond = 4f;
        public const string ContaminationFeedbackLabel = "오염 노출";
        public const string InternalShellTitle = "내부 면역 반응";
        public const string InternalShellObjective =
            "변이 조각 3개 수집 / 백혈구 회피";

        [Header("Mode Roots")]
        [SerializeField] private GameObject hostModeRoot;
        [SerializeField] private GameObject internalShellRoot;
        [SerializeField] private GameObject hostHudRoot;
        [SerializeField] private GameObject hostCameraRoot;
        [SerializeField] private GameObject internalCameraRoot;
        [SerializeField] private GameObject failurePanelRoot;
        [SerializeField] private GameObject mutationSelectionShellRoot;

        [Header("Host Runtime")]
        [SerializeField] private RatHost2DMovementController hostMovement;
        [SerializeField] private Collider2D[] hostColliders = Array.Empty<Collider2D>();

        [Header("Internal Virus Runtime")]
        [SerializeField] private RatHost2DVirusMovementController virusMovement;
        [SerializeField] private RatHost2DWhiteBloodCellChaser[] whiteBloodCells =
            Array.Empty<RatHost2DWhiteBloodCellChaser>();
        [SerializeField] private RatHost2DMutationFragment[] mutationFragments =
            Array.Empty<RatHost2DMutationFragment>();
        [SerializeField] private Collider2D[] internalColliders = Array.Empty<Collider2D>();

        private PrototypeSessionState _state;
        private int _internalShellEntryCount;
        private readonly HashSet<int> _queuedFragmentIndices = new HashSet<int>();
        private readonly HashSet<int> _collectedFragmentIndices = new HashSet<int>();
        private bool _queuedWhiteBloodCellHit;

        public event Action<RatHost2DHudSnapshot> HostHudChanged;
        public event Action<RatHost2DVirusHudSnapshot> VirusHudChanged;
        public event Action<PrototypeGameMode> ModeChanged;

        public PrototypeSessionState State
        {
            get
            {
                EnsureInitialized();
                return _state;
            }
        }

        public PrototypeGameMode CurrentMode => State.Mode;
        public bool CanProcessHostGameplay => State.Mode == PrototypeGameMode.RatHost;
        public bool CanProcessVirusGameplay =>
            State.Mode == PrototypeGameMode.InternalVirus
            && State.CurrentInternalMinigameType
                == InternalVirusMinigameType.WhiteBloodCellEvasion;
        public bool IsInternalArenaVisible =>
            State.Mode == PrototypeGameMode.InternalVirus
            || State.Mode == PrototypeGameMode.VirusFailed;
        public bool IsVirusFailureAwaitingConfirmation =>
            State.Mode == PrototypeGameMode.VirusFailed;
        public bool IsMutationSelectionHandoff =>
            State.Mode == PrototypeGameMode.MutationSelection;
        public bool IsHostHudVisible => CanProcessHostGameplay;
        public int InternalShellEntryCount => _internalShellEntryCount;

        public void Configure(
            GameObject hostRoot,
            GameObject shellRoot,
            GameObject hudRoot,
            RatHost2DMovementController movement,
            Collider2D[] colliders)
        {
            hostModeRoot = hostRoot;
            internalShellRoot = shellRoot;
            hostHudRoot = hudRoot;
            hostMovement = movement;
            hostColliders = colliders ?? Array.Empty<Collider2D>();

            EnsureInitialized();
            ApplyModeState();
            PublishHud();
        }

        public void ConfigureStage2(
            GameObject hostCamera,
            GameObject internalCamera,
            GameObject failurePanel,
            GameObject mutationShell,
            RatHost2DVirusMovementController virus,
            RatHost2DWhiteBloodCellChaser[] chasers,
            RatHost2DMutationFragment[] fragments,
            Collider2D[] colliders)
        {
            hostCameraRoot = hostCamera;
            internalCameraRoot = internalCamera;
            failurePanelRoot = failurePanel;
            mutationSelectionShellRoot = mutationShell;
            virusMovement = virus;
            whiteBloodCells = chasers ?? Array.Empty<RatHost2DWhiteBloodCellChaser>();
            mutationFragments = fragments ?? Array.Empty<RatHost2DMutationFragment>();
            internalColliders = colliders ?? Array.Empty<Collider2D>();

            EnsureInitialized();
            ApplyModeState();
            PublishVirusHud();
        }

        public void TickHostMode(float deltaTime)
        {
            EnsureInitialized();
            if (!CanProcessHostGameplay)
            {
                return;
            }

            var previousMode = _state.Mode;
            _state.TickRatMode(Mathf.Max(0f, deltaTime));
            HandleModeChange(previousMode);
            PublishHud();
        }

        public bool ApplyContaminationExposure(float deltaTime)
        {
            return ApplyContaminationExposure(
                deltaTime,
                ContaminationAlertPerSecond,
                ContaminationHealthDamagePerSecond,
                ContaminationFeedbackLabel);
        }

        public bool ApplyContaminationExposure(
            float deltaTime,
            float alertPerSecond,
            float healthDamagePerSecond,
            string feedbackLabel)
        {
            EnsureInitialized();
            if (!CanProcessHostGameplay)
            {
                return false;
            }

            var duration = Mathf.Max(0f, deltaTime);
            if (duration <= 0f)
            {
                return false;
            }

            var previousMode = _state.Mode;
            _state.DamageHost(Mathf.Max(0f, healthDamagePerSecond) * duration);
            _state.AddImmuneAlertAmount(
                Mathf.Max(0f, alertPerSecond) * duration,
                new ImmuneAlertEvent(
                    ImmuneAlertCauseType.ContaminationExposure,
                    string.IsNullOrWhiteSpace(feedbackLabel)
                        ? ContaminationFeedbackLabel
                        : feedbackLabel.Trim()));

            HandleModeChange(previousMode);
            PublishHud();
            return true;
        }

        public RatHost2DHudSnapshot ReadHostHud()
        {
            EnsureInitialized();
            return new RatHost2DHudSnapshot(
                _state.HostHealth,
                _state.Config.HostMaxHealth,
                _state.ImmuneAlert.Value,
                _state.ImmuneAlert.MaxValue,
                _state.Mode,
                _state.LastImmuneAlertFeedbackText,
                IsHostHudVisible);
        }

        public RatHost2DVirusHudSnapshot ReadVirusHud()
        {
            EnsureInitialized();
            return new RatHost2DVirusHudSnapshot(
                _state.VirusRun.Stability,
                _state.VirusRun.StartingStability,
                _state.VirusRun.CollectedFragments,
                _state.VirusRun.RequiredFragments,
                _state.Mode,
                _state.LastVirusPatternExposureFeedbackText,
                CanProcessVirusGameplay,
                IsVirusFailureAwaitingConfirmation,
                IsMutationSelectionHandoff);
        }

        public bool QueueVirusFragmentCollected(int fragmentIndex)
        {
            EnsureInitialized();
            if (!CanProcessVirusGameplay
                || fragmentIndex < 0
                || _collectedFragmentIndices.Contains(fragmentIndex)
                || _queuedFragmentIndices.Contains(fragmentIndex))
            {
                return false;
            }

            return _queuedFragmentIndices.Add(fragmentIndex);
        }

        public bool QueueWhiteBloodCellHit()
        {
            EnsureInitialized();
            if (!CanProcessVirusGameplay)
            {
                return false;
            }

            _queuedWhiteBloodCellHit = true;
            return true;
        }

        public VirusMinigameOutcome FlushQueuedVirusFrame()
        {
            EnsureInitialized();
            if (!CanProcessVirusGameplay)
            {
                ClearQueuedVirusFrame();
                return _state.VirusRun.Outcome;
            }

            if (_queuedFragmentIndices.Count == 0 && !_queuedWhiteBloodCellHit)
            {
                return _state.VirusRun.Outcome;
            }

            var hitByWhiteBloodCell = _queuedWhiteBloodCellHit;
            var queuedIndices = new int[_queuedFragmentIndices.Count];
            _queuedFragmentIndices.CopyTo(queuedIndices);
            _queuedFragmentIndices.Clear();
            _queuedWhiteBloodCellHit = false;

            if (queuedIndices.Length == 0)
            {
                return ResolveVirusFrameNow(false, hitByWhiteBloodCell);
            }

            var fragmentsUntilSuccess = Mathf.Max(
                1,
                _state.VirusRun.RequiredFragments
                    - _state.VirusRun.CollectedFragments);
            var hitResolutionIndex = hitByWhiteBloodCell
                ? Mathf.Min(queuedIndices.Length, fragmentsUntilSuccess) - 1
                : -1;
            var outcome = _state.VirusRun.Outcome;

            for (var index = 0; index < queuedIndices.Length; index++)
            {
                if (!CanProcessVirusGameplay)
                {
                    break;
                }

                _collectedFragmentIndices.Add(queuedIndices[index]);
                outcome = ResolveVirusFrameNow(
                    true,
                    hitByWhiteBloodCell && index == hitResolutionIndex);
            }

            return outcome;
        }

        public VirusMinigameOutcome ResolveVirusFrameNow(
            bool collectedFragment,
            bool hitByWhiteBloodCell)
        {
            EnsureInitialized();
            if (!CanProcessVirusGameplay)
            {
                return _state.VirusRun.Outcome;
            }

            var previousMode = _state.Mode;
            var outcome = _state.ResolveVirusFrame(
                collectedFragment,
                hitByWhiteBloodCell);
            HandleModeChange(previousMode);
            PublishVirusHud();
            return outcome;
        }

        public bool ConfirmVirusFailureReturn()
        {
            EnsureInitialized();
            var previousMode = _state.Mode;
            if (!_state.ReturnToRatHostAfterVirusFailure())
            {
                return false;
            }

            HandleModeChange(previousMode);
            PublishHud();
            PublishVirusHud();
            return true;
        }

        public bool ProcessFailureConfirmationInput(bool confirmPressed)
        {
            return confirmPressed
                && IsVirusFailureAwaitingConfirmation
                && ConfirmVirusFailureReturn();
        }

        private void Awake()
        {
            EnsureInitialized();
            ApplyModeState();
        }

        private void Start()
        {
            PublishHud();
        }

        private void Update()
        {
            if (CanProcessHostGameplay)
            {
                TickHostMode(Time.deltaTime);
                return;
            }

            if (CanProcessVirusGameplay)
            {
                FlushQueuedVirusFrame();
                return;
            }

            if (IsVirusFailureAwaitingConfirmation
                && PrototypeKeyboardInput.WasInteractPressed())
            {
                ProcessFailureConfirmationInput(true);
            }
        }

        private void EnsureInitialized()
        {
            if (_state != null)
            {
                return;
            }

            _state = new PrototypeSessionState(new PrototypeConfig
            {
                BaseAlertPerSecond = 0f,
                DefaultInternalMinigameType = InternalVirusMinigameType.WhiteBloodCellEvasion
            });
        }

        private void HandleModeChange(PrototypeGameMode previousMode)
        {
            if (previousMode == _state.Mode)
            {
                return;
            }

            if (previousMode == PrototypeGameMode.RatHost
                && _state.Mode == PrototypeGameMode.InternalVirus)
            {
                _internalShellEntryCount++;
                ResetVirusRuntime();
            }

            ClearQueuedVirusFrame();
            ApplyModeState();
            ModeChanged?.Invoke(_state.Mode);
            PublishHud();
            PublishVirusHud();
        }

        private void ApplyModeState()
        {
            var hostActive = CanProcessHostGameplay;
            var virusActive = CanProcessVirusGameplay;
            var internalVisible = IsInternalArenaVisible;

            SetActiveIfNeeded(hostModeRoot, hostActive);
            SetActiveIfNeeded(internalShellRoot, internalVisible);
            SetActiveIfNeeded(hostHudRoot, hostActive);
            SetActiveIfNeeded(hostCameraRoot, hostActive);
            SetActiveIfNeeded(internalCameraRoot, internalVisible);
            SetActiveIfNeeded(failurePanelRoot, IsVirusFailureAwaitingConfirmation);
            SetActiveIfNeeded(mutationSelectionShellRoot, IsMutationSelectionHandoff);

            if (hostMovement != null)
            {
                hostMovement.SetHostGameplayEnabled(hostActive);
            }

            if (virusMovement != null)
            {
                virusMovement.SetVirusGameplayEnabled(virusActive);
            }

            foreach (var whiteBloodCell in whiteBloodCells ?? Array.Empty<RatHost2DWhiteBloodCellChaser>())
            {
                if (whiteBloodCell != null)
                {
                    whiteBloodCell.SetVirusGameplayEnabled(virusActive);
                }
            }

            SetCollidersEnabled(hostColliders, hostActive);
            SetCollidersEnabled(internalColliders, virusActive);
        }

        private void PublishHud()
        {
            HostHudChanged?.Invoke(ReadHostHud());
        }

        private void PublishVirusHud()
        {
            VirusHudChanged?.Invoke(ReadVirusHud());
        }

        private void ResetVirusRuntime()
        {
            _collectedFragmentIndices.Clear();
            ClearQueuedVirusFrame();
            virusMovement?.ResetRun();

            foreach (var whiteBloodCell in whiteBloodCells ?? Array.Empty<RatHost2DWhiteBloodCellChaser>())
            {
                whiteBloodCell?.ResetRun();
            }

            foreach (var fragment in mutationFragments ?? Array.Empty<RatHost2DMutationFragment>())
            {
                fragment?.ResetRun();
            }
        }

        private void ClearQueuedVirusFrame()
        {
            _queuedFragmentIndices.Clear();
            _queuedWhiteBloodCellHit = false;
        }

        private static void SetCollidersEnabled(Collider2D[] colliders, bool enabled)
        {
            foreach (var target in colliders ?? Array.Empty<Collider2D>())
            {
                if (target != null)
                {
                    target.enabled = enabled;
                }
            }
        }

        private static void SetActiveIfNeeded(GameObject target, bool active)
        {
            if (target != null && target.activeSelf != active)
            {
                target.SetActive(active);
            }
        }
    }
}
