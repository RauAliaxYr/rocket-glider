using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    
    private bool isMusicOn;
    private bool isSfxOn;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }

    private void LoadSettings()
    {
        isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        isSfxOn = PlayerPrefs.GetInt("SfxOn", 1) == 1;

        musicSource.mute = !isMusicOn;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.Play();
    }
    
    public void ToggleMusic(bool on)
    {
        isMusicOn = on;
        musicSource.mute = !on;
        PlayerPrefs.SetInt("MusicOn", on ? 1 : 0);
    }
    public bool IsMusicOn => isMusicOn;
    public bool IsSfxOn => isSfxOn;
}


