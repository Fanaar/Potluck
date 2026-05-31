using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CorrectEnvelope : MonoBehaviour
{
    [Header("Objects To Activate")]
    public List<GameObject> objectsToActivate =
        new List<GameObject>();

    [Header("Objects To Move Down")]
    public List<Transform> objectsToMoveDown =
        new List<Transform>();

    [Header("Animators To Start")]
    public List<Animator> animatorsToStart =
        new List<Animator>();

    [Header("Move Settings")]
    public float moveDownAmount = 2f;
    public float moveSpeed = 4f;

    [Header("Random Object Speed")]
    public float minObjectSpeed = 0.6f;
    public float maxObjectSpeed = 1.4f;

    [Header("Fade Settings")]
    public float fadeSpeed = 4f;

    private bool clicked = false;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if (clicked)
            return;

        clicked = true;

        // Activeer objects
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }

        StartCoroutine(MoveAndFade());
    }

    IEnumerator MoveAndFade()
    {
        // Disable EnvelopeSwish scripts
        foreach (Transform obj in objectsToMoveDown)
        {
            if (obj != null)
            {
                EnvelopeSwish swish =
                    obj.GetComponent<EnvelopeSwish>();

                if (swish != null)
                {
                    swish.enabled = false;
                }
            }
        }

        // Beginposities opslaan
        List<Vector3> startPositions =
            new List<Vector3>();

        foreach (Transform obj in objectsToMoveDown)
        {
            startPositions.Add(obj.position);
        }

        // Random snelheden genereren
        List<float> randomSpeeds =
            new List<float>();

        for (int i = 0; i < objectsToMoveDown.Count; i++)
        {
            randomSpeeds.Add(
                Random.Range(
                    minObjectSpeed,
                    maxObjectSpeed
                )
            );
        }

        Vector3 envelopeStartPos =
            transform.position;

        Vector3 envelopeTargetPos =
            envelopeStartPos +
            Vector3.down * moveDownAmount;

        Color startColor = sr.color;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;

            float smoothT =
                Mathf.SmoothStep(0f, 1f, t);

            // Envelope bewegen
            transform.position = Vector3.Lerp(
                envelopeStartPos,
                envelopeTargetPos,
                smoothT
            );

            // Andere objects bewegen
            for (int i = 0; i < objectsToMoveDown.Count; i++)
            {
                if (objectsToMoveDown[i] != null)
                {
                    Vector3 targetPos =
                        startPositions[i] +
                        Vector3.down * moveDownAmount;

                    float individualT =
                        Mathf.Clamp01(
                            smoothT * randomSpeeds[i]
                        );

                    objectsToMoveDown[i].position =
                        Vector3.Lerp(
                            startPositions[i],
                            targetPos,
                            individualT
                        );
                }
            }

            // Envelope fade out
            Color c = startColor;

            c.a = Mathf.Lerp(
                1f,
                0f,
                smoothT
            );

            sr.color = c;

            yield return null;
        }

        // Start animators
        foreach (Animator anim in animatorsToStart)
        {
            if (anim != null)
            {
                anim.enabled = true;
            }
        }

        // Andere objects uitzetten
        foreach (Transform obj in objectsToMoveDown)
        {
            if (obj != null)
            {
                obj.gameObject.SetActive(false);
            }
        }

        // Envelope uitzetten
        gameObject.SetActive(false);
    }
}