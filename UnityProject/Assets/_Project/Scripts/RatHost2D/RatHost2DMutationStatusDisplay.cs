using LastHost.Prototype.Mutations;
using UnityEngine;
using UnityEngine.UI;

namespace LastHost.Prototype.RatHost2D
{
    public sealed class RatHost2DMutationStatusDisplay : MonoBehaviour
    {
        [SerializeField] private RatHost2DSessionController session;
        [SerializeField] private Text statusText;

        public void Configure(
            RatHost2DSessionController sessionController,
            Text appliedMutationText)
        {
            session = sessionController;
            statusText = appliedMutationText;
            RefreshNow();
        }

        public void RefreshNow()
        {
            if (statusText == null)
            {
                return;
            }

            statusText.text = session == null
                ? string.Empty
                : BuildStatusText(session);
        }

        private void OnEnable()
        {
            RefreshNow();
        }

        private void Update()
        {
            RefreshNow();
        }

        private static string BuildStatusText(
            RatHost2DSessionController sessionController)
        {
            var loadout = sessionController.State.Mutations;
            var hasDormancy = loadout.Has(MutationType.Dormancy);
            var hasNeuralControl = loadout.Has(MutationType.NeuralControl);
            var hasMammalAdaptation =
                loadout.Has(MutationType.MammalAdaptation);

            if (!hasDormancy && !hasNeuralControl && !hasMammalAdaptation)
            {
                return "적용 변이 없음";
            }

            var result = "적용 변이 ";
            if (hasDormancy)
            {
                result += MutationDefinition.GetName(MutationType.Dormancy);
            }

            if (hasNeuralControl)
            {
                result += hasDormancy ? " / " : string.Empty;
                result += MutationDefinition.GetName(MutationType.NeuralControl);
            }

            if (hasMammalAdaptation)
            {
                result += hasDormancy || hasNeuralControl
                    ? " / "
                    : string.Empty;
                result += MutationDefinition.GetName(
                    MutationType.MammalAdaptation);
            }

            return result;
        }
    }
}
