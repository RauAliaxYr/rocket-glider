using UnityEngine;

public class AircraftTapController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource audioSourceTap;
    
    private int tapsLeft;
    private bool hasLaunched = false;

    void Update()
    {
        if (hasLaunched)
        {
            HandleInAirTap();
        }
    }

    public void SetHasLaunched(bool launched)
    {
        hasLaunched = launched;
        tapsLeft = UpgradeManager.Instance.GetMaxTaps();
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
        if (!rb) return;
        
        audioSourceTap.Play();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * UpgradeManager.Instance.GetTapImpulse(), ForceMode2D.Impulse);
        tapsLeft--;
    }

    public void ResetTaps()
    {
        tapsLeft = 0;
    }
}
