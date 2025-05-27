using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeUI : MonoBehaviour
{
    public Transform upgradesParent;
    public GameObject upgradeItemPrefab;
    public AircraftController plane;
    public CanvasGroup canvasGroup;
    public Button restartButton;
    
    private float fadeDuration = 0.5f;

    private void Awake()
    {
        restartButton.onClick.AddListener(RestartGame);
    }
    public void RefreshUI()
    {
        upgradesParent.gameObject.SetActive(true);
        foreach (Transform child in upgradesParent)
        {
            Destroy(child.gameObject);
        }
        CreateUI();
    }

    void CreateUpgradeItem(string name, UpgradeConfig config, int level, System.Action onUpgrade, int purchases, int maxPurchase)
    {
        var item = Instantiate(upgradeItemPrefab, upgradesParent);
        var ui = item.GetComponent<UpgradeItemUI>();

        int cost = config.GetCost(level);
        bool canUpgrade = CurrencyManager.Instance.Coins >= cost;
        int innerProgress = purchases;
        int progressMax = maxPurchase; 

        ui.Setup(name, level, cost, canUpgrade, () =>
        {
            onUpgrade?.Invoke();
            RefreshUI();
        },innerProgress,progressMax);
    }
    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowRoutine());
        RefreshUI(); // обновление информации об улучшениях
    }

    private IEnumerator ShowRoutine()
    {
        // Плавное появление
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f; // Возвращаем время
        StartCoroutine(ResetGameState());
    }
    private IEnumerator ResetGameState()
    {
        // Скрываем баннер
        float elapsed = 0f;
        float fadeDuration = 0.5f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        gameObject.SetActive(false);
        plane.ResetAircraft();// Сбрасываем самолёт
        
        if (LevelManager.Instance.IsLevelReadyToAdvance)
        {
            LevelManager.Instance.ConfirmAdvanceLevel();
        }
    }

    private void CreateUI()
    {
        UpgradeManager manager = UpgradeManager.Instance;
        
        CreateUpgradeItem("Speed", manager.speedConfig, 
            manager.playerUpgrades.speedLevel,
            () => manager.TryUpgradeSpeed(),manager.playerUpgrades.speedPurchases,manager.playerUpgrades.speedLevel+3);

        CreateUpgradeItem("Launch", manager.launchConfig, 
            manager.playerUpgrades.launchLevel,
            () => manager.TryUpgradeLaunchForce(),manager.playerUpgrades.launchPurchases,manager.playerUpgrades.launchLevel+3);

        CreateUpgradeItem("Tap", manager.tapConfig, 
            manager.playerUpgrades.tapLevel,
            () => manager.TryUpgradeTapForce(),manager.playerUpgrades.tapPurchases,manager.playerUpgrades.tapLevel+3);
        
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)upgradesParent);
    }
}
