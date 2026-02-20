using UnityEngine;
using System.Collections;

public class SpriteFadeController : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Optional Next Sprite")]
    [SerializeField] private Sprite nextSprite;

    [Header("Optional Object To Activate After Fade")]
    [SerializeField] private GameObject activateAfterFade;


    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        FadeIn();
    }

    // Call this to fade IN
    public void FadeIn()
    {
        StartFade(0f, 1f, null);
    }

    // Call this to fade OUT
    public void FadeOut()
    {
        StartFade(1f, 0f, null);
    }

    // Call this to fade out, swap sprite, then fade in
    public void FadeToNextSprite()
    {
        StartFade(1f, 0f, () =>
        {
            if (nextSprite != null)
                spriteRenderer.sprite = nextSprite;

            StartFade(0f, 1f, null);
        });
    }

    private void StartFade(float from, float to, System.Action onComplete)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeRoutine(from, to, onComplete));
    }

    private IEnumerator FadeRoutine(float from, float to, System.Action onComplete)
    {
        float t = 0f;
        float duration = Mathf.Max(0.01f, fadeDuration);

        Color c = spriteRenderer.color;
        c.a = from;
        spriteRenderer.color = c;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / duration);

            c.a = a;
            spriteRenderer.color = c;

            yield return null;
        }

        c.a = to;
        spriteRenderer.color = c;

        onComplete?.Invoke();

        // ⭐ Activeer object als hij bestaat
        if (activateAfterFade != null)
            activateAfterFade.SetActive(true);
    }

}
