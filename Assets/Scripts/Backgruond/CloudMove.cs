using UnityEngine;

public class CloudMove : MonoBehaviour
{
    private float speed;
    private BackgroundManager manager;

    public void Initialize(BackgroundManager manager, float speed)
    {
        this.manager = manager;
        this.speed = speed;
    }

    void Update()
    {
        transform.Translate(Vector3.left * (speed * Time.deltaTime));
        
        if (IsOutOfScreen())
        {
            manager.ReturnCloudToPool(this);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    bool IsOutOfScreen()
    {
        return transform.position.x < manager.GetDespawnX();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void ResetCloud(Vector3 position)
    {
        transform.position = position;
    }
}

