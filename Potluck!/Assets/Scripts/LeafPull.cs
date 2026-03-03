using UnityEngine;
using System.Collections;

public class LeafPull : MonoBehaviour
{
    [Header("Idle Breeze")]
    [SerializeField] private float idleSwayAngle = 4f;
    [SerializeField] private float idleSwaySpeed = 0.8f;

    [Header("Hover Reaction")]
    [SerializeField] private float hoverSwayAngle = 8f;
    [SerializeField] private float hoverSwaySpeed = 1.2f;
    [SerializeField] private float hoverSmoothTime = 0.2f;

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

    // Sway system
    private float swayTimer = 0f;
    private float currentAngle;
    private float angleVelocity;
    private float currentSpeed;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseRotationZ = transform.eulerAngles.z;

        // Randomize starting phase
        swayTimer = Random.Range(0f, 100f);

        // Prevent Z-fighting when overlapping leaves
        transform.position += Vector3.forward * Random.Range(-0.01f, 0.01f);

        currentSpeed = idleSwaySpeed;
        currentAngle = idleSwayAngle;
    }

    private void Update()
    {
        if (!isTriggered)
        {
            UpdateSway();
        }
    }

    private void UpdateSway()
    {
        float targetAngle = isHovering ? hoverSwayAngle : idleSwayAngle;
        float targetSpeed = isHovering ? hoverSwaySpeed : idleSwaySpeed;

        // Smooth amplitude transition
        currentAngle = Mathf.SmoothDamp(
            currentAngle,
            targetAngle,
            ref angleVelocity,
            hoverSmoothTime
        );

        // Smooth speed transition
        currentSpeed = Mathf.Lerp(
            currentSpeed,
            targetSpeed,
            Time.deltaTime * 2f
        );

        // Advance internal timer (fixes speed stacking issue)
        swayTimer += Time.deltaTime * currentSpeed;

        float sway =
            Mathf.Sin(swayTimer) * 0.7f +
            Mathf.Sin(swayTimer * 0.5f) * 0.3f;

        float finalAngle = baseRotationZ + sway * currentAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, finalAngle);
    }

    private void OnMouseEnter()
    {
        if (!isTriggered)
            isHovering = true;
    }

    private void OnMouseExit()
    {
        if (!isTriggered)
            isHovering = false;
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