using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class AircraftTapController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private AudioSource _tapSound;

    [Header("Tap Settings")]
    [SerializeField] private float _yVelocityReset = 0f; // Обнуление Y-скорости при тапе

    private int _remainingTaps;
    private bool _isActive;
    private UpgradeManager _upgradeManager;

    private void Awake()
    {
        _upgradeManager = UpgradeManager.Instance;
        ValidateComponents();
    }

    private void Update()
    {
        if (!_isActive) return;

        if (IsTapInputReceived() && CanTap())
        {
            ApplyTapImpulse();
        }
    }

    public void SetHasLaunched(bool isLaunched)
    {
        _isActive = isLaunched;
        _remainingTaps = _upgradeManager.GetMaxTaps();
    }

    public void ResetTaps()
    {
        _remainingTaps = 0;
    }

    private bool CanTap()
    {
        return _remainingTaps > 0;
    }

    private bool IsTapInputReceived()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButtonDown(0);
#elif UNITY_ANDROID || UNITY_IOS
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
    }

    private void ApplyTapImpulse()
    {
        if (!_rigidbody) return;

        // 1. Сброс вертикальной скорости
        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _yVelocityReset);
        
        // 2. Применение импульса
        float impulseForce = _upgradeManager.GetTapImpulse();
        _rigidbody.AddForce(Vector2.up * impulseForce, ForceMode2D.Impulse);
        
        // 3. Воспроизведение звука
        PlayTapSound();
        
        // 4. Уменьшение счетчика
        _remainingTaps--;
    }

    private void PlayTapSound()
    {
        if (_tapSound && _tapSound.clip)
        {
            _tapSound.Play();
        }
    }

    private void ValidateComponents()
    {
        if (!_rigidbody)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            Debug.LogWarning("Rigidbody2D not assigned, using component from GameObject", this);
        }

        if (!_tapSound)
        {
            Debug.LogWarning("Tap sound AudioSource not assigned!", this);
        }

        if (!_upgradeManager)
        {
            Debug.LogError("UpgradeManager instance not found!", this);
            enabled = false;
        }
    }
}
