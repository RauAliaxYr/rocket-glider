using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Upgrade Configs")]
    public UpgradeConfig speedConfig;
    public UpgradeConfig launchConfig;
    public UpgradeConfig tapConfig;

    [Header("Player Upgrades")]
    public UpgradeData playerUpgrades = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public float GetTargetSpeed() => speedConfig.GetValue(playerUpgrades.speedLevel);
    public float GetLaunchForce() => launchConfig.GetValue(playerUpgrades.launchLevel);
    public float GetTapImpulse() => tapConfig.GetValue(playerUpgrades.tapLevel);
    public int GetMaxTaps() => playerUpgrades.PlaneLevel;
    
    public bool TryUpgradeSpeed()
    {
        int cost = speedConfig.GetCost(playerUpgrades.speedLevel);
        if (CurrencyManager.Instance.TrySpendCoins(cost))
        {
            return playerUpgrades.TryUpgrade(ref playerUpgrades.speedPurchases, ref playerUpgrades.speedLevel);
        }
        return false;
    }
    public bool TryUpgradeLaunchForce()
    {
        int cost = speedConfig.GetCost(playerUpgrades.launchLevel);
        if (CurrencyManager.Instance.TrySpendCoins(cost))
        {
            return playerUpgrades.TryUpgrade(ref playerUpgrades.launchPurchases, ref playerUpgrades.launchLevel);
        }
        return false;
    }
    public bool TryUpgradeTapForce()
    {
        int cost = speedConfig.GetCost(playerUpgrades.tapLevel);
        if (CurrencyManager.Instance.TrySpendCoins(cost))
        {
            return playerUpgrades.TryUpgrade(ref playerUpgrades.tapPurchases, ref playerUpgrades.tapLevel);
        }
        return false;
    }
}
