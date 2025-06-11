using System;
using UnityEngine;

public class CurrencyManager: MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    public static event Action<int> OnCoinsChanged;
    
    private const string COINS_KEY = "Coins";
    
    private int _coins;
    public int Coins 
    { 
        get => _coins;
        private set
        {
            if (_coins != value)
            {
                _coins = value;
                OnCoinsChanged?.Invoke(_coins);
                Save();
            }
        }
    }
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
        if (amount <= 0)
        {
            Debug.LogWarning($"Попытка добавить некорректное количество: {amount}");
            return;
        }

        try
        {
            Coins = checked(Coins + amount);
        }
        catch (OverflowException)
        {
            Debug.LogError("Переполнение при добавлении монет!");
            Coins = int.MaxValue;
        }
    }

    public bool TrySpendCoins(int amount)
    {
        Coins -= amount;
        return true;
    }
    
    private void Save()
    {
        PlayerPrefs.SetInt(COINS_KEY, Coins);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        _coins = PlayerPrefs.GetInt(COINS_KEY, 0);
        OnCoinsChanged?.Invoke(_coins); // Уведомляем подписчиков
    }
}
