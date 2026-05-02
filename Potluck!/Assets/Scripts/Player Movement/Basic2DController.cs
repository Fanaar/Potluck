using UnityEngine;

public class Basic2DController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform visual;

    private Vector3 originalScale;
    private bool isStopped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        visual = animator.transform;

        originalScale = visual.localScale;
    }

    void Update()
    {
        if (isStopped)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        // Altijd naar rechts lopen
        animator.SetBool("isWalking", true);

        visual.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void FixedUpdate()
    {
        if (isStopped)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("idle"))
        {
            isStopped = true;
        }
    }

    // Gebruik deze als je triggers gebruikt i.p.v. colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("idle"))
        {
            isStopped = true;
        }
    }
}