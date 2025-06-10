using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTileGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform target;
    
    [Header("Settings")]
    [SerializeField] private int viewDistance = 30;
    [SerializeField] private int groundHeightY = -3;
    [SerializeField] private int dirtDepth = 10;
    [SerializeField] private int cleanupDistanceBehind = 20;

    private TileBase _grassTile;
    private TileBase _dirtTile;
    
    private int _generatedMinX;
    private int _generatedMaxX;
    private int _targetX;
    
    private const int PregenOffset = 4; 

    void Start()
    {
        _generatedMinX = int.MaxValue;
        _generatedMaxX = int.MinValue;
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnLevelAdvanced += HandleLevelChange;
        }
        _grassTile = LevelManager.Instance.CurrentLevel.topGroundTile;
        _dirtTile = LevelManager.Instance.CurrentLevel.groundFillTile;
    }

    void Update()
    {
        _targetX = Mathf.FloorToInt(target.position.x);
        int generateToX = _targetX + viewDistance;
        int generateFromX = _targetX - PregenOffset; // чуть левее, чтобы избежать пробелов

        // Генерация вперёд
        for (int x = generateFromX; x <= generateToX; x++)
        {
            if (!tilemap.HasTile(new Vector3Int(x, groundHeightY, 0)))
            {
                GenerateColumn(x);
            }

            _generatedMinX = Mathf.Min(_generatedMinX, x);
            _generatedMaxX = Mathf.Max(_generatedMaxX, x);
        }
        //DeleteTileFromBeh(targetX);
    }

    void DeleteTileFromBeh(int targetX)
    {
        // Удаление тайлов позади
        int cleanupToX = targetX - cleanupDistanceBehind;

        for (int x = _generatedMinX; x < cleanupToX; x++)
        {
            tilemap.SetTile(new Vector3Int(x, groundHeightY, 0), null);

            for (int y = 1; y <= dirtDepth; y++)
            {
                tilemap.SetTile(new Vector3Int(x, groundHeightY - y, 0), null);
            }
        }

        _generatedMinX = cleanupToX; // Обновляем границу
    }

    void GenerateColumn(int x)
    {
        
        
        // Верхний слой - трава
        tilemap.SetTile(new Vector3Int(x, groundHeightY, 0), _grassTile);

        // Под травой - земля
        for (int y = 1; y <= dirtDepth; y++)
        {
            tilemap.SetTile(new Vector3Int(x, groundHeightY - y, 0), _dirtTile);
        }
    }
    private void OnDisable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnLevelAdvanced -= HandleLevelChange;
        }
    }

    private void HandleLevelChange()
    {
        tilemap.ClearAllTiles();
        _grassTile = LevelManager.Instance.CurrentLevel.topGroundTile;
        _dirtTile = LevelManager.Instance.CurrentLevel.groundFillTile;
    }
}
