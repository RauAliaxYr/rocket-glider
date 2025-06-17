using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    private const string SPEED_KEY = "SpeedLevel";
    private const string LAUNCH_KEY = "LaunchLevel";
    private const string TAP_KEY = "TapLevel";
    private const string SPEED_PURCHASES_KEY = "SpeedPurchases";
    private const string LAUNCH_PURCHASES_KEY = "LaunchPurchases";
    private const string TAP_PURCHASES_KEY = "TapPurchases";
    
    public const int BASE_PURCHASES_PER_LEVEL = 3;

    [Header("Current Levels")]
    public int speedLevel;
    public int launchLevel;
    public int tapLevel;

    [Header("Current Purchases")]
    public int speedPurchases;
    public int launchPurchases;
    public int tapPurchases;

    public int PlaneLevel => Mathf.Min(speedLevel, launchLevel, tapLevel) + 1;

    public bool TryUpgrade(UpgradeType type)
    {
        ref int level = ref GetLevelRef(type);
        ref int purchases = ref GetPurchasesRef(type);
        purchases++;
        Save();
        
        if (purchases >= BASE_PURCHASES_PER_LEVEL)
        {
            level++;
            purchases = 0;
            
            return true;
        }

        return false;
    }

    public void Save()
    {
        PlayerPrefs.SetInt(SPEED_KEY, speedLevel);
        PlayerPrefs.SetInt(LAUNCH_KEY, launchLevel);
        PlayerPrefs.SetInt(TAP_KEY, tapLevel);
        PlayerPrefs.SetInt(SPEED_PURCHASES_KEY, speedPurchases);
        PlayerPrefs.SetInt(LAUNCH_PURCHASES_KEY, launchPurchases);
        PlayerPrefs.SetInt(TAP_PURCHASES_KEY, tapPurchases);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        speedLevel = PlayerPrefs.GetInt(SPEED_KEY, 0);
        launchLevel = PlayerPrefs.GetInt(LAUNCH_KEY, 0);
        tapLevel = PlayerPrefs.GetInt(TAP_KEY, 0);
        speedPurchases = PlayerPrefs.GetInt(SPEED_PURCHASES_KEY, 0);
        launchPurchases = PlayerPrefs.GetInt(LAUNCH_PURCHASES_KEY, 0);
        tapPurchases = PlayerPrefs.GetInt(TAP_PURCHASES_KEY, 0);
    }

    private ref int GetLevelRef(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Speed: return ref speedLevel;
            case UpgradeType.Launch: return ref launchLevel;
            case UpgradeType.Tap: return ref tapLevel;
            default: return ref speedLevel;
        }
    }

    private ref int GetPurchasesRef(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Speed: return ref speedPurchases;
            case UpgradeType.Launch: return ref launchPurchases;
            case UpgradeType.Tap: return ref tapPurchases;
            default: return ref speedPurchases;
        }
    }

    public enum UpgradeType
    {
        Speed,
        Launch,
        Tap
    }
}
