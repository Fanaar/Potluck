using UnityEngine;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    public static bool IsFading = true;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        IsFading = true;

        float timer = 0f;

        // volledig zwart starten
        fadeCanvasGroup.alpha = 1f;

        // blokkeer alle muis/UI interactie
        fadeCanvasGroup.blocksRaycasts = true;
        fadeCanvasGroup.interactable = true;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            fadeCanvasGroup.alpha = 1f - (timer / fadeDuration);

            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;

        // interactie weer toestaan
        fadeCanvasGroup.blocksRaycasts = false;
        fadeCanvasGroup.interactable = false;

        IsFading = false;
    }
}