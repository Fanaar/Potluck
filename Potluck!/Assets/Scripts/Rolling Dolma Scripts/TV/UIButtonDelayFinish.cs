using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIButtonDelayFinish : MonoBehaviour
{
    [Header("Finish Settings")]
    public FinishBranchScene finishScene;

    [Header("Timer Settings")]
    public float delay = 30f;

    [Header("Fade Settings")]
    public Image fadePanel; // UI Image (je panel)
    public float fadeDuration = 1f;
    public float fadeCompleteBeforeEnd = 5f;

    [Header("Optional")]
    public bool disableButtonAfterClick = true;
    public GameObject buttonObject;

    [Header("Words")]
    public List<TextMeshProUGUI> words =
        new List<TextMeshProUGUI>();

    public float wordFadeDuration = 1f;
    public float timeBetweenWords = 4f;

    private int currentWordIndex = 0;

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

        StartCoroutine(WordSequence());

        StartCoroutine(TimerSequence());
    }

    IEnumerator TimerSequence()
    {
        // ⏱️ bereken wanneer fade moet starten
        float fadeStartTime =
            delay - fadeCompleteBeforeEnd - fadeDuration;

        // safety
        if (fadeStartTime < 0)
            fadeStartTime = 0;

        // 1. wacht tot fade moet beginnen
        yield return new WaitForSeconds(fadeStartTime);

        // 2. fade uitvoeren
        if (fadePanel != null)
        {
            yield return StartCoroutine(FadeToBlack());
        }

        // 3. wacht resterende tijd
        float remainingTime =
            delay - fadeStartTime - fadeDuration;

        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        // 4. scene switch
        if (finishScene != null)
        {
            finishScene.FinishScene();
        }
        else
        {
            Debug.LogWarning(
                "FinishScene script niet gekoppeld!"
            );
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

            // alpha van 0 → 1
            color.a = Mathf.Lerp(0f, 1f, t);

            fadePanel.color = color;

            yield return null;
        }

        color.a = 1f;
        fadePanel.color = color;
    }

    IEnumerator WordSequence()
    {
        // alles invisible maken + activeren
        foreach (TextMeshProUGUI word in words)
        {
            if (word != null)
            {
                word.gameObject.SetActive(true);

                Color c = word.color;
                c.a = 0f;
                word.color = c;
            }
        }

        while (currentWordIndex < words.Count)
        {
            TextMeshProUGUI currentWord =
                words[currentWordIndex];

            if (currentWord != null)
            {
                StartCoroutine(
                    FadeInWord(currentWord)
                );
            }

            currentWordIndex++;

            yield return new WaitForSeconds(
                timeBetweenWords
            );
        }
    }

    IEnumerator FadeInWord(TextMeshProUGUI word)
    {
        float timer = 0f;

        Color startColor = word.color;
        startColor.a = 0f;

        Color endColor = word.color;
        endColor.a = 1f;

        while (timer < wordFadeDuration)
        {
            timer += Time.deltaTime;

            word.color = Color.Lerp(
                startColor,
                endColor,
                timer / wordFadeDuration
            );

            yield return null;
        }

        word.color = endColor;
    }
}