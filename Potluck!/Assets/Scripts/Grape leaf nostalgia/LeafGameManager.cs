using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LeafGameManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string nextSceneName;

    [Header("UI Feedback")]
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float messageDuration = 2f;

    private int goodLeafCount;

    private void Start()
    {
        // Count all good leaves at start
        GameObject[] goodLeaves = GameObject.FindGameObjectsWithTag("goodleaf");
        goodLeafCount = goodLeaves.Length;
    }

    public void GoodLeafPlucked()
    {
        goodLeafCount--;

        if (goodLeafCount <= 0)
        {
            LoadNextScene();
        }
    }

    public void BadLeafPlucked()
    {
        if (feedbackText != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowMessage("hmm deze is misschien niet goed voor dolma"));
        }
    }

    private System.Collections.IEnumerator ShowMessage(string message)
    {
        feedbackText.text = message;
        feedbackText.alpha = 1f;

        yield return new WaitForSeconds(messageDuration);

        feedbackText.alpha = 0f;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}