using UnityEngine;

public class SwingMotion : MonoBehaviour
{
    [Header("Swing Settings")]
    [SerializeField] private float swingAngle = 20f;
    [SerializeField] private float swingDuration = 1.5f;

    [Header("Optional Object To Activate After Swing")]
    [SerializeField] private GameObject activateAfterSwing;

    private Quaternion startRotation;
    private float timer;
    private bool isSwinging;

    private void Awake()
    {
        startRotation = transform.rotation;
    }

    private void Start()
    {
        PlaySwing();
    }

    // Call this to start the swing
    public void PlaySwing()
    {
        if (isSwinging) return;

        timer = 0f;
        isSwinging = true;
    }

    private void Update()
    {
        if (!isSwinging) return;

        timer += Time.deltaTime;

        float normalized = timer / swingDuration;

        if (normalized >= 1f)
        {
            transform.rotation = startRotation;
            isSwinging = false;

            if (activateAfterSwing != null)
                activateAfterSwing.SetActive(true);

            return;
        }

        // Full wave: right → left → center
        float angle = Mathf.Sin(normalized * Mathf.PI * 2f) * swingAngle;

        transform.rotation = startRotation * Quaternion.Euler(0, 0, angle);
    }
}
