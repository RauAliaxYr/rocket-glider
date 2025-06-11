using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private CameraSettings _settings;
    
    private Rigidbody2D _targetRb;
    private Camera _camera;

    private void Awake() 
    {
        Initialize();
    }

    private void Initialize() 
    {
        if (_target == null) 
        {
            Debug.LogError("CameraFollow: Target is not assigned!");
            enabled = false;
            return;
        }
        _targetRb = _target.GetComponent<Rigidbody2D>();
        _camera = GetComponent<Camera>();
        if (_targetRb == null) 
        {
            Debug.LogWarning("CameraFollow: Target has no Rigidbody2D. Zoom will not work.");
        }
    }

    private void FixedUpdate() 
    {
        if (!IsValid()) return;
        FollowTarget();
        if (_targetRb) 
        {
            AdjustZoom();
        }
    }

    private bool IsValid() => _target && _camera;

    private void FollowTarget() 
    {
        Vector3 desiredPosition = _target.position + _settings.offset;
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position, 
            desiredPosition, 
            _settings.smoothSpeed * Time.deltaTime
        );
        transform.position = smoothedPosition;
    }

    private void AdjustZoom() 
    {
        float speed = Mathf.Abs(_targetRb.linearVelocity.x);
        float targetSize = CalculateTargetSize(speed);
        
        _camera.orthographicSize = Mathf.Lerp(
            _camera.orthographicSize, 
            targetSize, 
            Time.deltaTime * _settings.zoomSmoothness
        );
    }

    private float CalculateTargetSize(float speed) 
    {
        float t = Mathf.Clamp01(speed / _settings.maxSpeed);
        return Mathf.Lerp(_settings.minSize, _settings.maxSize, t);
    }
}

[System.Serializable]
public class CameraSettings 
{
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float minSize = 5f;
    public float maxSize = 10f;
    public float maxSpeed = 15f;
    public float zoomSmoothness = 5f;
}
