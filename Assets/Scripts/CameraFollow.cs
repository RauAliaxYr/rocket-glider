using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D targetRb;

    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("Zoom Settings")]
    public float minSize = 5f;
    public float maxSize = 10f;
    public float maxSpeed = 15f;
    public float zoomSmoothness = 5f;

    private Camera cam;

    void Awake()
    {
        targetRb = target.GetComponent<Rigidbody2D>();
        cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        if (target == null || targetRb == null) return;

        // Следование за целью
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothed;

        // Динамический зум в зависимости от скорости
        float speed = Mathf.Abs(targetRb.linearVelocity.x);
        float t = Mathf.Clamp01(speed / maxSpeed);
        float targetSize = Mathf.Lerp(minSize, maxSize, t);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSmoothness);
    }
}
