using UnityEngine;
using System.Collections;

public class TriggerFadeSwitcher : MonoBehaviour
{
    [Header("Objects")]
    public SpriteRenderer objectToFadeOut;
    public SpriteRenderer objectToFadeIn;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    [Header("Hover Settings")]
    public float hoverFadeSpeed = 5f;
    public float hoverDarkenAmount = 0.7f; // 1 = normaal, lager = donkerder

    [Header("Progress Manager")]
    public TriggerProgressManager progressManager;

    [Header("Sequential Word Fader")]
    public SequentialWordFader wordFader;

    private bool hasTriggered = false;
    private bool isHovering = false;

    private Color originalColor;

    void Start()
    {
        if (objectToFadeOut != null)
        {
            originalColor = objectToFadeOut.color;
        }
    }

    void Update()
    {
        if (hasTriggered || objectToFadeOut == null) return;

        // 👇 target kleur (alleen RGB aanpassen, alpha behouden)
        Color targetColor = originalColor;

        if (isHovering)
        {
            targetColor.r *= hoverDarkenAmount;
            targetColor.g *= hoverDarkenAmount;
            targetColor.b *= hoverDarkenAmount;
        }

        targetColor.a = objectToFadeOut.color.a; // alpha NIET aanpassen

        objectToFadeOut.color = Color.Lerp(
            objectToFadeOut.color,
            targetColor,
            Time.deltaTime * hoverFadeSpeed
        );
    }

    private void OnMouseEnter()
    {
        if (hasTriggered) return;
        isHovering = true;
    }

    private void OnMouseExit()
    {
        if (hasTriggered) return;
        isHovering = false;
    }

    private void OnMouseDown()
    {
        if (hasTriggered) return;

        hasTriggered = true;
        isHovering = false;

        StartCoroutine(FadeSequence());

        if (progressManager != null)
        {
            progressManager.RegisterTrigger();
        }

        if (wordFader != null)
        {
            wordFader.FadeNextWord();
        }
    }

    IEnumerator FadeSequence()
    {
        float time = 0;

        Color outColor = objectToFadeOut.color;
        Color inColor = objectToFadeIn.color;

        inColor.a = 0;
        objectToFadeIn.color = inColor;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            // Fade out (alleen alpha)
            outColor.a = Mathf.Lerp(1, 0, t);
            objectToFadeOut.color = outColor;

            // Fade in
            inColor.a = Mathf.Lerp(0, 1, t);
            objectToFadeIn.color = inColor;

            yield return null;
        }

        outColor.a = 0;
        inColor.a = 1;

        objectToFadeOut.color = outColor;
        objectToFadeIn.color = inColor;
    }
}