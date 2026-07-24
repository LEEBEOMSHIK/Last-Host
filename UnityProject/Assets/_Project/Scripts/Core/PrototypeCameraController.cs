using LastHost.Prototype.Core;
using LastHost.Prototype.Host;
using LastHost.Prototype.Input;
using LastHost.Prototype.VirusMinigame;
using UnityEngine;

namespace LastHost.Prototype.Cameras
{
    public enum PrototypeCameraMode
    {
        ThirdPerson,
        QuarterView,
        TopView
    }

    [RequireComponent(typeof(Camera))]
    public sealed class PrototypeCameraController : MonoBehaviour
    {
        [Header("Mode")]
        public PrototypeSessionController session;
        public PrototypeCameraMode startingHostMode = PrototypeCameraMode.ThirdPerson;

        [Header("Targets")]
        public Transform hostTarget;
        public Transform virusTarget;

        [Header("Third Person")]
        public Vector3 thirdPersonOffset = new Vector3(0f, 3.6f, -5.2f);
        public float thirdPersonFocusHeight = 0.55f;
        public float thirdPersonFieldOfView = 50f;

        [Header("Quarter View")]
        public Vector3 quarterViewOffset = new Vector3(-2.8f, 7.4f, -6.4f);
        // RatHost is a ground/collision root while RatVisual is a raised sprite child.
        // Aim at the rendered body centre rather than the root/feet so switching
        // directional sprites keeps the visible rat anchored in the quarter-view frame.
        public float quarterViewFocusHeight = 0.9f;
        public float quarterViewOrthographicSize = 5.2f;

        [Header("Quarter View Output Pixel Snap")]
        [Tooltip("Snaps only the final quarter-view camera output position to the internal render grid.")]
        public bool enableQuarterViewOutputPixelSnap;
        [Min(1)]
        public int quarterViewOutputPixelHeight = 540;

        [Header("Top View")]
        public Vector3 topViewOffset = new Vector3(0f, 9.5f, 0f);
        public float topViewOrthographicSize = 5.8f;

        [Header("Virus View")]
        public Vector3 virusViewOffset = new Vector3(0f, 8f, -0.15f);
        public float virusViewOrthographicSize = 4.4f;

        [Header("Follow")]
        public float followSharpness = 18f;

        private Camera attachedCamera;
        private bool modeInitialized;

        public PrototypeCameraMode CurrentHostMode { get; private set; }

        private void Awake()
        {
            EnsureInitialized();
            AutoWireIfNeeded();
        }

        private void Start()
        {
            ApplyCameraNow();
        }

        private void LateUpdate()
        {
            EnsureInitialized();
            AutoWireIfNeeded();

            if (ResolveMode() == PrototypeGameMode.RatHost && PrototypeKeyboardInput.WasCameraToggleRequested())
            {
                ToggleHostCameraMode();
            }

            ApplyCamera(ResolveMode(), Time.deltaTime, snap: false);
        }

        public void ToggleHostCameraMode()
        {
            EnsureInitialized();

            switch (CurrentHostMode)
            {
                case PrototypeCameraMode.ThirdPerson:
                    CurrentHostMode = PrototypeCameraMode.QuarterView;
                    break;
                case PrototypeCameraMode.QuarterView:
                    CurrentHostMode = PrototypeCameraMode.TopView;
                    break;
                default:
                    CurrentHostMode = PrototypeCameraMode.ThirdPerson;
                    break;
            }
        }

        public void ApplyCameraNow()
        {
            ApplyCamera(ResolveMode(), 0f, snap: true);
        }

        public void ApplyCameraNow(PrototypeGameMode mode)
        {
            ApplyCamera(mode, 0f, snap: true);
        }

        private void EnsureInitialized()
        {
            if (attachedCamera == null)
            {
                attachedCamera = GetComponent<Camera>();
            }

            if (!modeInitialized)
            {
                CurrentHostMode = startingHostMode;
                modeInitialized = true;
            }
        }

        private void AutoWireIfNeeded()
        {
            if (session == null)
            {
                session = FindAnyObjectByType<PrototypeSessionController>(FindObjectsInactive.Include);
            }

            if (hostTarget == null)
            {
                var rat = FindAnyObjectByType<RatHostController>(FindObjectsInactive.Include);
                if (rat != null)
                {
                    hostTarget = rat.transform;
                }
            }

            if (virusTarget == null)
            {
                var virus = FindAnyObjectByType<VirusPlayerController>(FindObjectsInactive.Include);
                if (virus != null)
                {
                    virusTarget = virus.transform;
                }
            }

        }

        private PrototypeGameMode ResolveMode()
        {
            return session != null ? session.CurrentMode : PrototypeGameMode.RatHost;
        }

        private void ApplyCamera(PrototypeGameMode mode, float deltaTime, bool snap)
        {
            EnsureInitialized();

            if (mode == PrototypeGameMode.InternalVirus || mode == PrototypeGameMode.VirusFailed)
            {
                ApplyVirusView(deltaTime, snap);
                return;
            }

            if (CurrentHostMode == PrototypeCameraMode.TopView)
            {
                ApplyTopView(deltaTime, snap);
            }
            else if (CurrentHostMode == PrototypeCameraMode.QuarterView)
            {
                ApplyQuarterView(deltaTime, snap);
            }
            else
            {
                ApplyThirdPerson(deltaTime, snap);
            }
        }

        private void ApplyThirdPerson(float deltaTime, bool snap)
        {
            if (hostTarget == null)
            {
                return;
            }

            var focus = hostTarget.position + Vector3.up * thirdPersonFocusHeight;
            var position = hostTarget.position + thirdPersonOffset;
            var rotation = Quaternion.LookRotation(focus - position, Vector3.up);

            attachedCamera.orthographic = false;
            attachedCamera.fieldOfView = thirdPersonFieldOfView;
            MoveCamera(position, rotation, deltaTime, snap);
        }

        private void ApplyQuarterView(float deltaTime, bool snap)
        {
            if (hostTarget == null)
            {
                return;
            }

            var focus = hostTarget.position + Vector3.up * quarterViewFocusHeight;
            var position = hostTarget.position + quarterViewOffset;
            var rotation = Quaternion.LookRotation(focus - position, Vector3.up);

            attachedCamera.orthographic = true;
            attachedCamera.orthographicSize = quarterViewOrthographicSize;
            // The host is the fixed visual anchor in quarter view. Smoothing here lets the
            // moving rat drift away from the intended screen center, so apply this mode
            // immediately before snapping the final output position to the render grid.
            MoveCamera(position, rotation, deltaTime, snap: true);
            RefreshQuarterViewOutputPixelSnap();
        }

        private void ApplyTopView(float deltaTime, bool snap)
        {
            if (hostTarget == null)
            {
                return;
            }

            var focus = hostTarget.position;
            var position = hostTarget.position + topViewOffset;
            var rotation = Quaternion.LookRotation(focus - position, Vector3.forward);

            attachedCamera.orthographic = true;
            attachedCamera.orthographicSize = topViewOrthographicSize;
            MoveCamera(position, rotation, deltaTime, snap);
        }

        private void ApplyVirusView(float deltaTime, bool snap)
        {
            if (virusTarget == null)
            {
                return;
            }

            var focus = virusTarget.position;
            var position = virusTarget.position + virusViewOffset;
            var rotation = Quaternion.LookRotation(focus - position, Vector3.up);

            attachedCamera.orthographic = true;
            attachedCamera.orthographicSize = virusViewOrthographicSize;
            MoveCamera(position, rotation, deltaTime, snap);
        }

        private void MoveCamera(Vector3 position, Quaternion rotation, float deltaTime, bool snap)
        {
            if (snap || followSharpness <= 0f)
            {
                transform.SetPositionAndRotation(position, rotation);
                return;
            }

            var t = 1f - Mathf.Exp(-followSharpness * deltaTime);
            transform.position = Vector3.Lerp(transform.position, position, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, t);
        }

        private void RefreshQuarterViewOutputPixelSnap()
        {
            if (!enableQuarterViewOutputPixelSnap || attachedCamera == null || !attachedCamera.orthographic)
            {
                return;
            }

            transform.position = PrototypeCameraOutputPixelSnapper.SnapPosition(
                transform.position,
                transform.right,
                transform.up,
                attachedCamera.orthographic,
                attachedCamera.orthographicSize,
                quarterViewOutputPixelHeight);
        }
    }

    /// <summary>
    /// Snaps an orthographic camera's output position on its screen plane. The forward-depth
    /// component is retained so camera focus, rotation, and gameplay-space transforms are untouched.
    /// </summary>
    public static class PrototypeCameraOutputPixelSnapper
    {
        public static bool TryGetWorldUnitsPerPixel(float orthographicSize, int internalPixelHeight, out float unitsPerPixel)
        {
            unitsPerPixel = 0f;
            if (float.IsNaN(orthographicSize)
                || float.IsInfinity(orthographicSize)
                || orthographicSize <= 0f
                || internalPixelHeight <= 0)
            {
                return false;
            }

            unitsPerPixel = 2f * orthographicSize / internalPixelHeight;
            return !float.IsNaN(unitsPerPixel)
                && !float.IsInfinity(unitsPerPixel)
                && unitsPerPixel > 0f;
        }

        public static Vector3 SnapPosition(
            Vector3 worldPosition,
            Vector3 cameraRight,
            Vector3 cameraUp,
            bool isOrthographic,
            float orthographicSize,
            int internalPixelHeight)
        {
            if (!isOrthographic
                || !TryGetWorldUnitsPerPixel(orthographicSize, internalPixelHeight, out var unitsPerPixel)
                || cameraRight.sqrMagnitude < 0.999f
                || cameraUp.sqrMagnitude < 0.999f)
            {
                return worldPosition;
            }

            var right = cameraRight.normalized;
            var up = cameraUp.normalized;
            var forward = Vector3.Cross(right, up);
            if (forward.sqrMagnitude < 0.999f)
            {
                return worldPosition;
            }

            forward.Normalize();
            var rightCoordinate = Mathf.Round(Vector3.Dot(worldPosition, right) / unitsPerPixel) * unitsPerPixel;
            var upCoordinate = Mathf.Round(Vector3.Dot(worldPosition, up) / unitsPerPixel) * unitsPerPixel;
            var forwardCoordinate = Vector3.Dot(worldPosition, forward);
            return right * rightCoordinate + up * upCoordinate + forward * forwardCoordinate;
        }
    }
}
