using UnityEngine;
using System.Collections;

public class DolmaRollController : MonoBehaviour
{
    public Sprite leafWithStuffing;
    public Sprite bottomFolded;
    public Sprite leftFolded;
    public Sprite rightFolded;
    public Sprite allFolded;

    public GameObject dolmaPickupObject;

    [Header("Question Endings")]
    public string leftEnding;
    public string rightEnding;

    [Header("Fade Settings")]
    public float fadeDuration = 0.15f;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    SpriteRenderer sr;
    Vector2 dragStart;

    int state = 0;
    bool isTransitioning = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (state != 1 || isTransitioning) return;

        Vector2 currentMouse = Input.mousePosition;
        Vector2 dir = currentMouse - dragStart;

        HandleHover(dir);
    }

    void OnMouseDown()
    {
        if (state >= 4 || isTransitioning) return;
        dragStart = Input.mousePosition;
    }

    void OnMouseUp()
    {
        if (state >= 4 || isTransitioning) return;

        Vector2 dragEnd = Input.mousePosition;
        Vector2 dir = dragEnd - dragStart;

        ProcessDrag(dir);
    }

    void ProcessDrag(Vector2 dir)
    {
        if (state >= 4 || isTransitioning) return;

        // STEP 1: bottom fold
        if (state == 0 && dir.y > 50)
        {
            StartCoroutine(CrossFadeToSprite(bottomFolded));
            state = 1;

            UIQuestionPreview.Instance.ShowEndingChoices(
                rightEnding,
                leftEnding
            );

            return;
        }

        // STEP 2: keuze links/rechts
        if (state == 1)
        {
            // LINKS (slepen naar links)
            if (dir.x < -50)
            {
                StartCoroutine(CrossFadeToSprite(leftFolded));
                state = 2;

                RoundManager.Instance.SetEndingIndex(0);
                GameState.Instance.lastChoice = 0;

                UIQuestionPreview.Instance.HideEndingChoices();
                UIQuestionPreview.Instance.LockQuestion();

                UIQuestionPreview.Instance.ShowFullQuestion(leftEnding);

                return;
            }

            // RECHTS (slepen naar rechts)
            if (dir.x > 50)
            {
                StartCoroutine(CrossFadeToSprite(rightFolded));
                state = 2;

                RoundManager.Instance.SetEndingIndex(1);
                GameState.Instance.lastChoice = 1;

                UIQuestionPreview.Instance.HideEndingChoices();
                UIQuestionPreview.Instance.LockQuestion();

                UIQuestionPreview.Instance.ShowFullQuestion(rightEnding);

                return;
            }
        }

        // STEP 3: dichtrollen
        if (state == 2)
        {
            StartCoroutine(CrossFadeToSprite(allFolded));
            state = 3;
            return;
        }

        // STEP 4: klaar → spawn dolma
        if (state == 3)
        {
            state = 4;
            StartCoroutine(FadeOutAndSpawnDolma());
        }
    }

    IEnumerator CrossFadeToSprite(Sprite newSprite)
    {
        isTransitioning = true;

        float t = 0;
        Color c = sr.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = t / fadeDuration;
            float curveValue = fadeCurve.Evaluate(normalized);

            c.a = Mathf.Lerp(1, 0, curveValue);
            sr.color = c;

            yield return null;
        }

        sr.sprite = newSprite;

        t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = t / fadeDuration;
            float curveValue = fadeCurve.Evaluate(normalized);

            c.a = Mathf.Lerp(0, 1, curveValue);
            sr.color = c;

            yield return null;
        }

        c.a = 1;
        sr.color = c;

        isTransitioning = false;
    }

    IEnumerator FadeOutAndSpawnDolma()
    {
        isTransitioning = true;

        float t = 0;
        Color c = sr.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = t / fadeDuration;
            float curveValue = fadeCurve.Evaluate(normalized);

            c.a = Mathf.Lerp(1, 0, curveValue);
            sr.color = c;

            yield return null;
        }

        // spawn dolma
        dolmaPickupObject.transform.position = transform.position;
        dolmaPickupObject.SetActive(true);

        gameObject.SetActive(false);
    }

    public void AddStuffing()
    {
        StartCoroutine(CrossFadeToSprite(leafWithStuffing));
    }

    void HandleHover(Vector2 dir)
    {
        if (dir.x < -30)
        {
            // hover LEFT
            UIQuestionPreview.Instance.PreviewLeft();
        }
        else if (dir.x > 30)
        {
            // hover RIGHT
            UIQuestionPreview.Instance.PreviewRight();
        }
        else
        {
            // neutraal (optioneel)
            UIQuestionPreview.Instance.PreviewNone();
        }
    }
}