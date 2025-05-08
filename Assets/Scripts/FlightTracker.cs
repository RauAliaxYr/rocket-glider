using UnityEngine;

public class FlightTracker : MonoBehaviour
{
    
    public float distanceTravelled { get; private set; }
    public int coinsEarned { get; private set; }

    private Vector2 startPosition;
    private bool levelAdvanced = false;
    private LevelManager levelManager;
    
    void Start()
    {
        startPosition = transform.position;
        levelAdvanced = false;
        levelManager = LevelManager.Instance;
    }

    void Update()
    {
        distanceTravelled = transform.position.x - startPosition.x;
        distanceTravelled = Mathf.Max(0f, distanceTravelled);
        if (!levelAdvanced && levelManager != null)
        {
            if (distanceTravelled >= LevelManager.Instance.CurrentLevel.requiredDistanceToPass)
            {
                levelAdvanced = true;
                LevelManager.Instance.TryAdvanceLevel(distanceTravelled);
            }
        }
    }

    public int TakeCoindByTravel()
    {
        return coinsEarned = Mathf.FloorToInt(distanceTravelled)/10;
    }
    
}
