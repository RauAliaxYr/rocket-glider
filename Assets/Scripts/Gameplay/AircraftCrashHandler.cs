using UnityEngine;

public class AircraftCrashHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private RewardBannerUI rewardBannerUI;
    [SerializeField] private FlightTracker tracker;
    [SerializeField] private AudioSource audioSourceEngine;
    
    private bool hasCrashed = false;

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
        rb.simulated = false;
        audioSourceEngine.Stop();
        rewardBannerUI.Show(tracker.TakeCoindByTravel());
    }

    public void ResetCrashState()
    {
        hasCrashed = false;
    }
}
