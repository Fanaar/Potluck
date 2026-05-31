using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DelayedFadeActivator : MonoBehaviour
{
    [Header("Timing")]
    public float delay = 5f;

    [Header("Fade Speed")]
    public float fadeDuration = 1f;

    [Header("Objects To Activate")]
    public List<GameObject> objectsToActivate =
        new List<GameObject>();

    [Header("Objects To Deactivate")]
    public List<GameObject> objectsToDeactivate =
        new List<GameObject>();

    void OnEnable()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        // Wacht eerst
        yield return new WaitForSeconds(delay);

        // --------
        // FADE OUT ALLES TEGELIJK
        // --------

        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
            {
                SpriteRenderer sr =
                    obj.GetComponent<SpriteRenderer>();

                if (sr != null)
                {
                    StartCoroutine(
                        FadeOutAndDisable(sr)
                    );
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }

        // Wacht tot fade klaar is
        yield return new WaitForSeconds(fadeDuration);

        // --------
        // FADE IN ALLES TEGELIJK
        // --------

        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(true);

                SpriteRenderer sr =
                    obj.GetComponent<SpriteRenderer>();

                if (sr != null)
                {
                    StartCoroutine(
                        FadeIn(sr)
                    );
                }
            }
        }
    }

    IEnumerator FadeIn(SpriteRenderer sr)
    {
        Color c = sr.color;

        c.a = 0f;
        sr.color = c;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;

            c.a = Mathf.Lerp(0f, 1f, t);

            sr.color = c;

            yield return null;
        }

        c.a = 1f;
        sr.color = c;
    }

    IEnumerator FadeOutAndDisable(SpriteRenderer sr)
    {
        Color c = sr.color;

        float startAlpha = c.a;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;

            c.a = Mathf.Lerp(
                startAlpha,
                0f,
                t
            );

            sr.color = c;

            yield return null;
        }

        c.a = 0f;
        sr.color = c;

        sr.gameObject.SetActive(false);
    }
}