using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Basic Info")]
    public string levelName;
    public int requiredDistanceToPass;
    public Color backgroundColor;

    [Header("Ground Tiles")]
    public TileBase topGroundTile;
    public TileBase groundFillTile;

    [Header("Legacy Background (Tile-based)")]
    [Tooltip("Используется, если backgroundSprite не задан")]
    public TileBase backgroundTile; // Для обратной совместимости

    [Header("New Background System")]
    public Sprite backgroundSprite; // Большое изображение фона
    [Range(0f, 1f)]
    public float backgroundScrollSpeed ; // Скорость параллакс-скроллинга

    [Header("Obstacles")]
    public GameObject[] obstaclePrefabs;
    
    [Header("Cloud Settings")]
    public bool useCloudSprites = true; // Если false - использует cloudTile
    public Sprite[] cloudSprites; // Варианты спрайтов облаков
    public TileBase cloudTile; // Тайл облака (альтернатива спрайтам)
    public float cloudSpawnRate;
    public Vector2 cloudHeightRange;
    public Vector2 cloudSpeedRange;
    public Vector2 cloudScaleRange;

    [Header("Background Layers (Параллакс)")]
    public ParallaxLayer[] parallaxLayers;

    [System.Serializable]
    public class ParallaxLayer
    {
        public Sprite layerSprite;
        [Range(0f, 1f)]
        public float scrollSpeed;
        public Color tintColor = Color.white;
        public Vector2 positionOffset;
    }
}
