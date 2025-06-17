using System;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Configurations")]
    [SerializeField] private UpgradeConfig _speedConfig;
    [SerializeField] private UpgradeConfig _launchConfig;
    [SerializeField] private UpgradeConfig _tapConfig;

    [Header("Player Data")]
    [SerializeField] private UpgradeData _playerUpgrades = new();
    
    [SerializeField] private CurrencyManager _currencyManager;
    [SerializeField] private UpgradeUI _upgradeUI;

    private void Awake()
    {
        InitializeSingleton();
    }

    // Основные геттеры
    public float GetTargetSpeed() => _speedConfig.GetValue(_playerUpgrades.speedLevel);
    public float GetLaunchForce() => _launchConfig.GetValue(_playerUpgrades.launchLevel);
    public float GetTapImpulse() => _tapConfig.GetValue(_playerUpgrades.tapLevel);
    public int GetMaxTaps() => _playerUpgrades.PlaneLevel;

    // Улучшение с проверкой валюты
    public bool TryUpgrade(UpgradeData.UpgradeType type)
    {
        
        int cost = GetUpgradeCost(type);
        if (!CurrencyManager.Instance.TrySpendCoins(cost))
            return false;

        bool upgraded = _playerUpgrades.TryUpgrade(type);
        _upgradeUI.RefreshAllUpgrades();
        return upgraded;
    }

    public int GetUpgradeCost(UpgradeData.UpgradeType type)
    {
        return type switch
        {
            UpgradeData.UpgradeType.Speed => _speedConfig.GetCost(_playerUpgrades.speedLevel),
            UpgradeData.UpgradeType.Launch => _launchConfig.GetCost(_playerUpgrades.launchLevel),
            UpgradeData.UpgradeType.Tap => _tapConfig.GetCost(_playerUpgrades.tapLevel),
            _ => 0
        };
    }
    public int GetRequiredPurchases(UpgradeData.UpgradeType type)
    {
        var data = GetUpgradeData(type);
        return UpgradeData.BASE_PURCHASES_PER_LEVEL + data.currentLevel;
    }
    public string GetUpgradeName(UpgradeData.UpgradeType type)
    {
        return type switch
        {
            UpgradeData.UpgradeType.Speed => "Speed",
            UpgradeData.UpgradeType.Launch => "Launch Power",
            UpgradeData.UpgradeType.Tap => "Tap Boost",
            _ => string.Empty
        };
    }
    public (int currentLevel, int currentPurchases) GetUpgradeData(UpgradeData.UpgradeType type)
    {
        return type switch
        {
            UpgradeData.UpgradeType.Speed => (_playerUpgrades.speedLevel, _playerUpgrades.speedPurchases),
            UpgradeData.UpgradeType.Launch => (_playerUpgrades.launchLevel, _playerUpgrades.launchPurchases),
            UpgradeData.UpgradeType.Tap => (_playerUpgrades.tapLevel, _playerUpgrades.tapPurchases),
            _ => (0, 0)
        };
    }

    #region Initialization
    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _playerUpgrades.Load();
    }
    #endregion
    
    public bool CanUpgrade(UpgradeData.UpgradeType type)
    {
        int cost = GetUpgradeCost(type);
        return _currencyManager.CanSpendCoins(cost);
    }
}