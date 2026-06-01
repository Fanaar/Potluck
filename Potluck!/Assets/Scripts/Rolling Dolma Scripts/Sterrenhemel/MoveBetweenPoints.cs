using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float speed = 2f;

    [Header("Wait For Fade")]
    public SceneFadeIn sceneFadeIn;

    private bool hasMoved = false;
    public bool hasFinishedMoving = false;

    void Start()
    {
        if (pointA != null)
        {
            transform.position = pointA.position;
        }
    }

    void Update()
    {
        // wacht tot fade klaar is
        if (sceneFadeIn != null && SceneFadeIn.IsFading)
            return;

        if (hasMoved || pointB == null)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            pointB.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, pointB.position) < 0.01f)
        {
            transform.position = pointB.position;

            hasMoved = true;
            hasFinishedMoving = true;
        }
    }
}