using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTileGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase grassTile;
    [SerializeField] private TileBase dirtTile;
    [SerializeField] private Transform target;
    [SerializeField] private int viewDistance = 30;
    [SerializeField] private int groundHeightY = -3;
    [SerializeField] private int dirtDepth = 10;
    [SerializeField] private int cleanupDistanceBehind = 20;

    private int generatedMinX;
    private int generatedMaxX;

    void Start()
    {
        generatedMinX = int.MaxValue;
        generatedMaxX = int.MinValue;
    }

    void Update()
    {
        int targetX = Mathf.FloorToInt(target.position.x);
        int generateToX = targetX + viewDistance;
        int generateFromX = targetX - 1; // чуть левее, чтобы избежать пробелов

        // Генерация вперёд
        for (int x = generateFromX; x <= generateToX; x++)
        {
            if (!tilemap.HasTile(new Vector3Int(x, groundHeightY, 0)))
            {
                // Верхний слой - трава
                tilemap.SetTile(new Vector3Int(x, groundHeightY, 0), grassTile);

                // Под травой - земля
                for (int y = 1; y <= dirtDepth; y++)
                {
                    tilemap.SetTile(new Vector3Int(x, groundHeightY - y, 0), dirtTile);
                }
            }

            generatedMinX = Mathf.Min(generatedMinX, x);
            generatedMaxX = Mathf.Max(generatedMaxX, x);
        }

        // Удаление тайлов позади
        int cleanupToX = targetX - cleanupDistanceBehind;

        for (int x = generatedMinX; x < cleanupToX; x++)
        {
            tilemap.SetTile(new Vector3Int(x, groundHeightY, 0), null);

            for (int y = 1; y <= dirtDepth; y++)
            {
                tilemap.SetTile(new Vector3Int(x, groundHeightY - y, 0), null);
            }
        }

        generatedMinX = cleanupToX; // Обновляем границу
    }
}
