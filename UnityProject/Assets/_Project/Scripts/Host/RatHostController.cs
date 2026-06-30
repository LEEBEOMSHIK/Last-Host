using LastHost.Prototype.Core;
using LastHost.Prototype.Input;
using UnityEngine;

namespace LastHost.Prototype.Host
{
    public sealed class RatHostController : MonoBehaviour
    {
        public PrototypeSessionController session;
        public float baseSpeed = 3.2f;
        public Vector2 xBounds = new Vector2(-5.4f, 5.4f);
        public Vector2 zBounds = new Vector2(-3.4f, 3.4f);

        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            var moveInput = ReadMoveInput();
            var move = new Vector3(moveInput.x, 0f, moveInput.y);
            if (move.sqrMagnitude > 1f)
            {
                move.Normalize();
            }

            var speedMultiplier = session != null ? session.State.Mutations.RatSpeedMultiplier : 1f;
            var delta = move * (baseSpeed * speedMultiplier * Time.deltaTime);

            if (characterController != null)
            {
                characterController.Move(delta);
            }
            else
            {
                transform.position += delta;
            }

            ClampToSewerBounds();

            if (move.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(move, Vector3.up);
            }
        }

        private static Vector2 ReadMoveInput()
        {
            return PrototypeKeyboardInput.ReadMoveInput();
        }

        private void ClampToSewerBounds()
        {
            var position = transform.position;
            position.x = Mathf.Clamp(position.x, xBounds.x, xBounds.y);
            position.z = Mathf.Clamp(position.z, zBounds.x, zBounds.y);
            transform.position = position;
        }
    }
}
