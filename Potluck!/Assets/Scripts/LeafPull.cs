using UnityEngine;
using System.Collections;

public class LeafPull : MonoBehaviour
{
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

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (!isTriggered)
        {
            isTriggered = true;

            // store current rotation so wiggle is relative
            baseRotationZ = transform.eulerAngles.z;

            // random fall behaviour
            driftDirection = Random.Range(-1f, 1f);
            spinDirection = Random.Range(-1f, 1f);

            StartCoroutine(WiggleFallFade());
        }
    }

    private IEnumerator WiggleFallFade()
    {
        // --- WIGGLE RELATIVE TO CURRENT ROTATION ---
        float timer = 0f;
        while (timer < wiggleDuration)
        {
            float angleOffset = Mathf.Sin(timer * wiggleSpeed) * wiggleAngle;
            transform.rotation = Quaternion.Euler(0f, 0f, baseRotationZ + angleOffset);

            timer += Time.deltaTime;
            yield return null;
        }

        // return smoothly to original rotation
        transform.rotation = Quaternion.Euler(0f, 0f, baseRotationZ);

        // --- FALL + RANDOM DRIFT + RANDOM SPIN + FADE ---
        float fadeTimer = 0f;
        Color startColor = sr.color;

        while (fadeTimer < fadeDuration)
        {
            // fall down
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            // drift sideways
            transform.position += Vector3.right * driftDirection * horizontalDrift * Time.deltaTime;

            // spin while falling
            transform.Rotate(0f, 0f, spinDirection * rotationSpeed * Time.deltaTime);

            // fade out
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            fadeTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}