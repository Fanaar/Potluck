using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [SerializeField] private Color hoverColor = Color.gray;
    [SerializeField] private float fadeSpeed = 5f; // 👈 fade snelheid

    private bool isHovering = false;
    private bool isDisabled = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (isDisabled) return;

        Color targetColor = isHovering ? hoverColor : originalColor;

        spriteRenderer.color = Color.Lerp(
            spriteRenderer.color,
            targetColor,
            Time.deltaTime * fadeSpeed
        );
    }

    void OnMouseEnter()
    {
        if (isDisabled) return;
        isHovering = true;
    }

    void OnMouseExit()
    {
        if (isDisabled) return;
        isHovering = false;
    }

    public void DisableHover()
    {
        isDisabled = true;
        isHovering = false;

        // 👇 Zorg dat hij netjes terug fade naar origineel
        StopAllCoroutines();
        StartCoroutine(FadeBackToOriginal());
    }

    private System.Collections.IEnumerator FadeBackToOriginal()
    {
        while (Vector4.Distance(spriteRenderer.color, originalColor) > 0.01f)
        {
            spriteRenderer.color = Color.Lerp(
                spriteRenderer.color,
                originalColor,
                Time.deltaTime * fadeSpeed
            );
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }
}