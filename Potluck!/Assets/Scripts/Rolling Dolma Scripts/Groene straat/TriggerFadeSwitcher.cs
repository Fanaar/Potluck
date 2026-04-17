using UnityEngine;
using System.Collections;

public class TriggerFadeSwitcher : MonoBehaviour
{
    [Header("Objects")]
    public SpriteRenderer objectToFadeOut;
    public SpriteRenderer objectToFadeIn;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    [Header("Progress Manager")]
    public TriggerProgressManager progressManager;

    private bool hasTriggered = false;

    private void OnMouseDown()
    {
        if (hasTriggered) return;

        hasTriggered = true;

        StartCoroutine(FadeSequence());

        if (progressManager != null)
        {
            progressManager.RegisterTrigger();
        }
    }

    IEnumerator FadeSequence()
    {
        float time = 0;

        Color outColor = objectToFadeOut.color;
        Color inColor = objectToFadeIn.color;

        // Zorg dat de "in" object start op alpha 0
        inColor.a = 0;
        objectToFadeIn.color = inColor;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            // Fade out
            outColor.a = Mathf.Lerp(1, 0, t);
            objectToFadeOut.color = outColor;

            // Fade in
            inColor.a = Mathf.Lerp(0, 1, t);
            objectToFadeIn.color = inColor;

            yield return null;
        }

        // Zeker zetten
        outColor.a = 0;
        inColor.a = 1;

        objectToFadeOut.color = outColor;
        objectToFadeIn.color = inColor;
    }
}