using System;
using UnityEngine;

public class AircraftLauncher : MonoBehaviour
{
   [SerializeField] private Rigidbody2D rb;
   [SerializeField] private FlightTracker tracker;
    
    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool isDragging = false;
    private bool hasLaunched = false;
    private Vector2 startPosition;
    
    [Header("Визуализация силы")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float maxLineLength = 3f;
    [SerializeField] private Color lineStartColor = Color.green;
    [SerializeField] private Color lineEndColor = Color.red;
    
    public event Action OnLaunch;

    void Start()
    {
        startPosition = transform.position;
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.5f;
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (!hasLaunched)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0))
            {
                startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                isDragging = true;
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                UpdateDragVisualization();
            }
            else if (Input.GetMouseButtonUp(0) && isDragging)
            {
                endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Launch();
                HideDragVisualization();
            }
#elif UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                
                if (touch.phase == TouchPhase.Began)
                {
                    startPoint = touchPos;
                    isDragging = true;
                }
                else if (touch.phase == TouchPhase.Moved && isDragging)
                {
                    endPoint = touchPos;
                    UpdateDragVisualization();
                }
                else if (touch.phase == TouchPhase.Ended && isDragging)
                {
                    endPoint = touchPos;
                    Launch();
                    HideDragVisualization();
                }
            }
#endif
        }
    }

    void Launch()
    {
        if (!rb) return;
        if (lineRenderer)
        {
            lineRenderer.enabled = false;
        }
        
        Vector2 direction = startPoint - endPoint;
        rb.bodyType = RigidbodyType2D.Dynamic;
        Vector2 clampedDirection = Vector2.ClampMagnitude(direction, 3);
        rb.AddForce(clampedDirection * UpgradeManager.Instance.GetLaunchForce(), ForceMode2D.Impulse);
        hasLaunched = true;
        isDragging = false;
        
        OnLaunch?.Invoke();
    }

    public void ResetToStart()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.rotation = Quaternion.identity;
        hasLaunched = false;
        isDragging = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
    }
    
    void UpdateDragVisualization()
    {
        if (lineRenderer == null) return;
    
        Vector2 direction = startPoint - endPoint;
        Vector2 clampedDirection = Vector2.ClampMagnitude(direction, maxLineLength);
        Vector2 endVisualPoint = (Vector2)transform.position + clampedDirection;
    
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endVisualPoint);
    
        // Изменение цвета в зависимости от силы
        float strength = clampedDirection.magnitude / maxLineLength;
        lineRenderer.startColor = Color.Lerp(lineStartColor, lineEndColor, strength);
        lineRenderer.endColor = Color.Lerp(lineStartColor, lineEndColor, strength);
    
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }
    }

    void HideDragVisualization()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }
}
