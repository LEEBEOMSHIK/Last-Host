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
        public bool Retry;
    }

    public static class PrototypeKeyboardInput
    {
        public static PrototypeInputState ReadCurrent()
        {
            return new PrototypeInputState
            {
                MoveLeft = IsPressed(Key.A, KeyCode.A) || IsPressed(Key.LeftArrow, KeyCode.LeftArrow),
                MoveRight = IsPressed(Key.D, KeyCode.D) || IsPressed(Key.RightArrow, KeyCode.RightArrow),
                MoveDown = IsPressed(Key.S, KeyCode.S) || IsPressed(Key.DownArrow, KeyCode.DownArrow),
                MoveUp = IsPressed(Key.W, KeyCode.W) || IsPressed(Key.UpArrow, KeyCode.UpArrow),
                SelectMutation1 = WasPressedThisFrame(Key.Digit1, KeyCode.Alpha1),
                SelectMutation2 = WasPressedThisFrame(Key.Digit2, KeyCode.Alpha2),
                SelectMutation3 = WasPressedThisFrame(Key.Digit3, KeyCode.Alpha3),
                Retry = WasPressedThisFrame(Key.Space, KeyCode.Space)
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
