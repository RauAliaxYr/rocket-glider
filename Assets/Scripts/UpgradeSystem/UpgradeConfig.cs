using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Config")]
public class UpgradeConfig : ScriptableObject
{
    public string upgradeName;
    public float[] valuesPerLevel;

    public float GetValue(int level)
    {
        if (valuesPerLevel == null || valuesPerLevel.Length == 0)
            return 0f;

        if (level < 0)
            return valuesPerLevel[0];
        if (level >= valuesPerLevel.Length)
            return valuesPerLevel[^1]; // Последний элемент

        return valuesPerLevel[level];
    }
}
