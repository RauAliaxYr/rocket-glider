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
    
    public event Action OnLaunch;

    void Start()
    {
        startPosition = transform.position;
        rb.bodyType = RigidbodyType2D.Kinematic;
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
            else if (Input.GetMouseButtonUp(0) && isDragging)
            {
                endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Launch();
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
                else if (touch.phase == TouchPhase.Ended && isDragging)
                {
                    endPoint = touchPos;
                    Launch();
                }
            }
#endif
        }
    }

    void Launch()
    {
        if (!rb) return;
        
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
}
