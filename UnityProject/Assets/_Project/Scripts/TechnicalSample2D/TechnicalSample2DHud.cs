using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace LastHost.Prototype.TechnicalSample2D
{
    public sealed class TechnicalSample2DHud : MonoBehaviour
    {
        [SerializeField] private TechnicalSample2DTelemetry telemetry;
        [SerializeField] private Text titleText;
        [SerializeField] private Text specificationText;
        [SerializeField] private Text controlsText;
        [SerializeField] private Text runtimeStatusText;

        public void Configure(
            TechnicalSample2DTelemetry sourceTelemetry,
            Text title,
            Text specification,
            Text controls,
            Text runtimeStatus)
        {
            telemetry = sourceTelemetry;
            titleText = title;
            specificationText = specification;
            controlsText = controls;
            runtimeStatusText = runtimeStatus;
            ApplyStaticText();
        }

        public string FormatRuntimeStatus(TechnicalSample2DTelemetrySnapshot snapshot)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "Direction {0} | Root ({1:0.000}, {2:0.000}) | Moving {3}\n" +
                "Max step {4:0.0000} | Camera error ({5:0.00}, {6:0.00}) px | Sort {7}",
                snapshot.Direction,
                snapshot.RootPosition.x,
                snapshot.RootPosition.y,
                snapshot.IsMoving ? "YES" : "NO",
                snapshot.MaximumObservedStep,
                snapshot.CameraErrorPixels.x,
                snapshot.CameraErrorPixels.y,
                snapshot.SortingOrder);
        }

        private void Awake()
        {
            ApplyStaticText();
        }

        private void Update()
        {
            if (runtimeStatusText != null && telemetry != null)
            {
                runtimeStatusText.text = FormatRuntimeStatus(telemetry.Capture());
            }
        }

        private void ApplyStaticText()
        {
            if (titleText != null)
            {
                titleText.text = "2D TECHNICAL SAMPLE";
            }

            if (specificationText != null)
            {
                specificationText.text = "960×540 / Tile 64×32 / PPU 64";
            }

            if (controlsText != null)
            {
                controlsText.text = "WASD 이동";
            }
        }
    }
}
