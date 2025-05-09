using UnityEngine;

public class UpgradeData
{
    public int speedLevel = 0;
    public int launchLevel = 0;
    public int tapLevel = 0;

    public int PlaneLevel => Mathf.Min(speedLevel, launchLevel, tapLevel) / 3 + 2;
}
