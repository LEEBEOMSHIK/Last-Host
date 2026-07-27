using UnityEngine;

namespace LastHost.Prototype.TechnicalSample2D
{
    public static class Movement2DModel
    {
        private const float InputEpsilonSquared = 0.000001f;

        public static Vector2 NormalizeInput(Vector2 rawInput)
        {
            if (rawInput.sqrMagnitude <= InputEpsilonSquared)
            {
                return Vector2.zero;
            }

            return rawInput.sqrMagnitude > 1f ? rawInput.normalized : rawInput;
        }

        public static Vector2 CalculateStep(Vector2 normalizedInput, float speed, float deltaTime)
        {
            return NormalizeInput(normalizedInput) * Mathf.Max(0f, speed) * Mathf.Max(0f, deltaTime);
        }
    }
}
