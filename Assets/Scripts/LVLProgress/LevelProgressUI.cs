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
    }

    private void Update()
    {
        if (flightTracker == null || distanceToPass <= 0f) return;

        float progress = Mathf.Clamp01(flightTracker.distanceTravelled / distanceToPass);
        progressFillImage.fillAmount = progress;
    }
}
