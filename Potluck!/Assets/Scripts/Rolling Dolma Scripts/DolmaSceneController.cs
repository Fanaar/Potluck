using UnityEngine;
using System.Collections;

public class DolmaSceneController : MonoBehaviour
{
    public GameObject dolmaOnPlatePrefab;

    void Start()
    {
        if (GameState.Instance.returningFromScene)
        {
            StartCoroutine(HandleReturnFlow());
        }
    }

    IEnumerator HandleReturnFlow()
    {
        GameState.Instance.returningFromScene = false;

        // moeder legt dolma neer
        dolmaOnPlatePrefab.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        // camera terug naar speler
        CameraPanController.Instance.PanToPlayer();

        yield return new WaitForSeconds(1.5f);

        // nieuwe vraag starten
        UIQuestionPreview.Instance.UnlockQuestion();
    }
}