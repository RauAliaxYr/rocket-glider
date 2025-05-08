using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public int requiredDistanceToPass;

    public TileBase topGroundTile;
    public TileBase groundFillTile;
    public TileBase backgroundTile;

    public Color backgroundColor;

    public GameObject[] obstaclePrefabs;
}
