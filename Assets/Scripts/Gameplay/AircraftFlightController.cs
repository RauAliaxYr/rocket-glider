using UnityEngine;

public class AircraftFlightController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speedCorrectionForce;
    
    void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            MaintainTargetSpeed();
            RotateTowardsVelocity();
        }
    }

    void RotateTowardsVelocity()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    void MaintainTargetSpeed()
    {
        float currentSpeedX = rb.linearVelocity.x;
        float speedDifference = UpgradeManager.Instance.GetTargetSpeed() - currentSpeedX;

        Vector2 force = new Vector2(speedDifference * speedCorrectionForce, 0f);
        rb.AddForce(force, ForceMode2D.Force);
    }
}

