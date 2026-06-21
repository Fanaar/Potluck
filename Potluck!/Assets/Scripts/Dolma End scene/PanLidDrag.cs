using UnityEngine;
using System.Collections;

public class PanLidDrag : MonoBehaviour
{
    [Header("Drag Settings")]
    public float dragSmoothness = 12f;
    public float liftHeight = 0.2f;

    [Header("Rotation")]
    public float dragRotation = -12f;
    public float rotationSmoothness = 8f;

    [Header("Return Settings")]
    public float returnSpeed = 8f;

    [Header("Fade Settings")]
    public float removeDistance = 2f;
    public float fadeDelay = 0.3f;
    public float fadeDuration = 1.5f;

    [Header("Steam")]
    public ParticleSystem steamParticles;
    private ParticleSystem.EmissionModule steamEmission;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Vector3 offset;

    private bool isDragging = false;
    private bool isReturning = false;
    private bool isFading = false;

    private Camera cam;
    private SpriteRenderer sr;

    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();

        originalPosition = transform.position;
        targetPosition = transform.position;

        if (steamParticles != null)
        {
            steamEmission = steamParticles.emission;
        }
    }

    void Update()
    {
        if (isDragging && !isFading)
        {
            DragObject();
        }

        if (isReturning)
        {
            ReturnToOriginalPosition();
        }
    }

    void DragObject()
    {
        Vector3 mouseWorld =
            cam.ScreenToWorldPoint(Input.mousePosition);

        mouseWorld.z = 0;

        targetPosition =
            mouseWorld + offset + Vector3.up * liftHeight;

        // Smooth movement
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            dragSmoothness * Time.deltaTime
        );

        // Smooth rotation
        Quaternion targetRot =
            Quaternion.Euler(0, 0, dragRotation);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRot,
            rotationSmoothness * Time.deltaTime
        );
    }

    void ReturnToOriginalPosition()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            originalPosition,
            returnSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.identity,
            rotationSmoothness * Time.deltaTime
        );

        // Stop returning when close enough
        if (Vector3.Distance(transform.position, originalPosition) < 0.01f)
        {
            transform.position = originalPosition;
            transform.rotation = Quaternion.identity;

            isReturning = false;

            // Resume steam emission
            if (steamParticles != null)
            {
                steamEmission.enabled = true;
            }
        }
    }

    void OnMouseEnter()
    {
        if (isFading) return;

        CustomCursorUI.Instance.SetHover();
    }

    void OnMouseExit()
    {
        if (isDragging) return;

        CustomCursorUI.Instance.SetDefault();
    }

    void OnMouseDown()
    {
        if (isFading) return;

        isDragging = true;
        isReturning = false;

        CustomCursorUI.Instance.SetGrab();

        Vector3 mouseWorld =
            cam.ScreenToWorldPoint(Input.mousePosition);

        mouseWorld.z = 0;

        offset = transform.position - mouseWorld;

        // Stop emitting new steam
        if (steamParticles != null)
        {
            steamEmission.enabled = false;
        }
    }

    void OnMouseUp()
    {
        if (isFading) return;

        isDragging = false;

        float distance =
            Vector3.Distance(transform.position, originalPosition);

        // FAR ENOUGH -> FADE OUT
        if (distance >= removeDistance)
        {
            CustomCursorUI.Instance.SetDefault();
            StartCoroutine(FadeOut());
        }
        else
        {
            CustomCursorUI.Instance.SetHover();

            // NOT FAR ENOUGH -> SNAP BACK
            isReturning = true;
        }
    }

    IEnumerator FadeOut()
    {
        isFading = true;

        // Stop steam permanently
        if (steamParticles != null)
        {
            steamEmission.enabled = false;
        }

        yield return new WaitForSeconds(fadeDelay);

        float timer = 0f;

        Color startColor = sr.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float t = timer / fadeDuration;

            Color newColor = startColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);

            sr.color = newColor;

            yield return null;
        }

        if (KitchenState.Instance != null)
        {
            KitchenState.Instance.lidRemoved = true;
            KitchenState.Instance.CheckCompletion();
        }

        Destroy(gameObject);
    }
}