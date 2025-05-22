using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public event Action OnLevelAdvanced;
    public static LevelManager Instance { get; private set; }
    public bool IsLevelReadyToAdvance { get; private set; } = false;

    [SerializeField] private LevelData[] levels;
    private int currentLevelIndex = 0;
    private const string LevelKey = "CurrentLevelIndex";

    public LevelData CurrentLevel => levels[currentLevelIndex];

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.flightMusic);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            //ResetAllProgress();
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentLevelIndex = PlayerPrefs.GetInt(LevelKey, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TryAdvanceLevel(float distanceTraveled)
    {
        if (currentLevelIndex < levels.Length - 1 &&
            distanceTraveled >= CurrentLevel.requiredDistanceToPass)
        {
            IsLevelReadyToAdvance = true; // Только флаг
        }
    }

    public void ResetLevels()
    {
        currentLevelIndex = 0;
    }
    public void ConfirmAdvanceLevel()
    {
        if (!IsLevelReadyToAdvance) return;
        currentLevelIndex++;
        IsLevelReadyToAdvance = false;
        PlayerPrefs.SetInt(LevelKey, currentLevelIndex);
        PlayerPrefs.Save();
        OnLevelAdvanced?.Invoke(); // Сигнал для других систем
    }
    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
    }
}
