using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public event Action OnLevelAdvanced;
    public static LevelManager Instance { get; private set; }
    public bool IsLevelReadyToAdvance { get; private set; } = false;
    
    [Header("Level Settings")]
    [SerializeField] private LevelData[] _levels;
    [SerializeField] private AudioClip _levelMusic;
    
    private const string LEVEL_KEY = "CurrentLevelIndex";
    private int _currentLevelIndex = 0;

    public LevelData CurrentLevel => _levels[_currentLevelIndex];
    public int CurrentLevelNumber => _currentLevelIndex + 1;
    public int TotalLevels => _levels.Length;
    
    private void Awake()
    {
        InitializeSingleton();
        LoadLevelProgress();
    }

    private void Start()
    {
        PlayLevelMusic();
    }

    #region Initialization
    private void InitializeSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void LoadLevelProgress()
    {
        _currentLevelIndex = PlayerPrefs.GetInt(LEVEL_KEY, 0);
        ClampLevelIndex();
    }

    private void PlayLevelMusic()
    {
        if (_levelMusic != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(_levelMusic);
        }
    }
    #endregion

    #region Level Progression
    public void TryAdvanceLevel(float distanceTraveled)
    {
        if (CanAdvanceLevel(distanceTraveled))
        {
            IsLevelReadyToAdvance = true;
        }
    }

    public void ConfirmAdvanceLevel()
    {
        if (!IsLevelReadyToAdvance) return;

        _currentLevelIndex++;
        ClampLevelIndex();
        
        SaveLevelProgress();
        IsLevelReadyToAdvance = false;
        
        OnLevelAdvanced?.Invoke();
    }

    private bool CanAdvanceLevel(float distanceTraveled)
    {
        return _currentLevelIndex < _levels.Length - 1 && 
               distanceTraveled >= CurrentLevel.requiredDistanceToPass;
    }

    private void ClampLevelIndex()
    {
        _currentLevelIndex = Mathf.Clamp(_currentLevelIndex, 0, _levels.Length - 1);
    }
    #endregion

    #region Progress Management
    private void SaveLevelProgress()
    {
        PlayerPrefs.SetInt(LEVEL_KEY, _currentLevelIndex);
        PlayerPrefs.Save();
    }

    public void ResetLevelProgress()
    {
        _currentLevelIndex = 0;
        SaveLevelProgress();
    }

    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
        ResetLevelProgress();
    }
    #endregion

    #region Editor Helpers
    // Для дебага в редакторе
    public void DebugAdvanceLevel()
    {
        _currentLevelIndex++;
        ClampLevelIndex();
        Debug.Log($"Debug: Advanced to level {CurrentLevelNumber}");
    }
    #endregion
}