using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeUIOnClick : MonoBehaviour
{
    [Header("Text Elements")]
    public TextMeshProUGUI[] texts;

    [Header("Background Image")]
    public Image backgroundImage;

    [Header("Fade Settings")]
    public float textFadeDuration = 1f;
    public float imageFadeDuration = 1f;

    public Button buttonToDisable;

    public void StartFade()
    {
        if (buttonToDisable != null)
        {
            buttonToDisable.gameObject.SetActive(false);
        }

        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Fade alle teksten tegelijk
        float timer = 0f;

        Color[] startColors = new Color[texts.Length];
        for (int i = 0; i < texts.Length; i++)
        {
            startColors[i] = texts[i].color;
        }

        while (timer < textFadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / textFadeDuration;

            for (int i = 0; i < texts.Length; i++)
            {
                Color c = startColors[i];
                c.a = Mathf.Lerp(startColors[i].a, 0f, t);
                texts[i].color = c;
            }

            yield return null;
        }

        // Daarna image faden
        timer = 0f;
        Color imageColor = backgroundImage.color;

        while (timer < imageFadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / imageFadeDuration;

            Color c = imageColor;
            c.a = Mathf.Lerp(imageColor.a, 0f, t);
            backgroundImage.color = c;

            yield return null;
        }
    }
}