using UnityEngine;
using TMPro;
using System.Collections;

public class WordHover : MonoBehaviour
{
    private bool hasBeenHovered = false;

    public WordHoverManager manager;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    private TextMeshProUGUI tmp;
    private RectTransform rectTransform;
    private Canvas canvas;

    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>();

        // start invisible
        Color c = tmp.color;
        c.a = 0f;
        tmp.color = c;
    }

    void Update()
    {
        if (hasBeenHovered)
            return;

        Vector2 mousePos = Input.mousePosition;

        bool isHovering =
            RectTransformUtility.RectangleContainsScreenPoint(
                rectTransform,
                mousePos,
                canvas.worldCamera
            );

        if (isHovering)
        {
            hasBeenHovered = true;

            StartCoroutine(FadeIn());

            if (manager != null)
            {
                manager.WordHovered();
            }
        }
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;

        Color startColor = tmp.color;
        startColor.a = 0f;

        Color endColor = tmp.color;
        endColor.a = 1f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            tmp.color = Color.Lerp(
                startColor,
                endColor,
                timer / fadeDuration
            );

            yield return null;
        }

        tmp.color = endColor;
    }
}