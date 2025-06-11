using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartupSequence : MonoBehaviour
{
    [Header("Logo Settings")]
    [SerializeField] private Image _studioLogoImage;
    [SerializeField] private GameObject _logoCanvas;
    [SerializeField] private float _logoFadeInDuration = 1.5f;
    [SerializeField] private float _logoDisplayDuration = 1.5f;
    [SerializeField] private float _logoFadeOutDuration = 0.8f;

    [Header("Menu References")]
    [SerializeField] private GameObject _mainMenuRoot;
    [SerializeField] private string _targetSceneName = "SampleScene";

    [Header("Audio")]
    [SerializeField] private AudioClip _menuMusic;

    private void Start()
    {
        InitializeSequence();
        StartCoroutine(PlayStartupSequence());
    }

    private void InitializeSequence()
    {
        if (_studioLogoImage != null)
        {
            _studioLogoImage.canvasRenderer.SetAlpha(0f);
        }

        if (_mainMenuRoot != null)
        {
            _mainMenuRoot.SetActive(false);
        }

        PlayMenuMusic();
    }

    private void PlayMenuMusic()
    {
        if (_menuMusic != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(_menuMusic);
        }
    }

    private IEnumerator PlayStartupSequence()
    {
        if (_studioLogoImage == null || _logoCanvas == null)
        {
            ShowMainMenuImmediately();
            yield break;
        }

        yield return FadeLogo(1f, _logoFadeInDuration); // Fade in
        yield return new WaitForSeconds(_logoDisplayDuration);
        yield return FadeLogo(0f, _logoFadeOutDuration); // Fade out

        HideLogoAndShowMenu();
    }

    private IEnumerator FadeLogo(float targetAlpha, float duration)
    {
        if (_studioLogoImage != null)
        {
            _studioLogoImage.CrossFadeAlpha(targetAlpha, duration, false);
            yield return new WaitForSeconds(duration);
        }
    }

    private void HideLogoAndShowMenu()
    {
        if (_studioLogoImage != null)
        {
            _studioLogoImage.gameObject.SetActive(false);
        }

        if (_logoCanvas != null)
        {
            _logoCanvas.SetActive(false);
        }

        if (_mainMenuRoot != null)
        {
            _mainMenuRoot.SetActive(true);
        }
    }

    private void ShowMainMenuImmediately()
    {
        if (_mainMenuRoot)
        {
            _mainMenuRoot.SetActive(true);
        }
    }

    public void OnStartButtonClicked()
    {
        LoadTargetScene();
    }
    private void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(_targetSceneName))
        {
            SceneManager.LoadScene(_targetSceneName);
        }
        else
        {
            Debug.LogError("Target scene name is not set!");
        }
    }
}

