using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUI : MonoBehaviour
{
    [SerializeField] private Image progressFillImage; // ссылка на fill image
    [SerializeField] private FlightTracker flightTracker;

    private float distanceToPass;

    private void Start()
    {
        distanceToPass = LevelManager.Instance.CurrentLevel.requiredDistanceToPass;
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnLevelAdvanced += ResetDistanceToPass;
        }
    }

    private void Update()
    {
        if (!flightTracker || distanceToPass <= 0f) return;

        float progress = Mathf.Clamp01(flightTracker.DistanceTravelled / distanceToPass);
        progressFillImage.fillAmount = progress;
    }
    private void OnDisable()
    {
        LevelManager.Instance.OnLevelAdvanced -= ResetDistanceToPass;
    }

    private void ResetDistanceToPass()
    {
        distanceToPass = LevelManager.Instance.CurrentLevel.requiredDistanceToPass;
    }
}
