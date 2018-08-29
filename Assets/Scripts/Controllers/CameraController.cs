using UnityEngine;

namespace Common
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float defaultDistance = 10f;
        [SerializeField] private float maxDistance = 40f;
        [SerializeField] private float minDistance = 3f;
        [SerializeField] [Range(0.1f, 3f)] private float cameraSensitive = 1f;
        [SerializeField] [Range(1f, 20f)] private float scrollSpeed = 5f;
        [Tooltip("Coefficient for resolving the vertical camera lock problem.")]
        [SerializeField] private float verticalSlopeThreshold = 0.01f;

        private Camera _camera;

        private Vector3 directionOnCamera;
        private float cameraDistance;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Start()
        {
            cameraDistance = defaultDistance;
            directionOnCamera = (target.position - transform.position).normalized;
        }

        private void Update()
        {
            UpdateCameraDistance();

            if (Input.GetMouseButton(1))
            {
                UpdateDirectionOnCamera();
            }

            UpdateCameraPosition();
        }

        private void UpdateDirectionOnCamera()
        {
            var horizontal = Input.GetAxis("HorizontalMouse") * cameraSensitive;
            var vertical = Input.GetAxis("VerticalMouse") * cameraSensitive;

            // Rotate around Y.
            directionOnCamera = Quaternion.AngleAxis(horizontal, Vector3.up) * directionOnCamera;

            // Rotate around the orthogonal to current and vertical vector.
            var newVerticalDirection = Quaternion.AngleAxis(vertical, Vector3.Cross(Vector3.down, directionOnCamera).normalized) * directionOnCamera;

            // Vertical camera threshold validation.
            if (Mathf.Abs(Vector3.Dot(newVerticalDirection, Vector3.up)) < 1f - verticalSlopeThreshold)
            {
                directionOnCamera = newVerticalDirection;
            }
        }

        private void UpdateCameraDistance()
        {
            var scrollWheel = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

            cameraDistance = Mathf.Clamp(cameraDistance + scrollWheel, minDistance, maxDistance);
        }

        private void UpdateCameraPosition()
        {
            transform.position = directionOnCamera * cameraDistance;
            _camera.transform.LookAt(target);
        }
    }
}