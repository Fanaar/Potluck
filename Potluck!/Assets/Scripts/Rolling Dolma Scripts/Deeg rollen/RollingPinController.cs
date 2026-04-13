using UnityEngine;

public class RollingPinController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float followSpeed = 15f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    private bool isHorizontal = true; // onthoud huidige richting
    [SerializeField] private float directionThreshold = 5f;

    private Vector3 lastMousePosition;
    private Vector2 movementDelta;

    private float targetRotation = 0f;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        MoveWithMouse();
        CalculateMovement();
        UpdateRotation();
    }

    private void MoveWithMouse()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        transform.position = Vector3.Lerp(transform.position, mouseWorld, followSpeed * Time.deltaTime);
    }

    private void CalculateMovement()
    {
        Vector3 delta = Input.mousePosition - lastMousePosition;
        movementDelta = new Vector2(delta.x, delta.y);

        lastMousePosition = Input.mousePosition;
    }

    private void UpdateRotation()
    {
        if (movementDelta.magnitude < 0.01f) return;

        float absX = Mathf.Abs(movementDelta.x);
        float absY = Mathf.Abs(movementDelta.y);

        // alleen switchen als verschil groot genoeg is
        if (isHorizontal)
        {
            if (absY > absX + directionThreshold)
            {
                isHorizontal = false;
            }
        }
        else
        {
            if (absX > absY + directionThreshold)
            {
                isHorizontal = true;
            }
        }

        // rotatie bepalen
        if (isHorizontal)
            targetRotation = 90f;
        else
            targetRotation = 0f;

        float currentZ = transform.eulerAngles.z;
        float newZ = Mathf.LerpAngle(currentZ, targetRotation, Time.deltaTime * rotationSpeed);

        transform.rotation = Quaternion.Euler(0, 0, newZ);
    }

    public Vector2 GetMovementDelta()
    {
        return movementDelta;
    }
}