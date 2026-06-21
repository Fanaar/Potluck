using UnityEngine;
using System.Collections;

public class PanSnapDrag : MonoBehaviour
{
    [Header("Drag Settings")]
    public float dragSmoothness = 12f;
    public float liftHeight = 0.2f;

    [Header("Rotation")]
    public float dragRotation = -5f;
    public float rotationSmoothness = 8f;

    [Header("Snap Settings")]
    public Transform snapPoint;
    public float snapDistance = 1.5f;
    public float snapDuration = 0.25f;

    [Header("Pan State")]
    public bool countAsPanRemoved = true;
    private Camera cam;

    private Vector3 offset;
    private Vector3 targetPosition;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isDragging = false;
    private bool isMoving = false;
    private bool isSnapped = false;

    void Start()
    {
        cam = Camera.main;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isDragging && !isMoving && !isSnapped)
        {
            DragObject();
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

        // Slight drag rotation
        Quaternion targetRot =
            Quaternion.Euler(0, 0, dragRotation);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRot,
            rotationSmoothness * Time.deltaTime
        );
    }

    void OnMouseEnter()
    {
        if (isMoving || isSnapped) return;

        CustomCursorUI.Instance.SetHover();
    }

    void OnMouseExit()
    {
        if (isDragging) return;

        CustomCursorUI.Instance.SetDefault();
    }

    void OnMouseDown()
    {
        if (isMoving || isSnapped) return;

        isDragging = true;

        CustomCursorUI.Instance.SetGrab();

        Vector3 mouseWorld =
            cam.ScreenToWorldPoint(Input.mousePosition);

        mouseWorld.z = 0;

        offset = transform.position - mouseWorld;
    }

    void OnMouseUp()
    {
        if (isMoving || isSnapped) return;

        isDragging = false;

        CustomCursorUI.Instance.SetHover();

        TrySnap();
    }

    void TrySnap()
    {
        if (snapPoint == null)
        {
            ReturnToOriginal();
            return;
        }

        float distance =
            Vector3.Distance(transform.position, snapPoint.position);

        if (distance <= snapDistance)
        {
            StartCoroutine(MoveToPosition(
                snapPoint.position,
                Quaternion.identity,
                true
            ));
        }
        else
        {
            ReturnToOriginal();
        }
    }

    void ReturnToOriginal()
    {
        StartCoroutine(MoveToPosition(
            originalPosition,
            originalRotation,
            false
        ));
    }

    IEnumerator MoveToPosition(
        Vector3 targetPos,
        Quaternion targetRot,
        bool snappedState
    )
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Quaternion startRotation = transform.rotation;

        float timer = 0f;

        while (timer < snapDuration)
        {
            timer += Time.deltaTime;

            float t = timer / snapDuration;

            transform.position = Vector3.Lerp(
                startPos,
                targetPos,
                t
            );

            transform.rotation = Quaternion.Lerp(
                startRotation,
                targetRot,
                t
            );

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        isSnapped = snappedState;

        if (snappedState && countAsPanRemoved)
        {
            if (KitchenState.Instance != null)
            {
                KitchenState.Instance.panRemoved = true;
                KitchenState.Instance.CheckCompletion();
            }
        }

        isMoving = false;
    }
}