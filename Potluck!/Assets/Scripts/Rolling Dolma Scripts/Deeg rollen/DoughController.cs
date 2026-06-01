using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DoughController : MonoBehaviour
{
    [Header("Scaling")]
    [SerializeField] private float minScale = 4f;
    [SerializeField] private float maxScale = 6f;
    [SerializeField] private float stretchSpeed = 2f;

    [Header("Finish")]
    [SerializeField] private float finishThreshold = 0.9f;
    [SerializeField] private float finishLerpSpeed = 2f;

    [Header("References")]
    [SerializeField] private RollingPinController rollingPin;

    [Header("Words")]
    [SerializeField]
    private List<TextMeshProUGUI> words =
        new List<TextMeshProUGUI>();

    [SerializeField] private float wordFadeDuration = 1f;

    private int currentWordIndex = 0;

    private float currentX;
    private float currentY;

    private bool isFinished = false;

    void Start()
    {
        currentX = minScale;
        currentY = minScale;

        transform.localScale = new Vector3(currentX, currentY, 1f);

        // woorden invisible maken
        foreach (TextMeshProUGUI word in words)
        {
            if (word != null)
            {
                Color c = word.color;
                c.a = 0f;
                word.color = c;
            }
        }
    }

    void Update()
    {
        if (isFinished)
        {
            // Smooth naar perfecte cirkel
            currentX = Mathf.Lerp(
                currentX,
                maxScale,
                Time.deltaTime * finishLerpSpeed
            );

            currentY = Mathf.Lerp(
                currentY,
                maxScale,
                Time.deltaTime * finishLerpSpeed
            );

            transform.localScale =
                new Vector3(currentX, currentY, 1f);
        }
    }

    public void Roll(Vector2 movement)
    {
        if (isFinished) return;
        if (movement.magnitude < 0.01f) return;

        // Richting bepalen
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            currentX += Mathf.Abs(movement.x)
                * stretchSpeed
                * Time.deltaTime;
        }
        else
        {
            currentY += Mathf.Abs(movement.y)
                * stretchSpeed
                * Time.deltaTime;
        }

        currentX = Mathf.Clamp(currentX, minScale, maxScale);
        currentY = Mathf.Clamp(currentY, minScale, maxScale);

        transform.localScale =
            new Vector3(currentX, currentY, 1f);

        // progress normalizen tussen minScale en maxScale
        float normalizedX =
            Mathf.InverseLerp(minScale, maxScale, currentX);

        float normalizedY =
            Mathf.InverseLerp(minScale, maxScale, currentY);

        float overallProgress =
            (normalizedX + normalizedY) / 2f;

        CheckWordProgress(overallProgress);

        // Check finish
        float xProgress = currentX / maxScale;
        float yProgress = currentY / maxScale;

        if (xProgress > finishThreshold
            && yProgress > finishThreshold)
        {
            isFinished = true;

            // force laatste woorden zichtbaar
            while (currentWordIndex < words.Count)
            {
                StartCoroutine(
                    FadeInWord(words[currentWordIndex])
                );

                currentWordIndex++;
            }

            if (rollingPin != null)
            {
                rollingPin.StartFadeOut();
            }
        }
    }

    void CheckWordProgress(float overallProgress)
    {
        if (currentWordIndex >= words.Count)
            return;

        float stepSize = 1f / words.Count;

        float requiredProgress =
            stepSize * (currentWordIndex + 1);

        if (overallProgress >= requiredProgress)
        {
            StartCoroutine(
                FadeInWord(words[currentWordIndex])
            );

            currentWordIndex++;
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