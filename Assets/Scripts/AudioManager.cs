using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip menuMusic;
    public AudioClip flightMusic;
    public AudioClip tapSfx;
    public AudioClip launchSfx;
    public AudioClip coinSfx;
    public AudioClip buttonClickSfx;

    private bool isMusicOn;
    private bool isSfxOn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else Destroy(gameObject);
    }

    private void LoadSettings()
    {
        isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        isSfxOn = PlayerPrefs.GetInt("SfxOn", 1) == 1;

        musicSource.mute = !isMusicOn;
        sfxSource.mute = !isSfxOn;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip != null && isSfxOn)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void ToggleMusic(bool on)
    {
        isMusicOn = on;
        musicSource.mute = !on;
        PlayerPrefs.SetInt("MusicOn", on ? 1 : 0);
    }

    public void ToggleSfx(bool on)
    {
        isSfxOn = on;
        sfxSource.mute = !on;
        PlayerPrefs.SetInt("SfxOn", on ? 1 : 0);
    }

    public bool IsMusicOn => isMusicOn;
    public bool IsSfxOn => isSfxOn;
}

