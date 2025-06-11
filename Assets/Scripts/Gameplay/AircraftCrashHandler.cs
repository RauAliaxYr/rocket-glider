using UnityEngine;

public class AircraftCrashHandler : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private RewardBannerUI _rewardBannerUI;
    [SerializeField] private FlightTracker _tracker;
    [SerializeField] private AudioSource _audioSourceEngine;
    [SerializeField] private AircraftTapController _aircraftTapController;
    
    [Header("Settings")]
    [SerializeField] private string _crashTag = "Ground";
    [SerializeField] private float _crashDelay = 0.1f; // Защита от множественных срабатываний

    private Rigidbody2D _rigidbody;
    private bool _hasCrashed;
    private float _lastCrashTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        ValidateDependencies();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldProcessCrash(collision))
        {
            ProcessCrash();
        }
    }

    private bool ShouldProcessCrash(Collision2D collision)
    {
        return !_hasCrashed && 
               collision.gameObject.CompareTag(_crashTag) &&
               Time.time > _lastCrashTime + _crashDelay;
    }

    private void ProcessCrash()
    {
        _aircraftTapController.ResetTaps();
        _hasCrashed = true;
        _lastCrashTime = Time.time;
        EndFlight();
    }

    private void EndFlight()
    {
        DisablePhysics();
        StopEngineSound();
        ShowReward();
    }

    private void DisablePhysics()
    {
        if (_rigidbody)
        {
            _rigidbody.simulated = false;
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }

    private void StopEngineSound()
    {
        if (_audioSourceEngine && _audioSourceEngine.isPlaying)
        {
            _audioSourceEngine.Stop();
        }
    }

    private void ShowReward()
    {
        if (_rewardBannerUI && _tracker)
        {
            int reward = _tracker.TakeCoindByTravel();
            _rewardBannerUI.Show(reward);
        }
    }

    public void ResetCrashState()
    {
        _hasCrashed = false;
        
        if (_rigidbody)
        {
            _rigidbody.simulated = true;
        }
    }

    private void ValidateDependencies()
    {
        if (!_rewardBannerUI)
            Debug.LogWarning("RewardBannerUI reference is missing!", this);

        if (!_tracker)
            Debug.LogWarning("FlightTracker reference is missing!", this);

        if (!_audioSourceEngine)
            Debug.LogWarning("Engine AudioSource reference is missing!", this);
    }
}

