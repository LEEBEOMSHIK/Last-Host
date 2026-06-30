using System.Text;
using LastHost.Prototype.Core;
using LastHost.Prototype.Mutations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace LastHost.Prototype.UI
{
    public sealed class PrototypeHud : MonoBehaviour
    {
        public Text modeText;
        public Text hostHealthText;
        public Text immuneAlertText;
        public Text virusStabilityText;
        public Text mutationFragmentsText;
        public Text mutationStatusText;
        public Text objectiveText;
        public Slider hostHealthSlider;
        public Slider immuneAlertSlider;
        public Slider virusStabilitySlider;
        public GameObject mutationPanel;
        public GameObject failurePanel;

        private PrototypeSessionController session;
        private readonly StringBuilder mutationBuilder = new StringBuilder();

        private void Awake()
        {
            EnsureEventSystem();
        }

        public void Bind(PrototypeSessionController controller)
        {
            session = controller;
        }

        public void Refresh(PrototypeSessionState state)
        {
            SetText(modeText, GetModeName(state.Mode));
            SetText(hostHealthText, $"숙주 생명력 {state.HostHealth:0}/{state.Config.HostMaxHealth:0}");
            SetText(immuneAlertText, $"면역 경계도 {state.ImmuneAlert.Value:0}%");
            SetText(virusStabilityText, $"바이러스 안정도 {state.VirusRun.Stability:0}/{state.VirusRun.StartingStability:0}");
            SetText(mutationFragmentsText, $"변이 조각 {state.VirusRun.CollectedFragments}/{state.VirusRun.RequiredFragments}");
            SetText(mutationStatusText, BuildMutationStatus(state.Mutations));
            SetText(objectiveText, GetObjective(state));

            SetSlider(hostHealthSlider, state.HostHealth / state.Config.HostMaxHealth);
            SetSlider(immuneAlertSlider, state.ImmuneAlert.NormalizedValue);
            SetSlider(virusStabilitySlider, state.VirusRun.NormalizedStability);

            if (mutationPanel != null)
            {
                mutationPanel.SetActive(state.Mode == PrototypeGameMode.MutationSelection);
            }

            if (failurePanel != null)
            {
                failurePanel.SetActive(state.Mode == PrototypeGameMode.VirusFailed);
            }
        }

        public void SelectDormancy()
        {
            session?.SelectMutation(MutationType.Dormancy);
        }

        public void SelectNeuralControl()
        {
            session?.SelectMutation(MutationType.NeuralControl);
        }

        public void SelectMammalAdaptation()
        {
            session?.SelectMutation(MutationType.MammalAdaptation);
        }

        public void RetryVirusMinigame()
        {
            session?.RetryVirusMinigame();
        }

        private string BuildMutationStatus(MutationLoadout loadout)
        {
            mutationBuilder.Clear();

            AppendMutation(loadout, MutationType.Dormancy);
            AppendMutation(loadout, MutationType.NeuralControl);
            AppendMutation(loadout, MutationType.MammalAdaptation);

            return mutationBuilder.Length == 0 ? "획득 변이 없음" : mutationBuilder.ToString();
        }

        private void AppendMutation(MutationLoadout loadout, MutationType type)
        {
            if (!loadout.Has(type))
            {
                return;
            }

            if (mutationBuilder.Length > 0)
            {
                mutationBuilder.Append(" / ");
            }

            mutationBuilder.Append(MutationDefinition.GetName(type));
        }

        private static string GetModeName(PrototypeGameMode mode)
        {
            switch (mode)
            {
                case PrototypeGameMode.RatHost:
                    return "쥐 숙주";
                case PrototypeGameMode.InternalVirus:
                    return "내부 바이러스";
                case PrototypeGameMode.VirusFailed:
                    return "바이러스 실패";
                case PrototypeGameMode.MutationSelection:
                    return "변이 선택";
                default:
                    return mode.ToString();
            }
        }

        private static string GetObjective(PrototypeSessionState state)
        {
            switch (state.Mode)
            {
                case PrototypeGameMode.RatHost:
                    return state.Mutations.CanUseMammalPassage ? "포유류 통로 개방됨" : "하수도 탐색 중";
                case PrototypeGameMode.InternalVirus:
                    return "변이 조각 수집 / 백혈구 회피";
                case PrototypeGameMode.VirusFailed:
                    return "바이러스 안정도 소진";
                case PrototypeGameMode.MutationSelection:
                    return "변이 선택";
                default:
                    return string.Empty;
            }
        }

        private static void SetText(Text text, string value)
        {
            if (text != null)
            {
                text.text = value;
            }
        }

        private static void SetSlider(Slider slider, float value)
        {
            if (slider != null)
            {
                slider.value = Mathf.Clamp01(value);
            }
        }

        private static void EnsureEventSystem()
        {
            if (UnityEngine.Object.FindAnyObjectByType<EventSystem>(FindObjectsInactive.Include) != null)
            {
                return;
            }

            var eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<InputSystemUIInputModule>();
        }
    }
}
