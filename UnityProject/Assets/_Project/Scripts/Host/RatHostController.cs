using LastHost.Prototype.Core;
using UnityEngine;
using UnityEngine.InputSystem;

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
            if (Keyboard.current == null)
            {
                return Vector2.zero;
            }

            var x = 0f;
            var y = 0f;

            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                x -= 1f;
            }

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                x += 1f;
            }

            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            {
                y -= 1f;
            }

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            {
                y += 1f;
            }

            return new Vector2(x, y);
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
