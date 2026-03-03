using UnityEngine;
using System.Collections;

public class LeafPull : MonoBehaviour
{
    [Header("Idle Breeze")]
    [SerializeField] private float idleSwayAngle = 4f;
    [SerializeField] private float idleSwaySpeed = 0.8f;

    [Header("Hover Reaction")]
    [SerializeField] private float hoverSwayMultiplier = 2f;
    [SerializeField] private float hoverSpeedMultiplier = 1.5f;
    [SerializeField] private float hoverBlendSpeed = 3f;
    [SerializeField] private float hoverStabilityTime = 0.05f;

    [Header("Click Wiggle")]
    [SerializeField] private float wiggleAngle = 15f;
    [SerializeField] private float wiggleSpeed = 20f;
    [SerializeField] private float wiggleDuration = 0.5f;

    [Header("Fall Settings")]
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private float horizontalDrift = 1f;
    [SerializeField] private float rotationSpeed = 180f;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;

    private bool isTriggered = false;
    private bool isHovering = false;

    private SpriteRenderer sr;

    private float baseRotationZ;
    private float driftDirection;
    private float spinDirection;
    private float idleTimeOffset;

    private float hoverInfluence = 0f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseRotationZ = transform.eulerAngles.z;

        idleTimeOffset = Random.Range(0f, 100f);

        // Prevent Z-fighting when leaves overlap
        transform.position += Vector3.forward * Random.Range(-0.01f, 0.01f);
    }

    private void Update()
    {
        if (!isTriggered)
        {
            UpdateHoverBlend();
            IdleSway();
        }
    }

    private void UpdateHoverBlend()
    {
        float target = isHovering ? 1f : 0f;

        hoverInfluence = Mathf.Lerp(
            hoverInfluence,
            target,
            Time.deltaTime * hoverBlendSpeed
        );
    }

    private void IdleSway()
    {
        float blendedAngle = Mathf.Lerp(
            idleSwayAngle,
            idleSwayAngle * hoverSwayMultiplier,
            hoverInfluence
        );

        float blendedSpeed = Mathf.Lerp(
            idleSwaySpeed,
            idleSwaySpeed * hoverSpeedMultiplier,
            hoverInfluence
        );

        float t = Time.time * blendedSpeed + idleTimeOffset;

        float sway =
            Mathf.Sin(t) * 0.7f +
            Mathf.Sin(t * 0.5f) * 0.3f;

        float finalAngle = baseRotationZ + sway * blendedAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, finalAngle);
    }

    private void OnMouseEnter()
    {
        if (!isTriggered)
        {
            isHovering = true;
        }
    }

    private void OnMouseExit()
    {
        if (!isTriggered)
        {
            StartCoroutine(HoverExitDelay());
        }
    }

    private IEnumerator HoverExitDelay()
    {
        yield return new WaitForSeconds(hoverStabilityTime);

        if (!IsMouseOverThis())
        {
            isHovering = false;
        }
    }

    private bool IsMouseOverThis()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D col = GetComponent<Collider2D>();
        return col != null && col.OverlapPoint(mousePos);
    }

    private void OnMouseDown()
    {
        if (!isTriggered)
        {
            isTriggered = true;

            baseRotationZ = transform.eulerAngles.z;

            driftDirection = Random.Range(-1f, 1f);
            spinDirection = Random.Range(-1f, 1f);

            StartCoroutine(WiggleFallFade());
        }
    }

    private IEnumerator WiggleFallFade()
    {
        float timer = 0f;

        while (timer < wiggleDuration)
        {
            float normalized = timer / wiggleDuration;
            float envelope = Mathf.Sin(normalized * Mathf.PI);

            float angleOffset =
                Mathf.Sin(timer * wiggleSpeed) *
                wiggleAngle *
                envelope;

            transform.rotation = Quaternion.Euler(0f, 0f, baseRotationZ + angleOffset);

            timer += Time.deltaTime;
            yield return null;
        }

        float fadeTimer = 0f;
        Color startColor = sr.color;

        while (fadeTimer < fadeDuration)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            transform.position += Vector3.right * driftDirection * horizontalDrift * Time.deltaTime;
            transform.Rotate(0f, 0f, spinDirection * rotationSpeed * Time.deltaTime);

            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            fadeTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}