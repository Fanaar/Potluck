using UnityEngine;

public class RollingPinController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float followSpeed = 15f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float directionThreshold = 5f;

    [Header("Fade Out")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float fadeSpeed = 2f;

    [SerializeField] private FinishBranchScene finishBranchScene;
    [SerializeField] private float delayAfterFade = 2f;

    private Vector3 lastMousePosition;
    private Vector2 movementDelta;

    private float targetRotation = 0f;

    private bool isPickedUp = false;
    private bool isHorizontal = true;
    private bool isFading = false;
    private HoverHighlight hoverHighlight;
    void Start()
    {
        lastMousePosition = Input.mousePosition;
        hoverHighlight = GetComponent<HoverHighlight>();
    }

    void Update()
    {
        HandlePickup();

        if (isFading)
        {
            FadeOut();
            return;
        }

        if (!isPickedUp) return;

        MoveWithMouse();
        CalculateMovement();
        UpdateRotation();
    }

    private void HandlePickup()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);

            if (hit != null && hit.gameObject == gameObject)
            {
                isPickedUp = true;

                // 👇 Hover UIT
                if (hoverHighlight != null)
                    hoverHighlight.DisableHover();

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        // Rechtermuisknop = loslaten (optioneel)
        if (Input.GetMouseButtonDown(1))
        {
            isPickedUp = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
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

        if (isHorizontal)
        {
            if (absY > absX + directionThreshold)
                isHorizontal = false;
        }
        else
        {
            if (absX > absY + directionThreshold)
                isHorizontal = true;
        }

        targetRotation = isHorizontal ? 90f : 0f;

        float currentZ = transform.eulerAngles.z;
        float newZ = Mathf.LerpAngle(currentZ, targetRotation, Time.deltaTime * rotationSpeed);

        transform.rotation = Quaternion.Euler(0, 0, newZ);
    }

    private bool hasTriggeredFinish = false;

    private void FadeOut()
    {
        Color c = spriteRenderer.color;
        c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * fadeSpeed);
        spriteRenderer.color = c;

        if (c.a < 0.05f && !hasTriggeredFinish)
        {
            hasTriggeredFinish = true;
            StartCoroutine(FinishAfterDelay());
        }
    }

    public void StartFadeOut()
    {
        isFading = true;

        // cursor terughalen
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public Vector2 GetMovementDelta()
    {
        return isPickedUp ? movementDelta : Vector2.zero;
    }

    public bool IsPickedUp()
    {
        return isPickedUp;
    }

    private System.Collections.IEnumerator FinishAfterDelay()
    {
        yield return new WaitForSeconds(delayAfterFade);

        if (finishBranchScene != null)
        {
            finishBranchScene.FinishScene();
        }
    }
}