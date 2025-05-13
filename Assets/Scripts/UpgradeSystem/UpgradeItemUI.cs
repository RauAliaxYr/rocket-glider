using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemUI : MonoBehaviour
{
    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public Button upgradeButton;

    private System.Action onUpgradeClick;

    public void Setup(string name, int level, int cost, bool canUpgrade, System.Action onClick)
    {
        upgradeNameText.text = name;
        levelText.text = $"Level: {level}";
        costText.text = $"Cost: {cost}";
        upgradeButton.interactable = canUpgrade;
        onUpgradeClick = onClick;

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => onUpgradeClick?.Invoke());
    }
}
