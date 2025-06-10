
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundManager : MonoBehaviour
{
   [Header("References")]
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private CloudMove cloudPrefab; // Префаб облака
    [SerializeField] private Transform cloudContainer;

    // Настройки из LevelData
    private float cloudSpawnRate;
    private Vector2 cloudHeightRange;
    private Vector2 cloudSpeedRange;
    private Vector2 cloudScaleRange;
    private Sprite[] cloudSprites;

    [SerializeField] private int cloudsLimit = 6;

    public float DespawnXPosition { get; private set; }

    private Queue<CloudMove> cloudPool ;
    private List<CloudMove> activeClouds ;
    private float spawnTimer;
    private float scrollOffset;
    
    private Camera mainCamera;
    
    private Material backgroundMaterial;

    void Start()
    {
        backgroundMaterial = backgroundRenderer.material;
        mainCamera = Camera.main;
        activeClouds = new List<CloudMove>();
        cloudPool = new Queue<CloudMove>();
        LevelData levelData = LevelManager.Instance.CurrentLevel;
        CalculateDespawnPosition();
        InitializeFromLevelData(levelData);
    }

    void InitializeFromLevelData(LevelData data)
    {
        // Фон
        backgroundRenderer.sprite = data.backgroundSprite;
        mainCamera.backgroundColor = data.backgroundColor;
        
        // Облака
        cloudSpawnRate = data.cloudSpawnRate;
        cloudHeightRange = data.cloudHeightRange;
        cloudSpeedRange = data.cloudSpeedRange;
        cloudScaleRange = data.cloudScaleRange;
        cloudSprites = data.cloudSprites;
    }

    void CalculateDespawnPosition()
    {
        DespawnXPosition = mainCamera.ViewportToWorldPoint(new Vector3(-0.2f, 0)).x;
    }

    void Update()
    {
        ScrollBackground();
        TrySpawnCloud();
    }

    void ScrollBackground()
    {
        scrollOffset += Time.deltaTime * LevelManager.Instance.CurrentLevel.backgroundScrollSpeed;
        backgroundMaterial.mainTextureOffset = new Vector2(scrollOffset, 0);
    }

    void TrySpawnCloud()
    {
        if (activeClouds.Count >= cloudsLimit) return;
        
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
        if (!cloud) return;
        cloud.gameObject.SetActive(false);
        activeClouds.Remove(cloud);
        cloudPool.Enqueue(cloud);
    }
    public float GetSpawnX() => mainCamera.ViewportToWorldPoint(new Vector3(1.1f, 0)).x;
    public float GetDespawnX() => mainCamera.ViewportToWorldPoint(new Vector3(-0.2f, 0)).x;
}
