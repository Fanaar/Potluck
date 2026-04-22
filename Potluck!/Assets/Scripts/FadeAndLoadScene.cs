using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeAndLoadScene : MonoBehaviour
{
    [Header("Fade UI")]
    [SerializeField] private Image fadeImage;

    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;

    private bool isTransitioning = false; // voorkomt spam klikken

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void StartTransition()
    {
        if (isTransitioning) return;

        isTransitioning = true;
        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;

        c.a = 1f;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
    }

    IEnumerator FadeOutAndLoad()
    {
        float t = 0f;

        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float alpha = Mathf.Clamp01(t / fadeDuration);
            c.a = alpha;

            fadeImage.color = c;

            yield return null;
        }

        // Zorg dat hij écht 100% zwart is
        c.a = 1f;
        fadeImage.color = c;

        Debug.Log("Fade klaar, scene laden");

        SceneManager.LoadScene(sceneToLoad);
    }
}