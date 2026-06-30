using LastHost.Prototype.Core;
using LastHost.Prototype.Mutations;
using UnityEngine;
using UnityEngine.UI;

namespace LastHost.Prototype.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class MutationOptionButton : MonoBehaviour
    {
        public PrototypeSessionController session;
        public MutationType mutationType;
        public Text label;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(SelectMutation);
            RefreshLabel();
        }

        private void OnDestroy()
        {
            if (button != null)
            {
                button.onClick.RemoveListener(SelectMutation);
            }
        }

        public void RefreshLabel()
        {
            if (label == null)
            {
                return;
            }

            label.text = $"{MutationDefinition.GetName(mutationType)}\n{MutationDefinition.GetPrototypeEffect(mutationType)}";
        }

        private void SelectMutation()
        {
            session?.SelectMutation(mutationType);
        }
    }
}
