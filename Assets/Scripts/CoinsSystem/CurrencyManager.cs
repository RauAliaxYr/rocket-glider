using System;
using UnityEngine;

public class CurrencyManager: MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int Coins { get;  set; } = 0;
    
    public static event Action<int> OnCoinsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        OnCoinsChanged?.Invoke(Coins);
        Save();
    }

    public bool TrySpendCoins(int amount)
    {
        if (Coins < amount) return false;

        Coins -= amount;
        OnCoinsChanged?.Invoke(Coins);
        return true;
    }
    
    private void Save()
    {
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        Coins = PlayerPrefs.GetInt("Coins", 0);
    }
    
}
