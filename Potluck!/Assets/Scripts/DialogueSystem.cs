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

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    [Header("Dialogue Settings")]
    [TextArea(2, 5)]
    public List<string> dialogueLines;

    public float revealSpeed = 0.02f;

    [Header("Scene Settings")]
    public string nextSceneName;

    [Header("Button Position")]
    public float buttonXOffset = 0f;

    private int currentIndex = 0;
    private Coroutine revealCoroutine;
    private bool isRevealing = false;

    void Start()
    {
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(false);

        // zorg dat fade canvas transparant start
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
            fadeCanvasGroup.interactable = false;
        }

        ShowLine();
    }

    public void NextLine()
    {
        if (isRevealing)
        {
            StopCoroutine(revealCoroutine);

            dialogueText.maxVisibleCharacters =
                dialogueText.text.Length;

            isRevealing = false;

            PositionButtons();
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

        revealCoroutine =
            StartCoroutine(
                RevealText(dialogueLines[currentIndex])
            );
    }

    IEnumerator RevealText(string line)
    {
        isRevealing = true;

        dialogueText.text = line;

        dialogueText.ForceMeshUpdate();

        int totalVisibleCharacters =
            dialogueText.textInfo.characterCount;

        dialogueText.maxVisibleCharacters = 0;

        for (int i = 0; i <= totalVisibleCharacters; i++)
        {
            dialogueText.maxVisibleCharacters = i;

            yield return new WaitForSeconds(revealSpeed);
        }

        isRevealing = false;

        PositionButtons();
        UpdateButtons();
    }

    void UpdateButtons()
    {
        prevButton.gameObject.SetActive(currentIndex > 0);

        bool isLastLine =
            currentIndex >= dialogueLines.Count - 1;

        nextButton.gameObject.SetActive(!isLastLine);
        endButton.gameObject.SetActive(isLastLine);
    }

    public void LoadNextScene()
    {
        StartCoroutine(FadeAndLoadScene());
    }

    IEnumerator FadeAndLoadScene()
    {
        float timer = 0f;

        // fade ambience tegelijk starten
        SceneAmbience ambience =
            FindObjectOfType<SceneAmbience>();

        if (ambience != null)
        {
            ambience.FadeOutAndStop();
        }

        // blokkeer input tijdens fade
        fadeCanvasGroup.blocksRaycasts = true;
        fadeCanvasGroup.interactable = true;

        // screen fade
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            fadeCanvasGroup.alpha =
                timer / fadeDuration;

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;

        SceneManager.LoadScene(nextSceneName);
    }

    void PositionButtons()
    {
        dialogueText.ForceMeshUpdate();

        TMP_TextInfo textInfo =
            dialogueText.textInfo;

        if (textInfo.characterCount == 0)
            return;

        // eerste zichtbare character
        TMP_CharacterInfo firstChar =
            textInfo.characterInfo[0];

        // laatste zichtbare character
        TMP_CharacterInfo lastChar =
            textInfo.characterInfo[textInfo.characterCount - 1];

        // wereld posities
        Vector3 firstPos =
            dialogueText.transform.TransformPoint(
                firstChar.bottomLeft
            );

        Vector3 lastPos =
            dialogueText.transform.TransformPoint(
                lastChar.bottomLeft
            );

        // omzetten naar local canvas positie
        Vector3 firstLocal =
            dialogueText.rectTransform.parent
                .InverseTransformPoint(firstPos);

        Vector3 lastLocal =
            dialogueText.rectTransform.parent
                .InverseTransformPoint(lastPos);

        // iets onder de laatste regel
        float buttonY =
            lastLocal.y - 60f;

        // begin onder eerste letter
        float startX =
            firstLocal.x + buttonXOffset;

        // Previous
        prevButton.transform.localPosition =
            new Vector3(
                startX,
                buttonY,
                0
            );

        bool isLastLine =
            currentIndex >= dialogueLines.Count - 1;

        // Next
        nextButton.transform.localPosition =
            new Vector3(
                startX + 160f,
                buttonY,
                0
            );

        // End
        endButton.transform.localPosition =
            new Vector3(
                startX + 160f,
                buttonY,
                0
            );
    }
}