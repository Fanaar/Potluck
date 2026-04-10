using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBranchScene : MonoBehaviour
{
    [Header("Scene Settings")]
    public string dolmaSceneName = "DolmaScene";

    public void FinishScene()
    {
        // ronde verhogen
        GameState.Instance.currentRound++;

        // onthouden dat we terugkomen uit een branch scene
        GameState.Instance.returningFromScene = true;

        // terug naar dolma scene
        SceneManager.LoadScene(dolmaSceneName);
    }
}