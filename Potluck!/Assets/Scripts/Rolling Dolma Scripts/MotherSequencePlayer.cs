using UnityEngine;
using System.Collections;
using TMPro;

public class MotherSequencePlayer : MonoBehaviour
{
    public static MotherSequencePlayer Instance;

    [Header("References")]
    public SpriteRenderer motherRenderer;
    public TextMeshProUGUI dialogueText;

    [Header("Timing")]
    public float timeBetweenLines = 2.5f;
    public float fadeDuration = 0.3f;
    public float startDelay = 1f;

    void Awake()
    {
        Instance = this;
        dialogueText.gameObject.SetActive(false);
    }

    public void PlaySequence(MotherLine[] lines) // ✅ FIX
    {
        StopAllCoroutines();
        StartCoroutine(Play(lines));
    }

    IEnumerator Play(MotherLine[] lines) // ✅ FIX
    {
        CameraPanController.Instance.PanToMother();

        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < lines.Length; i++)
        {
            yield return StartCoroutine(PlayLine(lines[i]));
        }

        dialogueText.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        RoundManager.Instance.NextRound();
    }

    IEnumerator PlayLine(MotherLine line) // ✅ FIX
    {
        yield return StartCoroutine(Fade(1, 0));

        if (line.sprite != null)
            motherRenderer.sprite = line.sprite;

        yield return StartCoroutine(Fade(0, 1));

        dialogueText.gameObject.SetActive(true);
        dialogueText.text = line.text;

        yield return new WaitForSeconds(timeBetweenLines);
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0;
        Color c = motherRenderer.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / fadeDuration);
            c.a = a;
            motherRenderer.color = c;
            yield return null;
        }

        c.a = to;
        motherRenderer.color = c;
    }
}