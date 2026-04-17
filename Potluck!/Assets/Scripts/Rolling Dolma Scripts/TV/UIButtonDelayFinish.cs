using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIButtonDelayFinish : MonoBehaviour
{
    [Header("Finish Settings")]
    public FinishBranchScene finishScene;

    [Header("Timer Settings")]
    public float delay = 30f;

    [Header("Fade Settings")]
    public Image fadePanel; // UI Image (je panel)
    public float fadeDuration = 1f;
    public float fadeCompleteBeforeEnd = 5f; // moet 5 sec vóór einde klaar zijn

    [Header("Optional")]
    public bool disableButtonAfterClick = true;
    public GameObject buttonObject;

    private bool hasStarted = false;

    public void StartTimer()
    {
        if (hasStarted) return;
        hasStarted = true;

        Debug.Log("Timer gestart: " + delay + " seconden");

        if (disableButtonAfterClick && buttonObject != null)
        {
            buttonObject.SetActive(false);
        }

        StartCoroutine(TimerSequence());
    }

    IEnumerator TimerSequence()
    {
        // ⏱️ bereken wanneer fade moet starten
        float fadeStartTime = delay - fadeCompleteBeforeEnd - fadeDuration;

        // safety (als iemand rare waardes invult)
        if (fadeStartTime < 0)
            fadeStartTime = 0;

        // 1. Wacht tot fade moet beginnen
        yield return new WaitForSeconds(fadeStartTime);

        // 2. Fade uitvoeren
        if (fadePanel != null)
        {
            yield return StartCoroutine(FadeToBlack());
        }

        // 3. Wacht resterende tijd tot delay compleet is
        float remainingTime = delay - fadeStartTime - fadeDuration;
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        // 4. Scene switch
        if (finishScene != null)
        {
            finishScene.FinishScene();
        }
        else
        {
            Debug.LogWarning("FinishScene script niet gekoppeld!");
        }
    }

    IEnumerator FadeToBlack()
    {
        float time = 0f;
        Color color = fadePanel.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            // alpha van 0 → 1 (zelfde als 0 → 255 visueel)
            color.a = Mathf.Lerp(0f, 1f, t);
            fadePanel.color = color;

            yield return null;
        }

        color.a = 1f;
        fadePanel.color = color;
    }
}