using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AircraftLauncher : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private FlightTracker _tracker;
    [SerializeField] private LineRenderer _lineRenderer;

    [Header("Launch Settings")]
    [SerializeField] private float _maxDragLength = 3f;
    

    [Header("Line Colors")]
    [SerializeField] private Color _lineStartColor = Color.green;
    [SerializeField] private Color _lineEndColor = Color.red;
    [SerializeField] private float _lineStartWidth = 0.1f;
    [SerializeField] private float _lineEndWidth = 0.5f;

    private Vector2 _startPosition;
    private bool _isDragging = false;
    private bool _hasLaunched = false;
    private Camera _mainCamera;

    public event Action OnLaunch;

    private void Awake()
    {
        _startPosition = transform.position;
        InitializeComponents();
        SetupLineRenderer();
    }

    private void Start()
    {
        ResetToStart();
    }

    private void Update()
    {
        if (_hasLaunched) return;

        HandleInput();
    }

    #region Initialization
    private void InitializeComponents()
    {
        _mainCamera = Camera.main;
        
        if (!_rb) 
            _rb = GetComponent<Rigidbody2D>();

        if (!_mainCamera)
            Debug.LogError("Main camera not found!", this);
    }

    private void SetupLineRenderer()
    {
        if (_lineRenderer)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.startWidth = _lineStartWidth;
            _lineRenderer.endWidth = _lineEndWidth;
            _lineRenderer.enabled = false;
        }
    }
    #endregion

    #region Input Handling
    private void HandleInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag(GetWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            ContinueDrag(GetWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            EndDrag(GetWorldPoint(Input.mousePosition));
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        Vector2 touchPos = GetWorldPoint(touch.position);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                StartDrag(touchPos);
                break;
            case TouchPhase.Moved:
                ContinueDrag(touchPos);
                break;
            case TouchPhase.Ended:
                EndDrag(touchPos);
                break;
        }
    }

    private Vector2 GetWorldPoint(Vector2 screenPoint)
    {
        return _mainCamera.ScreenToWorldPoint(screenPoint);
    }
    #endregion

    #region Drag Logic
    private void StartDrag(Vector2 startPoint)
    {
        _isDragging = true;
        UpdateDragVisualization(startPoint, startPoint);
    }

    private void ContinueDrag(Vector2 currentPoint)
    {
        if (!_isDragging) return;
        UpdateDragVisualization(_startPosition, currentPoint);
    }

    private void EndDrag(Vector2 endPoint)
    {
        if (!_isDragging) return;
        
        Launch(_startPosition, endPoint);
        HideDragVisualization();
        _isDragging = false;
    }
    #endregion

    #region Launch Logic
    private void Launch(Vector2 startPoint, Vector2 endPoint)
    {
        if (!_rb) return;

        Vector2 direction = CalculateLaunchDirection(startPoint, endPoint);
        float force = UpgradeManager.Instance.GetLaunchForce();

        ApplyLaunchPhysics(direction, force);
        _hasLaunched = true;

        OnLaunch?.Invoke();
    }

    private Vector2 CalculateLaunchDirection(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 direction = startPoint - endPoint;
        return Vector2.ClampMagnitude(direction, _maxDragLength);
    }

    private void ApplyLaunchPhysics(Vector2 direction, float force)
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
    #endregion

    #region Visualization
    private void UpdateDragVisualization(Vector2 startPoint, Vector2 endPoint)
    {
        if (!_lineRenderer) return;

        Vector2 direction = CalculateLaunchDirection(startPoint, endPoint);
        Vector2 endVisualPoint = (Vector2)transform.position + direction;

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, endVisualPoint);

        UpdateLineColor(direction.magnitude / _maxDragLength);
        _lineRenderer.enabled = true;
    }

    private void UpdateLineColor(float strength)
    {
        _lineRenderer.startColor = Color.Lerp(_lineStartColor, _lineEndColor, strength);
        _lineRenderer.endColor = Color.Lerp(_lineStartColor, _lineEndColor, strength);
    }

    private void HideDragVisualization()
    {
        if (_lineRenderer)
        {
            _lineRenderer.enabled = false;
        }
    }
    #endregion

    #region Reset
    public void ResetToStart()
    {
        if (_rb)
        {
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            _rb.simulated = true;
        }

        transform.position = _startPosition;
        transform.rotation = Quaternion.identity;
        _hasLaunched = false;
        _isDragging = false;
    }
    #endregion
}
