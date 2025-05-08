using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private LevelData[] levels;
    private int currentLevelIndex = 0;

    public LevelData CurrentLevel => levels[currentLevelIndex];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
            currentLevelIndex++;
            Debug.Log($"Переключение на уровень: {CurrentLevel.levelName}");
            // Можно запустить ивент для других систем, чтобы перегенерировали окружение
        }
    }

    public void ResetLevels()
    {
        currentLevelIndex = 0;
    }
}
