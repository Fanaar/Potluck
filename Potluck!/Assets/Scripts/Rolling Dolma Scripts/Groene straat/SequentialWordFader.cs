using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SequentialWordFader : MonoBehaviour
{
    [System.Serializable]
    public class FadeWord
    {
        public TextMeshProUGUI text;
    }

    [Header("Words In Order")]
    public List<FadeWord> words =
        new List<FadeWord>();

    public float fadeDuration = 1f;

    private int currentIndex = 0;

    void Start()
    {
        // alles invisible maken
        foreach (FadeWord word in words)
        {
            if (word.text != null)
            {
                Color c = word.text.color;
                c.a = 0f;
                word.text.color = c;
            }
        }
    }

    public void FadeNextWord()
    {
        if (currentIndex >= words.Count)
            return;

        FadeWord word = words[currentIndex];

        if (word.text != null)
        {
            StartCoroutine(FadeIn(word.text));
        }

        currentIndex++;
    }

    IEnumerator FadeIn(TextMeshProUGUI tmp)
    {
        float timer = 0f;

        Color startColor = tmp.color;
        startColor.a = 0f;

        Color endColor = tmp.color;
        endColor.a = 1f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            tmp.color = Color.Lerp(
                startColor,
                endColor,
                timer / fadeDuration
            );

            yield return null;
        }

        tmp.color = endColor;
    }
}