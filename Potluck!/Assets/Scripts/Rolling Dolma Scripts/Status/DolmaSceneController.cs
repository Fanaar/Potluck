using UnityEngine;
using System.Collections;

public class DolmaSceneController : MonoBehaviour
{
    [Header("Scene References")]
    public CameraPanController cameraPan;

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
        if (cameraPan != null)
        {
            cameraPan.PanToPlayer();
            yield return new WaitForSeconds(cameraPan.panDuration);
        }

        // nieuwe vraag unlocken
        UIQuestionPreview.Instance.UnlockQuestion();
    }
}