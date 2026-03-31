using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float speed = 2f;

    private bool hasMoved = false;
    public bool hasFinishedMoving = false; // 👈 deze

    void Start()
    {
        if (pointA != null)
        {
            transform.position = pointA.position;
        }
    }

    void Update()
    {
        if (hasMoved || pointB == null) return;

        transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pointB.position) < 0.01f)
        {
            transform.position = pointB.position;
            hasMoved = true;
            hasFinishedMoving = true; // 👈 hier zetten we hem aan
        }
    }
}