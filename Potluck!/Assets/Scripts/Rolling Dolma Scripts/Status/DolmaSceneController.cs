using UnityEngine;
using System.Collections;

public class DolmaSceneController : MonoBehaviour
{
    void Start()
    {
        if (GameState.Instance.returningFromScene)
        {
            StartCoroutine(ReturnFlow());
        }
    }

    IEnumerator ReturnFlow()
    {
        GameState.Instance.returningFromScene = false;

        yield return new WaitForSeconds(0.5f);

        // camera terug naar speler
        CameraPanController.Instance.PanToPlayer();

        yield return new WaitForSeconds(1.5f);

        // nieuwe vraag unlocken
        UIQuestionPreview.Instance.UnlockQuestion();
    }
}