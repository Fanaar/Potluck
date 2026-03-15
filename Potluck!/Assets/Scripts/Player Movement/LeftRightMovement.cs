using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;     // Snelheid (Inspector)
    [SerializeField] private float distance = 5f;  // Hoe ver naar rechts bewegen

    private Vector3 startPos;
    private float targetX;
    private bool isMoving = true;

    private void Start()
    {
        startPos = transform.position;
        targetX = startPos.x + distance;
    }

    private void Update()
    {
        if (!isMoving) return;

        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x >= targetX)
        {
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            isMoving = false; // stop na 1 keer bewegen
        }
    }
}
