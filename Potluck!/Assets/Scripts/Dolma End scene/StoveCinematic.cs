using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class StoveCinematic : MonoBehaviour
{
    [Header("Camera")]
    public Camera mainCamera;
    public Transform cameraTarget;

    public float zoomSize = 3f;
    public float moveDuration = 3f;

    [Header("Rotating Object")]
    public Transform rotatingObject;

    [Header("Text Fade")]
    public CanvasGroup textCanvasGroup;

    [Header("Scene Fade")]
    public CanvasGroup fadeCanvasGroup;

    [Header("Scene Loading")]
    public string titleSceneName = "TitleScreen";

    public float holdBeforeFadeOut = 2f;
    public float fadeOutDuration = 3f;

    public float textDelay = 2f;
    public float textFadeDuration = 2f;

    public float rotationSpeed = 20f;

    private bool cinematicStarted = false;

    void Update()
    {
        // Rotate object during cinematic
        if (cinematicStarted && rotatingObject != null)
        {
            rotatingObject.Rotate(
                0,
                0,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void StartCinematic()
    {
        if (cinematicStarted) return;

        cinematicStarted = true;

        StartCoroutine(CameraMove());
        StartCoroutine(FadeInText());
    }

    IEnumerator CameraMove()
    {
        Vector3 startPos = mainCamera.transform.position;

        Vector3 targetPos = new Vector3(
            cameraTarget.position.x,
            cameraTarget.position.y,
            startPos.z
        );

        float startSize = mainCamera.orthographicSize;

        float timer = 0f;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;

            float t = timer / moveDuration;

            mainCamera.transform.position =
                Vector3.Lerp(startPos, targetPos, t);

            mainCamera.orthographicSize =
                Mathf.Lerp(startSize, zoomSize, t);

            yield return null;
        }

        mainCamera.transform.position = targetPos;
        mainCamera.orthographicSize = zoomSize;
    }

    IEnumerator FadeInText()
    {
        yield return new WaitForSeconds(
            moveDuration + textDelay
        );

        float timer = 0f;

        // TEXT FADE IN
        while (timer < textFadeDuration)
        {
            timer += Time.deltaTime;

            float t = timer / textFadeDuration;

            textCanvasGroup.alpha =
                Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        textCanvasGroup.alpha = 1f;

        // HOLD BEFORE SCENE FADE
        yield return new WaitForSeconds(
            holdBeforeFadeOut
        );

        // START FADE OUT
        StartCoroutine(FadeOutScene());
    }

    IEnumerator FadeOutScene()
    {
        float timer = 0f;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;

            float t = timer / fadeOutDuration;

            fadeCanvasGroup.alpha =
                Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;

        // Small pause after full black
        yield return new WaitForSeconds(1f);

        if (GameState.Instance != null)
        {
            Debug.Log("Destroying GameState");
            Destroy(GameState.Instance.gameObject);
            GameState.Instance = null;
        }

        SceneManager.LoadScene(titleSceneName);
    }
}