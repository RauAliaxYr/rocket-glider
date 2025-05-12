using UnityEngine;

public class CurrencyManager: MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int Coins { get; private set; } = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        // TODO: Save coins if нужна персистентность
    }

    public bool TrySpendCoins(int cost)
    {
        if (Coins >= cost)
        {
            Coins -= cost;
            return true;
        }
        return false;
    }
}
