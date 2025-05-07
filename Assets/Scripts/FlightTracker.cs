using UnityEngine;

public class FlightTracker : MonoBehaviour
{
    [SerializeField] private RewardBannerUI rewardBannerUI;
    public float distanceTravelled { get; private set; }
    public int coinsEarned { get; private set; }

    private Vector2 startPosition;
    private bool hasCrashed = false;
    private Rigidbody2D rb;
    
    

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        distanceTravelled = transform.position.x - startPosition.x;
        distanceTravelled = Mathf.Max(0f, distanceTravelled);

        coinsEarned = Mathf.FloorToInt(distanceTravelled)/10;
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
        rewardBannerUI.Show(coinsEarned);
    }
}
