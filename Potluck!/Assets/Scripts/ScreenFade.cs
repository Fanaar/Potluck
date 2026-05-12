using UnityEngine;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    Coroutine currentFade;

    void Start()
    {
        canvasGroup.alpha = 0f;
    }

    public void FadeOut()
    {
        StartFade(1f);
    }

    public void FadeIn()
    {
        StartFade(0f);
    }

    void StartFade(float targetAlpha)
    {
        if (currentFade != null)
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeRoutine(targetAlpha));
    }

    IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                time / fadeDuration
            );

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}