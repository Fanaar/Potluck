using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinishBranchScene : MonoBehaviour
{
    [Header("Scene Settings")]
    public string dolmaSceneName = "DolmaScene";

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    public void FinishScene()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        float timer = 0f;

        // fade ambience tegelijk starten
        SceneAmbience ambience =
            FindObjectOfType<SceneAmbience>();

        if (ambience != null)
        {
            ambience.FadeOutAndStop();
        }


        // fade naar zwart
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;

        // ronde verhogen
        GameState.Instance.currentRound++;

        // onthouden dat we terugkomen uit een branch scene
        GameState.Instance.returningFromScene = true;

        // terug naar dolma scene
        SceneManager.LoadScene(dolmaSceneName);
    }
}