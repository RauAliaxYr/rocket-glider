using UnityEngine;

public class SlingshotLauncher : MonoBehaviour
{
    [Header("Запуск")]
    public float launchForceMultiplier = 10f;

    [Header("Полётные тапы")]
    public int maxTaps = 3;
    public float tapForce = 5f;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Rigidbody2D rb;

    private bool isDragging = false;
    private bool hasLaunched = false;
    private int tapsLeft;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Пока не запустили — отключаем физику
    }

    void Update()
    {
        if (hasLaunched)
        {
            RotateTowardsVelocity();
        }
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
        else
        {
            HandleInAirTap();
        }
    }

    void Launch()
    {
        Vector2 direction = startPoint - endPoint;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction * launchForceMultiplier, ForceMode2D.Impulse);
        hasLaunched = true;
        tapsLeft = maxTaps;
        isDragging = false;
    }
    void HandleInAirTap()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0) && tapsLeft > 0)
        {
            TapImpulse();
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && tapsLeft > 0)
        {
            TapImpulse();
        }
#endif
    }

    void TapImpulse()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Обнуляем вертикальную скорость
        rb.AddForce(Vector2.up * tapForce, ForceMode2D.Impulse);
        tapsLeft--;
    }
    void RotateTowardsVelocity()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.sqrMagnitude > 0.01f) // избегаем деления на ноль
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }
}
