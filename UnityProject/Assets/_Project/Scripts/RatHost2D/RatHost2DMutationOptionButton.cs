using LastHost.Prototype.Mutations;
using UnityEngine;
using UnityEngine.UI;

namespace LastHost.Prototype.RatHost2D
{
    [RequireComponent(typeof(Button))]
    public sealed class RatHost2DMutationOptionButton : MonoBehaviour
    {
        [SerializeField] private RatHost2DSessionController session;
        [SerializeField] private MutationType mutationType;
        [SerializeField] private Text label;

        private Button _button;

        public MutationType MutationType => mutationType;

        public void Configure(
            RatHost2DSessionController sessionController,
            MutationType type,
            Text optionLabel)
        {
            session = sessionController;
            mutationType = type;
            label = optionLabel;
            EnsureButtonListener();
            RefreshLabel();
        }

        public void RefreshLabel()
        {
            if (label != null)
            {
                label.text =
                    $"{MutationDefinition.GetName(mutationType)}\n"
                    + MutationDefinition.GetPrototypeEffect(mutationType);
            }
        }

        public bool SelectMutation()
        {
            return session != null && session.TrySelectMutation(mutationType);
        }

        private void Awake()
        {
            EnsureButtonListener();
            RefreshLabel();
        }

        private void OnDestroy()
        {
            if (_button != null)
            {
                _button.onClick.RemoveListener(HandleButtonClicked);
            }
        }

        private void EnsureButtonListener()
        {
            if (_button != null)
            {
                return;
            }

            _button = GetComponent<Button>();
            if (_button != null)
            {
                _button.onClick.AddListener(HandleButtonClicked);
            }
        }

        private void HandleButtonClicked()
        {
            SelectMutation();
        }
    }
}
