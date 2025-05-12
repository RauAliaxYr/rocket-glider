
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundManager : MonoBehaviour
{
   [Header("References")]
    public SpriteRenderer backgroundRenderer;
    public CloudMove cloudPrefab; // Префаб облака
    public Transform cloudContainer;

    // Настройки из LevelData
    private float cloudSpawnRate;
    private Vector2 cloudHeightRange;
    private Vector2 cloudSpeedRange;
    private Vector2 cloudScaleRange;
    private Sprite[] cloudSprites;

    public float DespawnXPosition { get; private set; }

    private Queue<CloudMove> cloudPool ;
    private List<CloudMove> activeClouds ;
    private float spawnTimer;
    private float scrollOffset;

    void Start()
    {
        activeClouds = new List<CloudMove>();
        cloudPool = new Queue<CloudMove>();
        LevelData levelData = LevelManager.Instance.CurrentLevel;
        InitializeFromLevelData(levelData);
        CalculateDespawnPosition();
    }

    void InitializeFromLevelData(LevelData data)
    {
        // Фон
        backgroundRenderer.sprite = data.backgroundSprite;
        Camera.main.backgroundColor = data.backgroundColor;
        
        // Облака
        cloudSpawnRate = data.cloudSpawnRate;
        cloudHeightRange = data.cloudHeightRange;
        cloudSpeedRange = data.cloudSpeedRange;
        cloudScaleRange = data.cloudScaleRange;
        cloudSprites = data.cloudSprites;
    }

    void CalculateDespawnPosition()
    {
        DespawnXPosition = Camera.main.ViewportToWorldPoint(new Vector3(-0.2f, 0)).x;
    }

    void Update()
    {
        ScrollBackground();
        TrySpawnCloud();
    }

    void ScrollBackground()
    {
        scrollOffset += Time.deltaTime * LevelManager.Instance.CurrentLevel.backgroundScrollSpeed;
        backgroundRenderer.material.mainTextureOffset = new Vector2(scrollOffset, 0);
    }

    void TrySpawnCloud()
    {
        if (activeClouds.Count >= 6) return;
        
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnCloud();
            spawnTimer = cloudSpawnRate;
        }
    }

    void SpawnCloud()
    {
        CloudMove cloud = GetCloudFromPool();
        
        // Настройки из LevelData
        Sprite randomSprite = cloudSprites[Random.Range(0, cloudSprites.Length)];
        float speed = Random.Range(cloudSpeedRange.x, cloudSpeedRange.y);
        Vector3 position = new Vector3(
            Camera.main.ViewportToWorldPoint(new Vector3(1.9f, 0)).x,
            Random.Range(cloudHeightRange.x, cloudHeightRange.y),
            0
        );
        Vector3 scale = Vector3.one * Random.Range(cloudScaleRange.x, cloudScaleRange.y);

        cloud.Initialize( this,speed );
        cloud.ResetCloud(position);
        cloud.gameObject.SetActive(true);
        activeClouds.Add(cloud);
    }

    CloudMove GetCloudFromPool()
    {
        if (cloudPool.Count == 0)
            CreateNewCloudInPool();

        return cloudPool.Dequeue();
    }

    void CreateNewCloudInPool()
    {
        CloudMove newCloud = Instantiate(cloudPrefab, cloudContainer);
        newCloud.gameObject.SetActive(false);
        cloudPool.Enqueue(newCloud);
    }

    public void ReturnCloudToPool(CloudMove cloud)
    {
        cloud.gameObject.SetActive(false);
        activeClouds.Remove(cloud);
        cloudPool.Enqueue(cloud);
    }
    public float GetSpawnX() => Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0)).x;
    public float GetDespawnX() => Camera.main.ViewportToWorldPoint(new Vector3(-0.2f, 0)).x;
}
