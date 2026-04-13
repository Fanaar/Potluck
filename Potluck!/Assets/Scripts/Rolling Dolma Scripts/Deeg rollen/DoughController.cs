using UnityEngine;

public class DoughController : MonoBehaviour
{
    [Header("Scaling")]
    [SerializeField] private float minScale = 4f;
    [SerializeField] private float maxScale = 6f;
    [SerializeField] private float stretchSpeed = 2f;

    [Header("Finish")]
    [SerializeField] private float finishThreshold = 0.9f;
    [SerializeField] private float finishLerpSpeed = 2f;

    [Header("References")]
    [SerializeField] private RollingPinController rollingPin;

    private float currentX;
    private float currentY;

    private bool isFinished = false;

    void Start()
    {
        currentX = minScale;
        currentY = minScale;
    }

    void Update()
    {
        if (isFinished)
        {
            // Smooth naar perfecte cirkel
            currentX = Mathf.Lerp(currentX, maxScale, Time.deltaTime * finishLerpSpeed);
            currentY = Mathf.Lerp(currentY, maxScale, Time.deltaTime * finishLerpSpeed);

            transform.localScale = new Vector3(currentX, currentY, 1f);
        }
    }

    public void Roll(Vector2 movement)
    {
        if (isFinished) return;
        if (movement.magnitude < 0.01f) return;

        // Richting bepalen
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            currentX += Mathf.Abs(movement.x) * stretchSpeed * Time.deltaTime;
        }
        else
        {
            currentY += Mathf.Abs(movement.y) * stretchSpeed * Time.deltaTime;
        }

        currentX = Mathf.Clamp(currentX, minScale, maxScale);
        currentY = Mathf.Clamp(currentY, minScale, maxScale);

        transform.localScale = new Vector3(currentX, currentY, 1f);

        // Check finish
        float xProgress = currentX / maxScale;
        float yProgress = currentY / maxScale;

        if (xProgress > finishThreshold && yProgress > finishThreshold)
        {
            isFinished = true;

            if (rollingPin != null)
            {
                rollingPin.StartFadeOut();
            }
        }
    }
}