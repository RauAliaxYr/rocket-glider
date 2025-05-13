using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeUI : MonoBehaviour
{
    public Transform upgradesParent;
    public GameObject upgradeItemPrefab;
    public SlingshotLauncher plane;
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

    void CreateUpgradeItem(string name, UpgradeConfig config, int level, System.Action onUpgrade)
    {
        var item = Instantiate(upgradeItemPrefab, upgradesParent);
        var ui = item.GetComponent<UpgradeItemUI>();

        int cost = config.GetCost(level);
        bool canUpgrade = CurrencyManager.Instance.Coins >= cost;

        ui.Setup(name, level, cost, canUpgrade, () =>
        {
            onUpgrade?.Invoke();
            RefreshUI();
        });
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
        
        plane.ResetToStart();
        // Сбрасываем самолёт
    }

    private void CreateUI()
    {
        CreateUpgradeItem("Speed", UpgradeManager.Instance.speedConfig, 
            UpgradeManager.Instance.playerUpgrades.speedLevel,
            () => UpgradeManager.Instance.TryUpgradeSpeed());

        CreateUpgradeItem("Launch", UpgradeManager.Instance.launchConfig, 
            UpgradeManager.Instance.playerUpgrades.launchLevel,
            () => UpgradeManager.Instance.TryUpgradeLaunchForce());

        CreateUpgradeItem("Tap", UpgradeManager.Instance.tapConfig, 
            UpgradeManager.Instance.playerUpgrades.tapLevel,
            () => UpgradeManager.Instance.TryUpgradeTapForce());
        
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)upgradesParent);
    }
}
