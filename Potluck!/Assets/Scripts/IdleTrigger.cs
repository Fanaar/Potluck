using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IdleTrigger : MonoBehaviour
{
    [Header("Sprite die moet verschijnen")]
    public SpriteRenderer spriteToFade;

    [Header("Sprite Fade")]
    public float fadeDuration = 1f;

    [Header("Scene Transition")]
    public bool isLastTrigger = false;
    public CanvasGroup fadePanel;
    public string nextSceneName;
    public float sceneFadeDuration = 1.5f;

    private bool isShowing = false;
    private bool sceneLoading = false;

    private Coroutine fadeRoutine;

    private void Start()
    {
        if (spriteToFade != null)
        {
            Color color = spriteToFade.color;
            color.a = 0f;
            spriteToFade.color = color;
        }

        if (fadePanel != null)
        {
            fadePanel.alpha = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        isShowing = true;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeSprite(1f));
    }

    private void Update()
    {
        if (!isShowing || sceneLoading)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            isShowing = false;

            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            fadeRoutine = StartCoroutine(FadeSprite(0f));

            if (isLastTrigger)
            {
                sceneLoading = true;
                StartCoroutine(LoadNextScene());
            }
        }
    }

    private IEnumerator FadeSprite(float targetAlpha)
    {
        Color color = spriteToFade.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            color.a = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                elapsed / fadeDuration
            );

            spriteToFade.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        spriteToFade.color = color;
    }

    private IEnumerator LoadNextScene()
    {
        // Wacht tot de sprite volledig is uitgefaded
        yield return new WaitForSeconds(fadeDuration);

        if (fadePanel != null)
        {
            float elapsed = 0f;

            while (elapsed < sceneFadeDuration)
            {
                elapsed += Time.deltaTime;

                fadePanel.alpha = Mathf.Lerp(
                    0f,
                    1f,
                    elapsed / sceneFadeDuration
                );

                yield return null;
            }

            fadePanel.alpha = 1f;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}