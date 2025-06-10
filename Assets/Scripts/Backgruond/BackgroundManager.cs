
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundManager : MonoBehaviour, ICloudPool
{
   [Header("Settings")]
    [SerializeField] private float _cloudSpawnRate = 2f;
    [SerializeField] private Vector2 _cloudHeightRange = new Vector2(-2f, 2f);
    [SerializeField] private Vector2 _cloudSpeedRange = new Vector2(1f, 3f);
    [SerializeField] private Vector2 _cloudScaleRange = new Vector2(0.5f, 1.5f);

    [Header("References")]
    [SerializeField] private SpriteRenderer _backgroundRenderer;
    [SerializeField] private CloudMove _cloudPrefab;
    [SerializeField] private Transform _cloudContainer;
    [SerializeField] private Sprite[] _cloudSprites;

    private Camera _mainCamera;
    private Material _backgroundMaterial;
    private float _scrollOffset;
    private float _spawnTimer;
    private Queue<CloudMove> _cloudPool = new Queue<CloudMove>();
    private List<CloudMove> _activeClouds = new List<CloudMove>();

    private const float CLOUD_SPAWN_X = 1.5f;
    private const float CLOUD_DESPAWN_X = -0.2f;
    private const int MAX_CLOUDS = 8;

    private void Awake() 
    {
        _mainCamera = Camera.main;
        _backgroundMaterial = _backgroundRenderer.material;
    }

    private void Start() 
    {
        PrewarmPool(4); // Создаём 4 облака заранее
    }

    private void Update() 
    {
        ScrollBackground();
        TrySpawnCloud();
    }

    // --- Фон ---
    private void ScrollBackground() 
    {
        _scrollOffset += Time.deltaTime * LevelManager.Instance.CurrentLevel.backgroundScrollSpeed;
        _backgroundMaterial.mainTextureOffset = new Vector2(_scrollOffset, 0);
    }

    //  Пул облаков 
    private void PrewarmPool(int count) 
    {
        for (int i = 0; i < count; i++) 
        {
            CreateNewCloud();
        }
    }

    private void TrySpawnCloud() 
    {
        if (_activeClouds.Count >= MAX_CLOUDS) return;
        
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0) 
        {
            SpawnCloud();
            _spawnTimer = _cloudSpawnRate;
        }
    }

    private void SpawnCloud() 
    {
        CloudMove cloud = GetCloudFromPool();
        
        // Настройки облака
        Sprite randomSprite = _cloudSprites[Random.Range(0, _cloudSprites.Length)];
        float speed = Random.Range(_cloudSpeedRange.x, _cloudSpeedRange.y);
        Vector3 position = new Vector3(
            _mainCamera.ViewportToWorldPoint(new Vector3(CLOUD_SPAWN_X, 0)).x,
            Random.Range(_cloudHeightRange.x, _cloudHeightRange.y),
            0
        );
        Vector3 scale = Vector3.one * Random.Range(_cloudScaleRange.x, _cloudScaleRange.y);

        // Инициализация
        cloud.Initialize(
            pool: this,
            speed: speed,
            despawnX: _mainCamera.ViewportToWorldPoint(new Vector3(CLOUD_DESPAWN_X, 0)).x
        );
        cloud.ResetCloud(position);
        cloud.transform.localScale = scale;
        cloud.GetComponent<SpriteRenderer>().sprite = randomSprite;
        
        _activeClouds.Add(cloud);
    }

    private CloudMove GetCloudFromPool() 
    {
        if (_cloudPool.Count == 0)
            CreateNewCloud();

        CloudMove cloud = _cloudPool.Dequeue();
        cloud.gameObject.SetActive(true);
        return cloud;
    }

    private void CreateNewCloud() 
    {
        CloudMove cloud = Instantiate(_cloudPrefab, _cloudContainer);
        cloud.gameObject.SetActive(false);
        _cloudPool.Enqueue(cloud);
    }
    
    public void ReturnToPool(CloudMove cloud) 
    {
        if (cloud == null) return;
        
        cloud.gameObject.SetActive(false);
        _activeClouds.Remove(cloud);
        _cloudPool.Enqueue(cloud);
    }

    public float GetDespawnX() 
    {
        return _mainCamera.ViewportToWorldPoint(new Vector3(CLOUD_DESPAWN_X, 0)).x;
    }
}
