using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardBannerUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Button restartButton;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float countDuration = 1f;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
    }

    public void Show(int coinsEarned)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f; // Останавливаем время
        StartCoroutine(ShowRoutine(coinsEarned));
    }

    private IEnumerator ShowRoutine(int coins)
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

        // Счётчик монет
        yield return StartCoroutine(CountCoins(coins));
    }

    private IEnumerator CountCoins(int targetAmount)
    {
        int current = 0;
        float elapsed = 0f;

        while (elapsed < countDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / countDuration);
            current = Mathf.RoundToInt(Mathf.Lerp(0, targetAmount, t));
            rewardText.text = $"Coins: {current}";
            yield return null;
        }

        rewardText.text = $"Coins: {targetAmount}";
    }

    private void RestartGame()
    {
        Time.timeScale = 1f; // Возвращаем время
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
