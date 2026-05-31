using UnityEngine;
using System.Collections;

public class EnvelopeSwish : MonoBehaviour
{
    [Header("Movement")]
    public float moveAmount = 0.12f;

    [Header("Rotation")]
    public float rotationAmount = 3f;

    [Header("Animation")]
    public float smoothSpeed = 8f;

    [Header("Scale")]
    public float hoverScale = 1.03f;

    [Header("Fade")]
    public float fadeSpeed = 12f;

    [Header("Sorting")]
    public int normalSortingOrder = 5;
    public int hoverSortingOrder = 8;

    [Header("Hover Cooldown")]
    public float hoverCooldown = 0.12f;

    private SpriteRenderer sr;

    private Vector3 targetPos;
    private Quaternion targetRot;
    private Vector3 targetScale;

    private Vector3 velocity;

    private Vector3 originalScale;

    private bool isHovered;
    private bool isFading;

    private float lastHoverTime;

    private float targetAlpha = 1f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        originalScale = transform.localScale;

        targetPos = transform.localPosition;
        targetRot = transform.localRotation;
        targetScale = originalScale;

        Color c = sr.color;
        c.a = 1f;
        sr.color = c;
    }

    void Update()
    {
        CheckHover();

        // Smooth movement
        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetPos,
            ref velocity,
            1f / smoothSpeed
        );

        // Smooth rotation
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetRot,
            Time.deltaTime * smoothSpeed
        );

        // Smooth scale
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * smoothSpeed
        );

        // Smooth alpha
        Color c = sr.color;

        c.a = Mathf.Lerp(
            c.a,
            targetAlpha,
            Time.deltaTime * fadeSpeed
        );

        sr.color = c;
    }

    void CheckHover()
    {
        Vector3 mouseWorld =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 mousePos2D = new Vector2(
            mouseWorld.x,
            mouseWorld.y
        );

        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos2D);

        EnvelopeSwish topEnvelope = null;
        int highestSortingOrder = -999;

        // Zoek envelope met hoogste sorting order
        foreach (Collider2D hit in hits)
        {
            EnvelopeSwish envelope =
                hit.GetComponent<EnvelopeSwish>();

            if (envelope != null)
            {
                SpriteRenderer hitSR =
                    envelope.GetComponent<SpriteRenderer>();

                if (hitSR.sortingOrder > highestSortingOrder)
                {
                    highestSortingOrder =
                        hitSR.sortingOrder;

                    topEnvelope = envelope;
                }
            }
        }

        bool hoveringThis = topEnvelope == this;

        // ENTER
        if (
            hoveringThis &&
            !isHovered &&
            !isFading &&
            Time.time > lastHoverTime + hoverCooldown
        )
        {
            lastHoverTime = Time.time;

            isHovered = true;

            StartCoroutine(FadeAndBringToFront());

            // Random beweging
            Vector3 randomOffset = new Vector3(
                Random.Range(-moveAmount, moveAmount),
                Random.Range(-moveAmount * 0.4f, moveAmount * 0.4f),
                0f
            );

            // Random rotatie
            float randomRot = Random.Range(
                -rotationAmount,
                rotationAmount
            );

            targetPos = transform.localPosition + randomOffset;

            targetRot = Quaternion.Euler(
                0f,
                0f,
                transform.localEulerAngles.z + randomRot
            );

            targetScale = originalScale * hoverScale;
        }

        // EXIT
        else if (!hoveringThis && isHovered)
        {
            isHovered = false;

            targetScale = originalScale;
        }
    }

    IEnumerator FadeAndBringToFront()
    {
        isFading = true;

        // Fade OUT
        targetAlpha = 0f;

        yield return new WaitForSeconds(0.08f);

        // Reset alle enveloppen
        EnvelopeSwish[] allEnvelopes =
            FindObjectsOfType<EnvelopeSwish>();

        foreach (EnvelopeSwish envelope in allEnvelopes)
        {
            SpriteRenderer otherSR =
                envelope.GetComponent<SpriteRenderer>();

            otherSR.sortingOrder = normalSortingOrder;
        }

        // Deze bovenop
        sr.sortingOrder = hoverSortingOrder;

        // Fade IN
        targetAlpha = 1f;

        yield return new WaitForSeconds(0.08f);

        isFading = false;
    }
}