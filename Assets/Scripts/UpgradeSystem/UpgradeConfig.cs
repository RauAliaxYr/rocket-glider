using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Config")]
public class UpgradeConfig : ScriptableObject
{
    [SerializeField] private string _upgradeName;
    [SerializeField] private float[] _valuesPerLevel;
    [SerializeField] private int[] _costPerLevel;

    public float GetValue(int level)
    {
        return GetLevelValue(_valuesPerLevel, level);
    }
    public int GetCost(int level)
    {
        return GetLevelValue(_costPerLevel, level);
    }

    private T GetLevelValue<T>(T[] array, int level)
    {
        if (array == null || array.Length == 0)
            return default;

        return array[Mathf.Clamp(level, 0, array.Length - 1)];
    }
}
