using UnityEngine;
using System.Collections;

public class FadeInSpriteOnTrigger : MonoBehaviour
{
    public SpriteRenderer targetSprite; // Sleep je sprite hierheen
    public float fadeDuration = 2f;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float time = 0f;
        Color color = targetSprite.color;
        color.a = 0f;
        targetSprite.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            targetSprite.color = color;
            yield return null;
        }

        color.a = 1f;
        targetSprite.color = color;
    }
}