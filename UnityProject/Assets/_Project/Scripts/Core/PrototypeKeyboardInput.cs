using LastHost.Prototype.Mutations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LastHost.Prototype.Input
{
    public struct PrototypeInputState
    {
        public bool MoveLeft;
        public bool MoveRight;
        public bool MoveDown;
        public bool MoveUp;
        public bool SelectMutation1;
        public bool SelectMutation2;
        public bool SelectMutation3;
        public bool Interact;
        public bool Retry;
        public bool ToggleCameraMode;
    }

    public static class PrototypeKeyboardInput
    {
        public static PrototypeInputState ReadCurrent()
        {
            var spacePressed = WasPressedThisFrame(Key.Space, KeyCode.Space);

            return new PrototypeInputState
            {
                MoveLeft = IsPressed(Key.A, KeyCode.A) || IsPressed(Key.LeftArrow, KeyCode.LeftArrow),
                MoveRight = IsPressed(Key.D, KeyCode.D) || IsPressed(Key.RightArrow, KeyCode.RightArrow),
                MoveDown = IsPressed(Key.S, KeyCode.S) || IsPressed(Key.DownArrow, KeyCode.DownArrow),
                MoveUp = IsPressed(Key.W, KeyCode.W) || IsPressed(Key.UpArrow, KeyCode.UpArrow),
                SelectMutation1 = WasPressedThisFrame(Key.Digit1, KeyCode.Alpha1),
                SelectMutation2 = WasPressedThisFrame(Key.Digit2, KeyCode.Alpha2),
                SelectMutation3 = WasPressedThisFrame(Key.Digit3, KeyCode.Alpha3),
                Interact = spacePressed,
                Retry = spacePressed,
                ToggleCameraMode = WasPressedThisFrame(Key.V, KeyCode.V)
            };
        }

        public static Vector2 ReadMoveInput()
        {
            return ComposeMoveInput(ReadCurrent());
        }

        public static Vector2 ComposeMoveInput(PrototypeInputState input)
        {
            var x = 0f;
            var y = 0f;

            if (input.MoveLeft)
            {
                x -= 1f;
            }

            if (input.MoveRight)
            {
                x += 1f;
            }

            if (input.MoveDown)
            {
                y -= 1f;
            }

            if (input.MoveUp)
            {
                y += 1f;
            }

            var move = new Vector2(x, y);
            return move.sqrMagnitude > 1f ? move.normalized : move;
        }

        public static Vector3 ReadCameraRelativeMoveInput(Transform cameraTransform)
        {
            return ComposeCameraRelativeMoveInput(ReadCurrent(), cameraTransform);
        }

        public static Vector3 ComposeCameraRelativeMoveInput(PrototypeInputState input, Transform cameraTransform)
        {
            var move = ComposeMoveInput(input);
            if (move == Vector2.zero)
            {
                return Vector3.zero;
            }

            if (cameraTransform == null)
            {
                return new Vector3(move.x, 0f, move.y);
            }

            var forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
            if (forward.sqrMagnitude < 0.0001f)
            {
                forward = Vector3.forward;
            }

            var right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up);
            if (right.sqrMagnitude < 0.0001f)
            {
                right = Vector3.right;
            }

            var worldMove = (right.normalized * move.x) + (forward.normalized * move.y);
            return worldMove.sqrMagnitude > 1f ? worldMove.normalized : worldMove;
        }

        public static bool TryGetSelectedMutation(PrototypeInputState input, out MutationType type)
        {
            if (input.SelectMutation1)
            {
                type = MutationType.Dormancy;
                return true;
            }

            if (input.SelectMutation2)
            {
                type = MutationType.NeuralControl;
                return true;
            }

            if (input.SelectMutation3)
            {
                type = MutationType.MammalAdaptation;
                return true;
            }

            type = default;
            return false;
        }

        public static bool WasRetryPressed()
        {
            return ReadCurrent().Retry;
        }

        public static bool WasInteractPressed()
        {
            return WasInteractPressed(ReadCurrent());
        }

        public static bool WasInteractPressed(PrototypeInputState input)
        {
            return input.Interact;
        }

        public static bool WasCameraToggleRequested()
        {
            return WasCameraToggleRequested(ReadCurrent());
        }

        public static bool WasCameraToggleRequested(PrototypeInputState input)
        {
            return input.ToggleCameraMode;
        }

        private static bool IsPressed(Key inputSystemKey, KeyCode legacyKey)
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard[inputSystemKey].isPressed)
            {
                return true;
            }

            return TryLegacyInput(() => UnityEngine.Input.GetKey(legacyKey));
        }

        private static bool WasPressedThisFrame(Key inputSystemKey, KeyCode legacyKey)
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard[inputSystemKey].wasPressedThisFrame)
            {
                return true;
            }

            return TryLegacyInput(() => UnityEngine.Input.GetKeyDown(legacyKey));
        }

        private static bool TryLegacyInput(System.Func<bool> read)
        {
            try
            {
                return read();
            }
            catch (System.InvalidOperationException)
            {
                return false;
            }
        }
    }
}
