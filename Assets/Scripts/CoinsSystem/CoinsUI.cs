using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;

    void Start()
    {
        UpdateCoins(CurrencyManager.Instance.Coins);
    }

    private void OnEnable()
    {
        CurrencyManager.OnCoinsChanged += UpdateCoins;
    }

    private void OnDisable()
    {
        CurrencyManager.OnCoinsChanged -= UpdateCoins;
    }

    private void UpdateCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }
}
