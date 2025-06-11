using UnityEngine;

public class FlightTracker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _coinsPerUnit = 10; // Количество монет за единицу расстояния
    
    public float DistanceTravelled { get; private set; }
    public int CoinsEarned { get; private set; }

    private Vector2 _startPosition;
    private bool _isLevelAdvanced;
    private LevelManager _levelManager;
    private CurrencyManager _currencyManager;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateDistance();
        CheckLevelProgress();
    }

    private void Initialize()
    {
        _startPosition = transform.position;
        _isLevelAdvanced = false;
        _levelManager = LevelManager.Instance;
        _currencyManager = CurrencyManager.Instance;

        if (_levelManager == null)
        {
            Debug.LogError("LevelManager instance is missing!", this);
        }

        if (_currencyManager == null)
        {
            Debug.LogError("CurrencyManager instance is missing!", this);
        }
    }

    private void UpdateDistance()
    {
        float currentDistance = transform.position.x - _startPosition.x;
        DistanceTravelled = Mathf.Max(0f, currentDistance);
    }

    private void CheckLevelProgress()
    {
        if (_isLevelAdvanced || _levelManager == null) return;

        float requiredDistance = _levelManager.CurrentLevel.requiredDistanceToPass;
        if (DistanceTravelled >= requiredDistance)
        {
            _isLevelAdvanced = true;
            _levelManager.TryAdvanceLevel(DistanceTravelled);
        }
    }

    public int CalculateAndAddCoins()
    {
        CoinsEarned = Mathf.FloorToInt(DistanceTravelled) / _coinsPerUnit;
        
        if (_currencyManager)
        {
            _currencyManager.AddCoins(CoinsEarned);
        }
        else
        {
            Debug.LogWarning("CurrencyManager is missing - coins not added");
        }

        return CoinsEarned;
    }

    public void ResetTracker()
    {
        _startPosition = transform.position;
        DistanceTravelled = 0f;
        CoinsEarned = 0;
        _isLevelAdvanced = false;
    }
}
