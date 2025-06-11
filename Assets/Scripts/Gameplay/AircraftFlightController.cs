using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AircraftFlightController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speedCorrectionForce = 1f;
    [SerializeField] private float _rotationSmoothness = 5f;
    [SerializeField] private float _minVelocityThreshold = 0.1f;
    
    private Rigidbody2D _rigidbody;
    private UpgradeManager _upgradeManager;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _upgradeManager = UpgradeManager.Instance;
        
        if (!_upgradeManager)
        {
            Debug.LogError("UpgradeManager instance is missing!");
            enabled = false;
        }
    }
    private void Update()
    {
        if (!IsActive()) return;
        
        MaintainTargetSpeed();
        RotateTowardsVelocity();
    }
    private bool IsActive()
    {
        return _rigidbody && 
               _rigidbody.bodyType == RigidbodyType2D.Dynamic;
    }

    private void RotateTowardsVelocity()
    {
        Vector2 velocity = _rigidbody.linearVelocity;

        if (velocity.sqrMagnitude > _minVelocityThreshold)
        {
            float targetAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                _rotationSmoothness * Time.deltaTime
            );
        }
    }
    private void MaintainTargetSpeed()
    {
        float currentSpeedX = _rigidbody.linearVelocity.x;
        float targetSpeed = _upgradeManager.GetTargetSpeed();
        float speedDifference = targetSpeed - currentSpeedX;

        Vector2 correctionForce = new Vector2(
            speedDifference * _speedCorrectionForce, 
            0f
        );
        
        _rigidbody.AddForce(correctionForce, ForceMode2D.Force);
    }
    
}

