using UnityEngine;

public class AircraftController : MonoBehaviour
{
    [SerializeField] private AircraftLauncher launcher;
    [SerializeField] private AircraftTapController tapController;
    [SerializeField] private AircraftFlightController flightController;
    [SerializeField] private AircraftCrashHandler crashHandler;
    
    private void OnEnable()
    {
        launcher.OnLaunch += HandleLaunch;
    }
    
    private void OnDisable()
    {
        launcher.OnLaunch -= HandleLaunch;
    }
    
    private void HandleLaunch()
    {
        tapController.SetHasLaunched(true);
    }
    
    public void ResetAircraft()
    {
        launcher.ResetToStart();
        tapController.ResetTaps();
        crashHandler.ResetCrashState();
    }
}
