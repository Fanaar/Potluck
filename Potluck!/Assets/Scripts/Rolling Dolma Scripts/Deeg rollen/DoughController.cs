using UnityEngine;

public class DoughController : MonoBehaviour
{
    [SerializeField] private float minScale = 4f;
    [SerializeField] private float maxScale = 6f;
    [SerializeField] private float stretchSpeed = 2f;

    private float currentX;
    private float currentY;

    void Start()
    {
        currentX = minScale;
        currentY = minScale;
    }

    public void Roll(Vector2 movement)
    {
        if (movement.magnitude < 0.01f) return;

        // bepaal richting
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            // horizontaal → stretch X
            currentX += Mathf.Abs(movement.x) * stretchSpeed * Time.deltaTime;
        }
        else
        {
            // verticaal → stretch Y
            currentY += Mathf.Abs(movement.y) * stretchSpeed * Time.deltaTime;
        }

        // clamp zodat het niet oneindig groeit
        currentX = Mathf.Clamp(currentX, minScale, maxScale);
        currentY = Mathf.Clamp(currentY, minScale, maxScale);

        // optioneel: klein beetje terugveren voor zachtheid
        currentX = Mathf.Lerp(currentX, minScale, Time.deltaTime * 0.5f);
        currentY = Mathf.Lerp(currentY, minScale, Time.deltaTime * 0.5f);

        transform.localScale = new Vector3(currentX, currentY, 1f);
    }
}