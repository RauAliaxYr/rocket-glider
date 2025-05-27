using UnityEngine;
using UnityEngine.UI;

public class RewardCanavasStarter : MonoBehaviour
{
    [SerializeField] private Vector2 scalerWindows;
    [SerializeField] private Vector2 scalerAndroid;
    void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        GetComponent<CanvasScaler>().referenceResolution = scalerWindows;
#elif UNITY_ANDROID || UNITY_IOS
        GetComponent<CanvasScaler>().referenceResolution = scaleAndroid;
#endif
    }
}
