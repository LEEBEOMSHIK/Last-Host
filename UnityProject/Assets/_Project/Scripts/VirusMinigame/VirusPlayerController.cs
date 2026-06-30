using LastHost.Prototype.Input;
using UnityEngine;

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
            var move = ReadMoveInput();
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

        private static Vector3 ReadMoveInput()
        {
            var mainCamera = Camera.main;
            return PrototypeKeyboardInput.ReadCameraRelativeMoveInput(mainCamera != null ? mainCamera.transform : null);
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
