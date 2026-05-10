using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;
    public Button nextButton;
    public Button prevButton;
    public Button endButton;

    [Header("Dialogue Settings")]
    [TextArea(2, 5)]
    public List<string> dialogueLines;

    public float revealSpeed = 0.02f;

    [Header("Scene Settings")]
    public string nextSceneName;

    private int currentIndex = 0;
    private Coroutine revealCoroutine;
    private bool isRevealing = false;

    void Start()
    {
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);

        ShowLine();
    }

    public void NextLine()
    {
        if (isRevealing)
        {
            StopCoroutine(revealCoroutine);

            dialogueText.maxVisibleCharacters = dialogueText.text.Length;

            isRevealing = false;
            UpdateButtons();
            return;
        }

        if (currentIndex < dialogueLines.Count - 1)
        {
            currentIndex++;
            ShowLine();
        }
    }

    public void PreviousLine()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowLine();
        }
    }

    void ShowLine()
    {
        if (revealCoroutine != null)
            StopCoroutine(revealCoroutine);

        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);

        revealCoroutine = StartCoroutine(RevealText(dialogueLines[currentIndex]));
    }

    IEnumerator RevealText(string line)
    {
        isRevealing = true;

        dialogueText.text = line;

        dialogueText.ForceMeshUpdate();

        int totalVisibleCharacters = dialogueText.textInfo.characterCount;

        dialogueText.maxVisibleCharacters = 0;

        for (int i = 0; i <= totalVisibleCharacters; i++)
        {
            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(revealSpeed);
        }

        isRevealing = false;
        UpdateButtons();
    }

    void UpdateButtons()
    {
        prevButton.gameObject.SetActive(currentIndex > 0);

        bool isLastLine = currentIndex >= dialogueLines.Count - 1;

        nextButton.gameObject.SetActive(!isLastLine);
        endButton.gameObject.SetActive(isLastLine);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}