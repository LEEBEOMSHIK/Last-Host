using LastHost.Prototype.Immune;
using LastHost.Prototype.Mutations;
using LastHost.Prototype.VirusMinigame;
using UnityEngine;

namespace LastHost.Prototype.Core
{
    public sealed class PrototypeSessionState
    {
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

        public bool TickRatMode(float deltaTime)
        {
            if (Mode != PrototypeGameMode.RatHost)
            {
                return false;
            }

            if (!ImmuneAlert.Tick(deltaTime, Mutations.ImmuneAlertRateMultiplier))
            {
                return false;
            }

            EnterVirusMinigame();
            return true;
        }

        public bool AddRiskAlert(float severityMultiplier)
        {
            if (Mode != PrototypeGameMode.RatHost)
            {
                return false;
            }

            if (!ImmuneAlert.AddRiskEvent(severityMultiplier))
            {
                return false;
            }

            EnterVirusMinigame();
            return true;
        }

        public bool AddImmuneAlertAmount(float amount)
        {
            if (Mode != PrototypeGameMode.RatHost)
            {
                return false;
            }

            if (!ImmuneAlert.AddRawAmount(amount))
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
            if (Mode != PrototypeGameMode.VirusFailed)
            {
                return false;
            }

            VirusRun.ResetRun();
            Mode = PrototypeGameMode.InternalVirus;
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
            VirusRun.ResetRun();
            Mode = PrototypeGameMode.RatHost;
            return true;
        }
    }
}
