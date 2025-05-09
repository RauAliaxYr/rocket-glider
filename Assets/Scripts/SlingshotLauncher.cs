using UnityEngine;

public class SlingshotLauncher : MonoBehaviour
{
    [SerializeField] private RewardBannerUI rewardBannerUI;
    [SerializeField] private FlightTracker tracker;
    
    [Header("Полётные тапы")]
    [SerializeField] private float speedCorrectionForce ;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Rigidbody2D rb;

    private bool isDragging = false;
    private bool hasLaunched = false;
    private int tapsLeft;
    private Vector2 startPosition;
    private bool hasCrashed = false;
    

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Пока не запустили — отключаем физику
    }

    void Update()
    {
        if (hasLaunched)
        {
            MaintainTargetSpeed();
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
        Vector2 clampedDirection = Vector2.ClampMagnitude(direction, 3);
        rb.AddForce(clampedDirection * UpgradeManager.Instance.GetLaunchForce(), ForceMode2D.Impulse);
        hasLaunched = true;
        tapsLeft = UpgradeManager.Instance.GetMaxTaps();
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
        rb.AddForce(Vector2.up * UpgradeManager.Instance.GetTapImpulse(), ForceMode2D.Impulse);
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
    public void ResetToStart()
    {
        transform.position = startPosition; // сохранить в Start()
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.rotation = Quaternion.identity;
        hasLaunched = false;
        isDragging = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        hasCrashed = false;

        // (опционально) сбрасываем энергию тапов
        tapsLeft = 0;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCrashed && collision.gameObject.CompareTag("Ground"))
        {
            hasCrashed = true;
            EndFlight();
        }
    }
    void EndFlight()
    {
        rb.simulated = false; // Отключаем физику
        rewardBannerUI.Show(tracker.TakeCoindByTravel());
    }
    void MaintainTargetSpeed()
    {
        float currentSpeedX = rb.linearVelocity.x;
        float speedDifference = UpgradeManager.Instance.GetTargetSpeed() - currentSpeedX;

        // Плавное приближение к целевой скорости по X
        Vector2 force = new Vector2(speedDifference * speedCorrectionForce, 0f);
        rb.AddForce(force, ForceMode2D.Force);
    }
    
}
