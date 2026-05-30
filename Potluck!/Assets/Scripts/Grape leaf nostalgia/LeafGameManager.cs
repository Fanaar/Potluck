using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LeafGameManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string nextSceneName;

    [Header("UI Feedback")]
    [SerializeField] private TextMeshProUGUI feedbackText;

    [SerializeField] private float messageDuration = 2f;

    [SerializeField] private float textFadeDuration = 0.25f;

    [Header("Bad Leaf Messages")]
    [TextArea(2, 4)]
    [SerializeField]
    private string[] badLeafMessages;

    [Header("Bad Leaf Preview")]
    [SerializeField] private Image badLeafPreviewImage;

    [Header("Scene Fade")]
    [SerializeField] private CanvasGroup sceneFadeCanvasGroup;

    [SerializeField] private float sceneFadeDuration = 1f;

    [SerializeField] private CanvasGroup badLeafPreviewCanvasGroup;

    [SerializeField] private float previewFadeDuration = 0.25f;

    [SerializeField] private float previewVisibleDuration = 2f;

    [Header("Preview Slide")]
    [SerializeField]
    private Vector2 previewStartOffset =
        new Vector2(0f, -100f);

    [SerializeField]
    private AnimationCurve previewSlideCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private int goodLeafCount;

    private Coroutine messageCoroutine;
    private Coroutine previewCoroutine;

    // ordered message cycling
    private int currentMessageIndex = 0;

    private void Start()
    {
        // Count all good leaves at start
        GameObject[] goodLeaves =
            GameObject.FindGameObjectsWithTag("goodleaf");

        goodLeafCount = goodLeaves.Length;

        // start hidden
        if (feedbackText != null)
        {
            feedbackText.alpha = 0f;
        }

        if (badLeafPreviewCanvasGroup != null)
        {
            badLeafPreviewCanvasGroup.alpha = 0f;
        }
    }

    public void GoodLeafPlucked()
    {
        goodLeafCount--;

        if (goodLeafCount <= 0)
        {
            LoadNextScene();
        }
    }

    public void BadLeafPlucked(Sprite badLeafSprite)
    {
        // stop old coroutines
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        if (previewCoroutine != null)
        {
            StopCoroutine(previewCoroutine);
        }

        // ordered message
        if (feedbackText != null)
        {
            string message =
                GetNextBadLeafMessage();

            messageCoroutine = StartCoroutine(
                ShowMessage(message)
            );
        }

        // preview
        if (badLeafSprite != null)
        {
            previewCoroutine = StartCoroutine(
                ShowBadLeafPreview(badLeafSprite)
            );
        }
    }

    string GetNextBadLeafMessage()
    {
        if (badLeafMessages == null ||
            badLeafMessages.Length == 0)
        {
            return
                "Hmm.. this one might not be suitable for dolma.";
        }

        string message =
            badLeafMessages[currentMessageIndex];

        currentMessageIndex++;

        // loop back to start
        if (currentMessageIndex >= badLeafMessages.Length)
        {
            currentMessageIndex = 0;
        }

        return message;
    }

    private IEnumerator ShowMessage(string message)
    {
        feedbackText.text = message;

        float t = 0f;

        // start transparent
        feedbackText.alpha = 0f;

        // FADE IN
        while (t < textFadeDuration)
        {
            t += Time.deltaTime;

            feedbackText.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    t / textFadeDuration
                );

            yield return null;
        }

        feedbackText.alpha = 1f;

        // visible
        yield return new WaitForSeconds(
            messageDuration
        );

        // FADE OUT
        t = 0f;

        while (t < textFadeDuration)
        {
            t += Time.deltaTime;

            feedbackText.alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    t / textFadeDuration
                );

            yield return null;
        }

        feedbackText.alpha = 0f;
    }

    private IEnumerator ShowBadLeafPreview(Sprite sprite)
    {
        if (badLeafPreviewImage == null ||
            badLeafPreviewCanvasGroup == null)
        {
            yield break;
        }

        badLeafPreviewImage.sprite = sprite;

        RectTransform rect =
            badLeafPreviewImage.rectTransform;

        Vector2 targetPosition =
            Vector2.zero;

        Vector2 startPosition =
            targetPosition + previewStartOffset;

        rect.anchoredPosition = startPosition;

        float t = 0f;

        // FADE + SLIDE IN
        while (t < previewFadeDuration)
        {
            t += Time.deltaTime;

            float normalized =
                t / previewFadeDuration;

            float curve =
                previewSlideCurve.Evaluate(normalized);

            // fade
            badLeafPreviewCanvasGroup.alpha =
                Mathf.Lerp(0f, 1f, curve);

            // move
            rect.anchoredPosition =
                Vector2.Lerp(
                    startPosition,
                    targetPosition,
                    curve
                );

            yield return null;
        }

        badLeafPreviewCanvasGroup.alpha = 1f;
        rect.anchoredPosition = targetPosition;

        // visible
        yield return new WaitForSeconds(
            previewVisibleDuration
        );

        t = 0f;

        // FADE + SLIDE OUT
        while (t < previewFadeDuration)
        {
            t += Time.deltaTime;

            float normalized =
                t / previewFadeDuration;

            float curve =
                previewSlideCurve.Evaluate(normalized);

            // fade out
            badLeafPreviewCanvasGroup.alpha =
                Mathf.Lerp(1f, 0f, curve);

            // slide out
            rect.anchoredPosition =
                Vector2.Lerp(
                    targetPosition,
                    startPosition,
                    curve
                );

            yield return null;
        }

        badLeafPreviewCanvasGroup.alpha = 0f;
    }

    private void LoadNextScene()
    {
        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        if (sceneFadeCanvasGroup != null)
        {
            float t = 0f;

            // start transparent
            sceneFadeCanvasGroup.alpha = 0f;

            while (t < sceneFadeDuration)
            {
                t += Time.deltaTime;

                sceneFadeCanvasGroup.alpha =
                    Mathf.Lerp(
                        0f,
                        1f,
                        t / sceneFadeDuration
                    );

                yield return null;
            }

            sceneFadeCanvasGroup.alpha = 1f;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}