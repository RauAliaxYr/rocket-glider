using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class UpgradeUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform _upgradesParent;
    [SerializeField] private GameObject _upgradeItemPrefab;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private Button _resetButton;
    
    [Header("Animation")]
    [SerializeField] private float _fadeDuration = 0.5f;
    
    [Header("Plane")]
    [SerializeField] private AircraftController _plane;
    
    
    private void Awake()
    {
        _resetButton.onClick.AddListener(RestartGame);
    }
    public void ShowUI()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeAnimation(0f, 1f));
        RefreshAllUpgrades();
    }

    public void HideUI()
    {
        StartCoroutine(FadeAnimation(1f, 0f, () => gameObject.SetActive(false)));
    }

    public void RefreshAllUpgrades()
    {
        ClearUpgradeItems();
        
        foreach (var type in System.Enum.GetValues(typeof(UpgradeData.UpgradeType)))
        {
            CreateUpgradeItem((UpgradeData.UpgradeType)type);
        }
    }

    private void CreateUpgradeItem(UpgradeData.UpgradeType type)
    {
        var item = Instantiate(_upgradeItemPrefab, _upgradesParent);
        if (item.TryGetComponent(out UpgradeItemUI ui))
        {
            var data = _upgradeManager.GetUpgradeData(type);
            ui.Setup(
                _upgradeManager.GetUpgradeName(type),
                data.currentLevel,
                _upgradeManager.GetUpgradeCost(type),
                _upgradeManager.CanUpgrade(type),
                () => _upgradeManager.TryUpgrade(type),
                data.currentPurchases,
                _upgradeManager.GetRequiredPurchases(type) // Максимум покупок для уровня
            );
        }
    }

    private void ClearUpgradeItems()
    {
        foreach (Transform child in _upgradesParent)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator FadeAnimation(float from, float to, System.Action onComplete = null)
    {
        float elapsed = 0f;
        while (elapsed < _fadeDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / _fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        _canvasGroup.alpha = to;
        onComplete?.Invoke();
    }
    private void RestartGame()
    {
        HideUI();
        Time.timeScale = 1f;
        _plane.ResetAircraft();
    }
}

