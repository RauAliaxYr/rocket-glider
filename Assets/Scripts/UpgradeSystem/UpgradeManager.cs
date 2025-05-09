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
}
