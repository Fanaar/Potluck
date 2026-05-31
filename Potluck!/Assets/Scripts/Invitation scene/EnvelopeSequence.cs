using UnityEngine;
using System.Collections;

public enum EnvelopeState
{
    Front,
    Back,
    Open,
    LetterPulled,
    Finished
}

public class EnvelopeSequence : MonoBehaviour
{
    [Header("Envelope States")]
    public GameObject frontView;
    public GameObject backView;
    public GameObject openEnvelope;

    [Header("Letter")]
    public LetterDrag letter;

    [Header("Fade")]
    public SpriteRenderer[] envelopeSprites;

    [Header("Transition")]
    public float stateFadeDuration = 0.35f;

    private EnvelopeState currentState = EnvelopeState.Front;

    private bool transitioning = false;

    private void Start()
    {
        ShowFront();

        // reset alle alpha's
        foreach (SpriteRenderer sr in envelopeSprites)
        {
            Color c = sr.color;
            c.a = 1f;
            sr.color = c;
        }

        letter.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (transitioning) return;

        switch (currentState)
        {
            case EnvelopeState.Front:

                StartCoroutine(
                    FadeTransition(
                        frontView,
                        backView,
                        EnvelopeState.Back
                    ));

                break;

            case EnvelopeState.Back:

                StartCoroutine(
                    FadeTransition(
                        backView,
                        openEnvelope,
                        EnvelopeState.Open
                    ));

                break;
        }
    }

    IEnumerator FadeTransition(
        GameObject from,
        GameObject to,
        EnvelopeState nextState)
    {
        transitioning = true;

        SpriteRenderer[] fromSprites =
            from.GetComponentsInChildren<SpriteRenderer>();

        SpriteRenderer[] toSprites =
            to.GetComponentsInChildren<SpriteRenderer>();

        // nieuwe sprites transparant maken
        foreach (SpriteRenderer sr in toSprites)
        {
            Color c = sr.color;
            c.a = 0;
            sr.color = c;
        }

        to.SetActive(true);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / stateFadeDuration;

            // fade out oude
            foreach (SpriteRenderer sr in fromSprites)
            {
                Color c = sr.color;
                c.a = 1 - t;
                sr.color = c;
            }

            // fade in nieuwe
            foreach (SpriteRenderer sr in toSprites)
            {
                Color c = sr.color;
                c.a = t;
                sr.color = c;
            }

            yield return null;
        }

        // final alpha fix
        foreach (SpriteRenderer sr in toSprites)
        {
            Color c = sr.color;
            c.a = 1f;
            sr.color = c;
        }

        from.SetActive(false);

        currentState = nextState;

        // kaart activeren wanneer open envelope verschijnt
        if (nextState == EnvelopeState.Open)
        {
            letter.gameObject.SetActive(true);

            letter.EnableDragging(this);
        }

        transitioning = false;
    }

    void ShowFront()
    {
        frontView.SetActive(true);
        backView.SetActive(false);
        openEnvelope.SetActive(false);
    }

    public void OnLetterPulledOut()
    {
        if (currentState == EnvelopeState.Open)
        {
            currentState = EnvelopeState.LetterPulled;

            StartCoroutine(FinishSequence());
        }
    }

    IEnumerator FinishSequence()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;

            float alpha = 1 - t;

            foreach (SpriteRenderer sr in envelopeSprites)
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }

            yield return null;
        }

        openEnvelope.SetActive(false);

        letter.MoveToCenter();

        currentState = EnvelopeState.Finished;
    }
}