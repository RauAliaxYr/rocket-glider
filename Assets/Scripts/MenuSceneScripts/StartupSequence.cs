using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartupSequence : MonoBehaviour
{
    [SerializeField] private Image studioLogoImage;
    [SerializeField] private GameObject logoCanavas;
    [SerializeField] private GameObject mainMenuRoot;
    [SerializeField] private float logoFadeDuration = 1.5f;
    [SerializeField] private float logoDisplayDuration = 1.5f;

    private void Start()
    {
        studioLogoImage.canvasRenderer.SetAlpha(0f);
        mainMenuRoot.SetActive(false);
        StartCoroutine(PlayStartupSequence());
    }

    private IEnumerator PlayStartupSequence()
    {
        studioLogoImage.CrossFadeAlpha(1f, logoFadeDuration, false);
        yield return new WaitForSeconds(logoFadeDuration + logoDisplayDuration);

        studioLogoImage.CrossFadeAlpha(0f, 0.8f, false);
        yield return new WaitForSeconds(0.8f);

        studioLogoImage.gameObject.SetActive(false);
        logoCanavas.SetActive(false);
        mainMenuRoot.SetActive(true);
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("SampleScene"); // Убедись, что имя совпадает
    }
}

