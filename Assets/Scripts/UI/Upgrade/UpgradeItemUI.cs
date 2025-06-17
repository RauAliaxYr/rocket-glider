using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemUI : MonoBehaviour
{
    [Header("Text Components")]
    [SerializeField] private TextMeshProUGUI _upgradeNameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _progressText;

    [Header("UI Elements")]
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Image _progressFillImage;

    private System.Action _onUpgradeCallback;

    public void Setup(
        string upgradeName,
        int currentLevel,
        int cost,
        bool canUpgrade,
        System.Action onUpgrade,
        int purchasesCompleted,
        int purchasesRequired
    )
    {
        // Установка текстовых полей
        _upgradeNameText.text = upgradeName;
        _levelText.text = $"Level: {currentLevel}";
        _costText.text = $"Cost: {cost}"; // Новый метод
        _upgradeButton.interactable = canUpgrade;
        _onUpgradeCallback = onUpgrade;

        // Коллбэк
        _upgradeButton.onClick.RemoveAllListeners();
        _upgradeButton.onClick.AddListener(() => _onUpgradeCallback?.Invoke());
        if (_progressFillImage)
        {
            _progressFillImage.fillAmount = Mathf.Clamp01((float)purchasesCompleted / purchasesRequired);
        }
        if (_progressText)
            _progressText.text = $"{purchasesCompleted} / {purchasesRequired}";;
    }

}
