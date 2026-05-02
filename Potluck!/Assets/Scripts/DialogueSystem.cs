using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro; // toevoegen bovenaan

public class DialogueSystem : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;
    public RectTransform backgroundBox;
    public Button nextButton;
    public Button prevButton;
    public Button endButton;

    [Header("Dialogue Settings")]
    [TextArea(2, 5)]
    public List<string> dialogueLines;
    public float typingSpeed = 0.03f;

    [Header("Scene Settings")]
    public string nextSceneName;

    private int currentIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Start()
    {
        ShowLine();
        UpdateButtons();
        endButton.gameObject.SetActive(false);
    }

    public void NextLine()
    {
        if (isTyping)
        {
            // tekst meteen afmaken
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogueLines[currentIndex];
            UpdateBackgroundSize();
            isTyping = false;
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
            UpdateButtons();
        }
    }

    void ShowLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // ❗ knoppen uitzetten terwijl tekst typt
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);

        typingCoroutine = StartCoroutine(TypeText(dialogueLines[currentIndex]));
    }

    IEnumerator TypeText(string line)
    {
        dialogueText.text = "";
        isTyping = true;
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            UpdateBackgroundSize();
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        // ❗ tekst is klaar → knoppen tonen
        UpdateButtons();
    }

    void UpdateBackgroundSize()
    {
        float padding = 40f;
        Vector2 textSize = new Vector2(dialogueText.preferredWidth, dialogueText.preferredHeight);
        backgroundBox.sizeDelta = textSize + new Vector2(padding, padding);
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
