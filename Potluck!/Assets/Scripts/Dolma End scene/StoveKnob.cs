using UnityEngine;

public class StoveKnob : MonoBehaviour
{
    [Header("Rotation Limits")]
    public float minRotation = 0f;
    public float maxRotation = 180f;

    [Header("Turn Off")]
    public float turnOffThreshold = 10f;
    public GameObject stoveOnObject;

    [Header("Rotation Feel")]
    public float rotationSpeed = 5f;

    [Header("Cinematic")]
    public StoveCinematic cinematic;

    private bool isDragging = false;
    private bool isOff = false;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (isDragging && !isOff)
        {
            RotateKnob();
        }
    }

    void RotateKnob()
    {
        Vector3 mousePos =
            cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction =
            mousePos - transform.position;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle -= 90f;

        if (angle < 0)
        {
            angle += 360f;
        }

        // Clamp rotation
        angle = Mathf.Clamp(angle, minRotation, maxRotation);

        Quaternion targetRotation =
            Quaternion.Euler(0, 0, angle);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        if (angle <= turnOffThreshold)
        {
            isOff = true;

            stoveOnObject.SetActive(false);

            transform.rotation =
                Quaternion.Euler(0, 0, 0);

            // START CINEMATIC
            if (cinematic != null)
            {
                cinematic.StartCinematic();
            }
        }
    }

    void OnMouseDown()
    {
        if (isOff) return;

        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}