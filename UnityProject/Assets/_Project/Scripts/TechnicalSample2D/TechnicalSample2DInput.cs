using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LastHost.Prototype.TechnicalSample2D
{
    public sealed class TechnicalSample2DInput : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;

        private InputAction _moveAction;
        private bool _enabledByThisComponent;

        public InputAction MoveAction => _moveAction;

        public void Configure(InputActionAsset asset)
        {
            if (isActiveAndEnabled)
            {
                DisableOwnedAction();
            }

            inputActions = asset;
            ResolveMoveAction();

            if (isActiveAndEnabled)
            {
                EnableResolvedAction();
            }
        }

        public Vector2 ReadMove()
        {
            return _moveAction == null ? Vector2.zero : _moveAction.ReadValue<Vector2>();
        }

        private void OnEnable()
        {
            ResolveMoveAction();
            EnableResolvedAction();
        }

        private void OnDisable()
        {
            DisableOwnedAction();
        }

        private void ResolveMoveAction()
        {
            _moveAction = inputActions == null
                ? null
                : inputActions.FindAction(TechnicalSample2DConstants.MoveActionPath, false);

            if (inputActions != null && _moveAction == null)
            {
                throw new InvalidOperationException(
                    $"Input action '{TechnicalSample2DConstants.MoveActionPath}' was not found.");
            }
        }

        private void EnableResolvedAction()
        {
            if (_moveAction == null || _moveAction.enabled)
            {
                _enabledByThisComponent = false;
                return;
            }

            _moveAction.Enable();
            _enabledByThisComponent = true;
        }

        private void DisableOwnedAction()
        {
            if (_enabledByThisComponent && _moveAction != null)
            {
                _moveAction.Disable();
            }

            _enabledByThisComponent = false;
        }
    }
}
