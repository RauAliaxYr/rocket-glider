using UnityEngine;

public class SlingshotLauncher : MonoBehaviour
{
    public float launchForceMultiplier = 10f;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Rigidbody2D rb;
    private bool isDragging = false;
    private bool hasLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Пока не запустили — отключаем физику
    }

    void Update()
    {
        if (hasLaunched) return;

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

    void Launch()
    {
        Vector2 direction = startPoint - endPoint;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction * launchForceMultiplier, ForceMode2D.Impulse);
        hasLaunched = true;
    }
}
