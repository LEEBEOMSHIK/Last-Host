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
        QuarterView
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
        public Vector3 quarterViewOffset = new Vector3(6.4f, 7.4f, -6.4f);
        public float quarterViewFocusHeight = 0.3f;
        public float quarterViewOrthographicSize = 5.2f;

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
            CurrentHostMode = CurrentHostMode == PrototypeCameraMode.ThirdPerson
                ? PrototypeCameraMode.QuarterView
                : PrototypeCameraMode.ThirdPerson;
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

            if (CurrentHostMode == PrototypeCameraMode.QuarterView)
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
    }
}
