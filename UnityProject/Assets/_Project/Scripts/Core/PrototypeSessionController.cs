using LastHost.Prototype.Host;
using LastHost.Prototype.Input;
using LastHost.Prototype.Mutations;
using LastHost.Prototype.UI;
using LastHost.Prototype.VirusMinigame;
using UnityEngine;

namespace LastHost.Prototype.Core
{
    public sealed class PrototypeSessionController : MonoBehaviour
    {
        [Header("Mode Roots")]
        public GameObject ratHostModeRoot;
        public GameObject virusMinigameModeRoot;

        [Header("Controllers")]
        public RatHostController ratHostController;
        public VirusMinigameController virusMinigameController;
        public PrototypeHud hud;
        public InternalVirusMinigameType defaultInternalMinigameType = InternalVirusMinigameType.WhiteBloodCellEvasion;

        [Header("Debug")]
        public bool enableDebugMinigameHotkeys = true;

        public PrototypeSessionState State { get; private set; }
        public PrototypeGameMode CurrentMode => State.Mode;

        private void Awake()
        {
            State = new PrototypeSessionState(new PrototypeConfig
            {
                DefaultInternalMinigameType = defaultInternalMinigameType
            });
            AutoWireIfNeeded();
        }

        private void Start()
        {
            ApplyModeVisibility(resetVirusScene: true);
            hud?.Bind(this);
            hud?.Refresh(State);
        }

        private void Update()
        {
            var previousMode = State.Mode;

            if (State.Mode == PrototypeGameMode.RatHost)
            {
                if (enableDebugMinigameHotkeys && PrototypeKeyboardInput.WasDebugSignalSuppressionRequested())
                {
                    EnterImmuneSignalSuppressionMinigame();
                    return;
                }

                State.TickRatMode(Time.deltaTime);
            }
            else if (State.Mode == PrototypeGameMode.MutationSelection)
            {
                if (PrototypeKeyboardInput.TryGetSelectedMutation(PrototypeKeyboardInput.ReadCurrent(), out var selectedMutation))
                {
                    SelectMutation(selectedMutation);
                    return;
                }
            }
            else if (State.Mode == PrototypeGameMode.InternalVirus && State.CurrentInternalMinigameType == InternalVirusMinigameType.ImmuneSignalSuppression)
            {
                State.TickSignalSuppression(Time.deltaTime);
                if (State.Mode == PrototypeGameMode.InternalVirus && PrototypeKeyboardInput.WasInteractPressed())
                {
                    State.ResolveSignalSuppressionInput();
                }
            }
            else if (State.Mode == PrototypeGameMode.VirusFailed && PrototypeKeyboardInput.WasRetryPressed())
            {
                ReturnToRatHostAfterVirusFailure();
                return;
            }

            if (previousMode != State.Mode)
            {
                OnModeChanged(previousMode, State.Mode);
            }

            hud?.Refresh(State);
        }

        public bool AddRiskAlert(float severityMultiplier)
        {
            return AddRiskAlert(severityMultiplier, new ImmuneAlertEvent(ImmuneAlertCauseType.Unspecified, string.Empty));
        }

        public bool AddRiskAlert(float severityMultiplier, string feedbackLabel)
        {
            return AddRiskAlert(severityMultiplier, new ImmuneAlertEvent(ImmuneAlertCauseType.Unspecified, feedbackLabel));
        }

        public bool AddRiskAlert(float severityMultiplier, ImmuneAlertEvent alertEvent)
        {
            var previousMode = State.Mode;
            var previousAlert = State.ImmuneAlert.Value;
            var enteredVirusMode = State.AddRiskAlert(severityMultiplier, alertEvent);
            if (previousMode != State.Mode)
            {
                OnModeChanged(previousMode, State.Mode);
            }

            if (previousMode != State.Mode || !Mathf.Approximately(previousAlert, State.ImmuneAlert.Value))
            {
                hud?.Refresh(State);
            }

            return enteredVirusMode;
        }

        public bool AddImmuneAlertAmount(float amount)
        {
            return AddImmuneAlertAmount(amount, new ImmuneAlertEvent(ImmuneAlertCauseType.Unspecified, string.Empty));
        }

        public bool AddImmuneAlertAmount(float amount, string feedbackLabel)
        {
            return AddImmuneAlertAmount(amount, new ImmuneAlertEvent(ImmuneAlertCauseType.Unspecified, feedbackLabel));
        }

        public bool AddImmuneAlertAmount(float amount, ImmuneAlertEvent alertEvent)
        {
            var previousMode = State.Mode;
            var previousAlert = State.ImmuneAlert.Value;
            var enteredVirusMode = State.AddImmuneAlertAmount(amount, alertEvent);
            if (previousMode != State.Mode)
            {
                OnModeChanged(previousMode, State.Mode);
            }

            if (previousMode != State.Mode || !Mathf.Approximately(previousAlert, State.ImmuneAlert.Value))
            {
                hud?.Refresh(State);
            }

            return enteredVirusMode;
        }

        public void DamageHost(float amount)
        {
            State.DamageHost(amount);
            hud?.Refresh(State);
        }

        public void SetRatRiskInteractionAffordance(bool isAvailable, string prompt)
        {
            if (State == null)
            {
                return;
            }

            if (State.SetRatRiskInteractionAffordance(isAvailable, prompt))
            {
                hud?.Refresh(State);
            }
        }

        public VirusMinigameOutcome ResolveVirusFrame(bool collectedFragment, bool hitByWhiteBloodCell)
        {
            var previousMode = State.Mode;
            var outcome = State.ResolveVirusFrame(collectedFragment, hitByWhiteBloodCell);
            if (previousMode != State.Mode)
            {
                OnModeChanged(previousMode, State.Mode);
            }

            hud?.Refresh(State);
            return outcome;
        }

        public void EnterImmuneSignalSuppressionMinigame()
        {
            var previousMode = State.Mode;
            State.EnterVirusMinigame(InternalVirusMinigameType.ImmuneSignalSuppression);
            OnModeChanged(previousMode, State.Mode);
            hud?.Refresh(State);
        }

        public ImmuneSignalSuppressionJudgement ResolveSignalSuppressionInput()
        {
            var previousMode = State.Mode;
            var judgement = State.ResolveSignalSuppressionInput();
            if (previousMode != State.Mode)
            {
                OnModeChanged(previousMode, State.Mode);
            }

            hud?.Refresh(State);
            return judgement;
        }

        public void RetryVirusMinigame()
        {
            ReturnToRatHostAfterVirusFailure();
        }

        public void ReturnToRatHostAfterVirusFailure()
        {
            var previousMode = State.Mode;
            if (!State.ReturnToRatHostAfterVirusFailure())
            {
                return;
            }

            OnModeChanged(previousMode, State.Mode);
            hud?.Refresh(State);
        }

        public void SelectMutation(MutationType type)
        {
            var previousMode = State.Mode;
            if (!State.SelectMutation(type))
            {
                return;
            }

            OnModeChanged(previousMode, State.Mode);
            hud?.Refresh(State);
        }

        private void AutoWireIfNeeded()
        {
            if (ratHostController == null)
            {
                ratHostController = FindAnyObjectByType<RatHostController>(FindObjectsInactive.Include);
            }

            if (virusMinigameController == null)
            {
                virusMinigameController = FindAnyObjectByType<VirusMinigameController>(FindObjectsInactive.Include);
            }

            if (hud == null)
            {
                hud = FindAnyObjectByType<PrototypeHud>(FindObjectsInactive.Include);
            }

            if (ratHostController != null)
            {
                ratHostController.session = this;
            }

            if (virusMinigameController != null)
            {
                virusMinigameController.session = this;
            }
        }

        private void OnModeChanged(PrototypeGameMode previousMode, PrototypeGameMode nextMode)
        {
            ApplyModeVisibility(resetVirusScene: nextMode == PrototypeGameMode.InternalVirus
                && State.CurrentInternalMinigameType == InternalVirusMinigameType.WhiteBloodCellEvasion);
        }

        private void ApplyModeVisibility(bool resetVirusScene)
        {
            var ratActive = State.Mode == PrototypeGameMode.RatHost;
            var virusVisible = State.Mode == PrototypeGameMode.InternalVirus || State.Mode == PrototypeGameMode.VirusFailed;
            var virusPlayable = State.Mode == PrototypeGameMode.InternalVirus
                && State.CurrentInternalMinigameType == InternalVirusMinigameType.WhiteBloodCellEvasion;

            if (ratHostModeRoot != null)
            {
                ratHostModeRoot.SetActive(ratActive);
            }

            if (virusMinigameModeRoot != null)
            {
                virusMinigameModeRoot.SetActive(virusVisible);
            }

            if (ratHostController != null)
            {
                ratHostController.enabled = ratActive;
            }

            if (virusMinigameController != null)
            {
                if (resetVirusScene)
                {
                    virusMinigameController.ResetRun();
                }

                virusMinigameController.SetGameplayActive(virusPlayable);
            }
        }
    }
}
