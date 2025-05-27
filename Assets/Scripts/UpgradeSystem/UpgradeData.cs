using UnityEngine;

public class UpgradeData
{
    public int speedLevel = 0;
    public int launchLevel = 0;
    public int tapLevel = 0;

    public int speedPurchases = 0;
    public int launchPurchases = 0;
    public int tapPurchases = 0;

    private int purchasesPerLevel = 3;

    public int PlaneLevel => Mathf.Min(speedLevel, launchLevel, tapLevel) + 1;

    public bool TryUpgrade(ref int purchases, ref int level)
    {
        int requiredPurchases = purchasesPerLevel + level;

        purchases++;
        if (purchases >= requiredPurchases)
        {
            purchases = 0;
            level++;
            return true; // уровень апгрейда вырос
        }
        return false;
    }
    public void Save()
    {
        PlayerPrefs.SetInt("SpeedLevel", speedLevel);
        PlayerPrefs.SetInt("LaunchLevel", launchLevel);
        PlayerPrefs.SetInt("TapLevel", tapLevel);
        PlayerPrefs.SetInt("SpeedLevelPurch", speedPurchases);
        PlayerPrefs.SetInt("LaunchLevelPurch", launchPurchases);
        PlayerPrefs.SetInt("TapLevelPurch", tapPurchases);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        speedLevel = PlayerPrefs.GetInt("SpeedLevel", 0);
        launchLevel = PlayerPrefs.GetInt("LaunchLevel", 0);
        tapLevel = PlayerPrefs.GetInt("TapLevel", 0);
        speedPurchases=PlayerPrefs.GetInt("SpeedLevelPurch", 0);
        launchPurchases=PlayerPrefs.GetInt("LaunchLevelPurch", 0);
        tapPurchases=PlayerPrefs.GetInt("TapLevelPurch",0 );
    }
}
