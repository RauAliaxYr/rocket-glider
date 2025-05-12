using UnityEngine;

public class UpgradeData
{
    public int speedLevel = 0;
    public int launchLevel = 0;
    public int tapLevel = 0;

    public int speedPurchases = 0;
    public int launchPurchases = 0;
    public int tapPurchases = 0;

    private const int purchasesPerLevel = 3;

    public int PlaneLevel => Mathf.Min(speedLevel, launchLevel, tapLevel) / purchasesPerLevel + 2;

    public bool TryUpgrade(ref int purchases, ref int level)
    {
        purchases++;
        if (purchases >= purchasesPerLevel)
        {
            purchases = 0;
            level++;
            return true; // уровень апгрейда вырос
        }
        return false;
    }
}
