using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public Tilemap background;
    public TileBase baseTile;

    public Transform followTarget;
    public int groundHeight = -2;
    public int heightRange = 5;
    public int chunkWidth = 16;

    private int lastGeneratedX = -10;

    public int keepBehindDistance = 20;
    private int lastCleanedX = 0;

    void Update()
    {
        if (followTarget == null) return;

        int targetX = Mathf.FloorToInt(followTarget.position.x) + chunkWidth;

        while (lastGeneratedX < targetX)
        {
            GenerateColumn(lastGeneratedX);
            lastGeneratedX++;
        }

        CleanBehindTiles(Mathf.FloorToInt(followTarget.position.x) - keepBehindDistance);
    }

    void CleanBehindTiles(int xLimit)
    {
        for (int x = lastCleanedX; x < xLimit; x++)
        {
            for (int y = groundHeight; y < groundHeight + heightRange; y++)
            {
                background.SetTile(new Vector3Int(x, y, 0), null); // Удаление тайла
            }
        }
        lastCleanedX = xLimit;
    }



    void GenerateColumn(int x)
    {
        for (int y = groundHeight; y < groundHeight + heightRange; y++)
        {
            background.SetTile(new Vector3Int(x, y, 0), baseTile);
        }
    }
}
