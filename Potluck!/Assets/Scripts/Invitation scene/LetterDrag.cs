using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LetterDrag : MonoBehaviour
{
    private bool canDrag = false;
    private bool dragging = false;

    private bool canOpenScene = false;
    private bool movingToCenter = false;

    private Vector3 offset;

    private EnvelopeSequence sequence;

    [Header("Drag")]
    public float pullThreshold = 2f;

    [Header("Move To Center")]
    public float centerMoveDuration = 1f;

    [Header("Scene Transition")]
    public CanvasGroup fadePanel;
    public string nextSceneName;
    public float fadeDuration = 1f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;

        // zorg dat fade panel onzichtbaar start
        if (fadePanel != null)
        {
            fadePanel.alpha = 0;
        }
    }

    public void EnableDragging(EnvelopeSequence seq)
    {
        canDrag = true;

        sequence = seq;
    }

    private void OnMouseDown()
    {
        // wanneer kaart in midden staat -> scene openen
        if (canOpenScene)
        {
            StartCoroutine(FadeAndLoadScene());
            return;
        }

        // drag starten
        if (!canDrag) return;

        dragging = true;

        Vector3 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        offset =
            transform.position -
            new Vector3(mouse.x, mouse.y, transform.position.z);
    }

    private void OnMouseUp()
    {
        dragging = false;
    }

    private void Update()
    {
        if (!dragging) return;

        Vector3 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 target =
            new Vector3(mouse.x, mouse.y, transform.position.z) + offset;

        // alleen verticale beweging
        target.x = startPos.x;

        transform.position = target;

        float distance = transform.position.y - startPos.y;

        if (distance > pullThreshold)
        {
            transform.SetParent(null);

            canDrag = false;
            dragging = false;

            sequence.OnLetterPulledOut();
        }
    }

    public void MoveToCenter()
    {
        if (!movingToCenter)
        {
            StartCoroutine(MoveRoutine());
        }
    }

    IEnumerator MoveRoutine()
    {
        movingToCenter = true;

        Vector3 start = transform.position;

        Vector3 end =
            Camera.main.ViewportToWorldPoint(
                new Vector3(0.5f, 0.5f, 10f));

        end.z = 0;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / centerMoveDuration;

            transform.position =
                Vector3.Lerp(start, end, t);

            yield return null;
        }

        transform.position = end;

        movingToCenter = false;

        // speler mag nu klikken om verder te gaan
        canOpenScene = true;
    }

    IEnumerator FadeAndLoadScene()
    {
        canOpenScene = false;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;

            fadePanel.alpha = t;

            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}