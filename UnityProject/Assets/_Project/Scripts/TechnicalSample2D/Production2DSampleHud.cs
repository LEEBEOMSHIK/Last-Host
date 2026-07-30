using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace LastHost.Prototype.TechnicalSample2D
{
    public sealed class Production2DSampleHud : MonoBehaviour
    {
        [SerializeField] private TechnicalSample2DTelemetry telemetry;
        [SerializeField] private Text runtimeStatusText;

        public void Configure(
            TechnicalSample2DTelemetry sourceTelemetry,
            Text runtimeStatus)
        {
            telemetry = sourceTelemetry;
            runtimeStatusText = runtimeStatus;
            Refresh();
        }

        public void Refresh()
        {
            if (runtimeStatusText == null || telemetry == null)
            {
                return;
            }

            var snapshot = telemetry.Capture();
            runtimeStatusText.text = string.Format(
                CultureInfo.InvariantCulture,
                "WASD  |  SIDE 3F ONLY  |  PPU 128 CANDIDATE\n" +
                "Root ({0:0.00}, {1:0.00})  Camera ({2:0.00}, {3:0.00}) px  Sort {4}",
                snapshot.RootPosition.x,
                snapshot.RootPosition.y,
                snapshot.CameraErrorPixels.x,
                snapshot.CameraErrorPixels.y,
                snapshot.SortingOrder);
        }

        private void Update()
        {
            Refresh();
        }
    }
}
