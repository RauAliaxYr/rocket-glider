using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Config")]
public class UpgradeConfig : ScriptableObject
{
    public string upgradeName;
    public float[] valuesPerLevel;
    public int[] costPerLevel;

    public float GetValue(int level)
    {
        if (valuesPerLevel == null || valuesPerLevel.Length == 0)
            return 0f;
        if (level < 0)
            return valuesPerLevel[0];
        if (level >= valuesPerLevel.Length)
            return valuesPerLevel[^1];
        return valuesPerLevel[level];
    }

    public int GetCost(int level)
    {
        if (costPerLevel == null || costPerLevel.Length == 0)
            return 0;
        if (level < 0)
            return costPerLevel[0];
        if (level >= costPerLevel.Length)
            return costPerLevel[^1];
        return costPerLevel[level];
    }
}
