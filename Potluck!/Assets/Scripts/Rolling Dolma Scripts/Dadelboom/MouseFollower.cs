using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    public LayerMask wallLayer;
    public float radius = 0.3f;
    public float smoothSpeed = 15f;

    public Transform startPoint; // 👈 deze toevoegen

    void Start()
    {
        Cursor.visible = false;

        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
    }

    void Update()
    {
        Vector2 currentPos = transform.position;
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = targetPos - currentPos;
        Vector2 desiredMove = direction * smoothSpeed * Time.deltaTime;

        Vector2 moveX = new Vector2(desiredMove.x, 0);
        if (!IsBlocked(currentPos + moveX))
        {
            currentPos += moveX;
        }

        Vector2 moveY = new Vector2(0, desiredMove.y);
        if (!IsBlocked(currentPos + moveY))
        {
            currentPos += moveY;
        }

        transform.position = currentPos;
    }

    bool IsBlocked(Vector2 position)
    {
        return Physics2D.OverlapCircle(position, radius, wallLayer);
    }
}