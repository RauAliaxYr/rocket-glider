using UnityEngine;

public class FlightTracker : MonoBehaviour
{
    
    public float distanceTravelled { get; private set; }
    public int coinsEarned { get; private set; }

    private Vector2 startPosition;
    
    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        distanceTravelled = transform.position.x - startPosition.x;
        distanceTravelled = Mathf.Max(0f, distanceTravelled);
    }

    public int TakeCoindByTravel()
    {
        return coinsEarned = Mathf.FloorToInt(distanceTravelled)/10;
    }
    
}
