using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;

    private void OnEnable()
    {
        ValidateReferences();
        UpdateCoinsDisplay(CurrencyManager.Instance.Coins);
        CurrencyManager.OnCoinsChanged += UpdateCoinsDisplay;
    }

    private void OnDisable()
    {
        CurrencyManager.OnCoinsChanged -= UpdateCoinsDisplay;
    }

    private void ValidateReferences()
    {
        if (!coinsText)
        {
            Debug.LogError("Coins Text reference is missing!", this);
            enabled = false;
        }

        if (!CurrencyManager.Instance)
        {
            Debug.LogError("CurrencyManager instance is missing!", this);
            enabled = false;
        }
    }

    private void UpdateCoinsDisplay(int newCoins)
    {
        coinsText.text = newCoins.ToString();
    }
}
