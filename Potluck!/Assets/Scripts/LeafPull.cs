using UnityEngine;
using System.Collections;

public class LeafPull : MonoBehaviour
{
    [Header("Idle Breeze (Always On)")]
    [SerializeField] private float idleSwayAngle = 4f;
    [SerializeField] private float idleSwaySpeed = 0.8f;

    [Header("Wiggle Settings")]
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
    private SpriteRenderer sr;

    private float baseRotationZ;
    private float driftDirection;
    private float spinDirection;

    private float idleTimeOffset; // makes each leaf sway differently

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseRotationZ = transform.eulerAngles.z;

        // random phase so all leaves don't move in sync
        idleTimeOffset = Random.Range(0f, 100f);
    }

    private void Update()
    {
        if (!isTriggered)
        {
            IdleSway();
        }
    }

    private void IdleSway()
    {
        // smooth easing sway (organic breeze)
        float t = Time.time * idleSwaySpeed + idleTimeOffset;

        // layered sine for more natural motion
        float sway =
            Mathf.Sin(t) * 0.7f +
            Mathf.Sin(t * 0.5f) * 0.3f;

        float angle = baseRotationZ + sway * idleSwayAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnMouseDown()
    {
        if (!isTriggered)
        {
            isTriggered = true;

            // capture current rotation so wiggle continues smoothly
            baseRotationZ = transform.eulerAngles.z;

            // random fall behaviour
            driftDirection = Random.Range(-1f, 1f);
            spinDirection = Random.Range(-1f, 1f);

            StartCoroutine(WiggleFallFade());
        }
    }

    private IEnumerator WiggleFallFade()
    {
        // --- STRONG WIGGLE BEFORE RELEASE ---
        float timer = 0f;
        while (timer < wiggleDuration)
        {
            float normalized = timer / wiggleDuration;

            // easing envelope (strong in middle, soft start/end)
            float envelope = Mathf.Sin(normalized * Mathf.PI);

            float angleOffset =
                Mathf.Sin(timer * wiggleSpeed) *
                wiggleAngle *
                envelope;

            transform.rotation = Quaternion.Euler(0f, 0f, baseRotationZ + angleOffset);

            timer += Time.deltaTime;
            yield return null;
        }

        // --- FALL + DRIFT + SPIN + FADE ---
        float fadeTimer = 0f;
        Color startColor = sr.color;

        while (fadeTimer < fadeDuration)
        {
            // fall
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            // sideways drift
            transform.position += Vector3.right * driftDirection * horizontalDrift * Time.deltaTime;

            // spin
            transform.Rotate(0f, 0f, spinDirection * rotationSpeed * Time.deltaTime);

            // fade
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            fadeTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}