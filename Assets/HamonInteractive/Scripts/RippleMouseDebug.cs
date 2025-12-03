using UnityEngine;

namespace HamonInteractive
{
    /// <summary>
    /// Debug component: applies mouse input as a transient force into RippleSimulation.
    /// </summary>
    [DisallowMultipleComponent]
    public class RippleMouseDebug : MonoBehaviour
    {
        [SerializeField] private RippleSimulation simulation;
        [SerializeField] private Camera inputCamera;
        [SerializeField, Range(0.001f, 0.25f)] private float brushRadius = 0.03f;
        [SerializeField] private float brushStrength = 2.0f;
        [SerializeField, Range(0.1f, 8f)] private float brushFalloff = 2.0f;
        [SerializeField] private bool clearEachFrame = true;

        private void Reset()
        {
            if (simulation == null)
            {
                simulation = GetComponent<RippleSimulation>();
            }
        }

        private void OnValidate()
        {
            brushRadius = Mathf.Clamp(brushRadius, 0.001f, 0.25f);
        }

        private void Update()
        {
            if (!Application.isPlaying) return;
            if (simulation == null) return;

            if (clearEachFrame)
            {
                simulation.ClearForceTexture();
            }

            bool isDown = Input.GetMouseButton(0) || Input.GetMouseButton(1);
            if (!isDown) return;

            Camera cam = inputCamera != null ? inputCamera : Camera.main;
            Vector3 mouse = Input.mousePosition;
            Vector2 uv = cam != null
                ? (Vector2)cam.ScreenToViewportPoint(mouse)
                : new Vector2(mouse.x / Screen.width, mouse.y / Screen.height);

            if (uv.x < 0f || uv.x > 1f || uv.y < 0f || uv.y > 1f) return;

            float sign = Input.GetMouseButton(0) ? 1f : -1f;
            simulation.AddForceBrush(uv, brushRadius, brushStrength * sign, brushFalloff);
        }
    }
}
