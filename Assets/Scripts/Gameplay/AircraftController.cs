using UnityEngine;

public class AircraftController : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private AircraftLauncher _launcher;
    [SerializeField] private AircraftTapController _tapController;
    [SerializeField] private AircraftCrashHandler _crashHandler;
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        ValidateComponents();
    }

    private void OnEnable()
    {
        if (_launcher)
        {
            _launcher.OnLaunch += HandleLaunch;
        }
    }

    private void OnDisable()
    {
        if (_launcher)
        {
            _launcher.OnLaunch -= HandleLaunch;
        }
    }

    private void ValidateComponents()
    {
        if (!_launcher)
        {
            Debug.LogError($"{name}: AircraftLauncher reference is missing!", this);
            enabled = false;
        }

        if (!_tapController)
            Debug.LogWarning($"{name}: TapController reference is missing!", this);

        if (!_crashHandler)
            Debug.LogWarning($"{name}: CrashHandler reference is missing!", this);
    }

    private void HandleLaunch()
    {
        if (_tapController)
        {
            _tapController.SetHasLaunched(true);
            _audioSource.Play();
        }
    }

    public void ResetAircraft()
    {
        if (_launcher) 
            _launcher.ResetToStart();
        
        if (_tapController) 
            _tapController.ResetTaps();
        
        if (_crashHandler) 
            _crashHandler.ResetCrashState();
    }
}

