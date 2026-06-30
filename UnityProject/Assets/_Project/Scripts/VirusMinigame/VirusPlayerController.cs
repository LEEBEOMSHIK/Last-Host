using UnityEngine;
using UnityEngine.InputSystem;

namespace LastHost.Prototype.VirusMinigame
{
    public sealed class VirusPlayerController : MonoBehaviour
    {
        public float speed = 4.2f;
        public Vector2 xBounds = new Vector2(-4.8f, 4.8f);
        public Vector2 zBounds = new Vector2(-2.8f, 2.8f);

        private Vector3 startPosition;

        private void Awake()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            var moveInput = ReadMoveInput();
            var move = new Vector3(moveInput.x, 0f, moveInput.y);
            if (move.sqrMagnitude > 1f)
            {
                move.Normalize();
            }

            transform.position += move * (speed * Time.deltaTime);
            ClampToArena();
        }

        public void ResetToStart()
        {
            transform.position = startPosition;
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

        private void ClampToArena()
        {
            var position = transform.position;
            position.x = Mathf.Clamp(position.x, xBounds.x, xBounds.y);
            position.z = Mathf.Clamp(position.z, zBounds.x, zBounds.y);
            transform.position = position;
        }
    }
}
