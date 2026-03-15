using UnityEngine;

public class Basic2DController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform visual;

    private float moveInput;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        visual = animator.transform;

        originalScale = visual.localScale;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        animator.SetBool("isWalking", Mathf.Abs(moveInput) > 0.01f);

        if (moveInput > 0)
            visual.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (moveInput < 0)
            visual.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}