using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MotherSequencePlayer : MonoBehaviour
{
    [Header("Scene References")]
    public CameraPanController cameraPan;

    [Header("Dialogue References")]
    public SpriteRenderer motherRenderer;
    public TextMeshProUGUI dialogueText;

    [Header("UI Buttons")]
    public Button nextButton;
    public Button prevButton;
    public Button continueButton;

    public float buttonXOffset = 0f;

    [Header("Timing")]
    public float fadeDuration = 0.3f;
    public float startDelay = 1f;

    [Header("Typewriter")]
    public float revealSpeed = 0.02f;

    [Header("Screen Fade")]
    public ScreenFade screenFade;

    public bool IsReady { get; private set; } = false;

    private MotherLine[] currentLines;
    private int currentIndex = 0;

    private bool isPlaying = false;
    private bool canClick = false;
    private bool isRevealing = false;

    private Coroutine revealCoroutine;

    void Start()
    {
        RebindReferences();

        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        if (prevButton != null)
            prevButton.gameObject.SetActive(false);

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        IsReady = true;
    }

    void RebindReferences()
    {
        if (cameraPan == null)
            cameraPan = FindObjectOfType<CameraPanController>();

        if (motherRenderer == null)
        {
            GameObject mother = GameObject.Find("Mother");

            if (mother != null)
                motherRenderer =
                    mother.GetComponent<SpriteRenderer>();
        }

        if (dialogueText == null)
        {
            GameObject textObj =
                GameObject.Find("DialogueText");

            if (textObj != null)
                dialogueText =
                    textObj.GetComponent<TextMeshProUGUI>();
        }

        if (motherRenderer == null || dialogueText == null)
        {
            Debug.LogError(
                "MotherSequencePlayer: Missing references!"
            );
        }
    }

    public void PlaySequence(MotherLine[] lines)
    {
        RebindReferences();

        if (lines == null || lines.Length == 0)
        {
            Debug.LogError("NO LINES FOUND!");
            return;
        }

        currentLines = lines;

        currentIndex = 0;

        isPlaying = true;

        canClick = false;

        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        // camera pan
        if (cameraPan != null)
        {
            cameraPan.PanToMother();

            yield return new WaitForSeconds(
                cameraPan.panDuration
            );
        }

        yield return new WaitForSeconds(startDelay);

        // buttons zichtbaar maken
        if (nextButton != null)
            nextButton.gameObject.SetActive(true);

        if (prevButton != null)
            prevButton.gameObject.SetActive(false);

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        // eerste line automatisch starten
        ShowLine(currentLines[currentIndex]);
    }

    public void NextLine()
    {
        if (!isPlaying)
            return;

        // eerst typing afmaken
        if (isRevealing)
        {
            if (revealCoroutine != null)
                StopCoroutine(revealCoroutine);

            dialogueText.maxVisibleCharacters =
                dialogueText.text.Length;

            isRevealing = false;

            canClick = true;

            return;
        }

        // daarna pas normale input blokkeren
        if (!canClick)
            return;

        // laatste zin
        if (currentIndex >= currentLines.Length - 1)
        {
            if (nextButton != null)
                nextButton.gameObject.SetActive(false);

            if (continueButton != null)
                continueButton.gameObject.SetActive(true);

            return;
        }

        currentIndex++;

        ShowLine(currentLines[currentIndex]);
    }

    public void PreviousLine()
    {
        if (!canClick)
            return;

        if (currentIndex > 0)
        {
            currentIndex--;

            ShowLine(currentLines[currentIndex]);
        }
    }

    public void ContinueToScene()
    {
        EndSequence();
    }

    void ShowLine(MotherLine line)
    {
        if (revealCoroutine != null)
            StopCoroutine(revealCoroutine);

        StartCoroutine(ShowLineRoutine(line));
    }

    IEnumerator ShowLineRoutine(MotherLine line)
    {
        canClick = false;

        // fade out
        yield return StartCoroutine(Fade(1, 0));

        // sprite wisselen
        if (line.sprite != null && motherRenderer != null)
        {
            motherRenderer.sprite = line.sprite;
        }

        // fade in
        yield return StartCoroutine(Fade(0, 1));

        // tekst tonen
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(true);

            dialogueText.text = line.text;

            dialogueText.ForceMeshUpdate();

            dialogueText.maxVisibleCharacters = 0;

            revealCoroutine =
                StartCoroutine(RevealText());
        }

    }

    IEnumerator RevealText()
    {
        isRevealing = true;

        // buttons tijdelijk verbergen
        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        if (prevButton != null)
            prevButton.gameObject.SetActive(false);

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        int totalVisibleCharacters =
            dialogueText.textInfo.characterCount;

        for (int i = 0; i <= totalVisibleCharacters; i++)
        {
            dialogueText.maxVisibleCharacters = i;

            yield return new WaitForSeconds(revealSpeed);
        }

        isRevealing = false;

        canClick = true;

        // NU PAS buttons tonen
        UpdateButtons();
    }

    void UpdateButtons()
    {
        dialogueText.ForceMeshUpdate();

        TMP_TextInfo textInfo =
            dialogueText.textInfo;

        if (textInfo.characterCount == 0)
            return;

        // eerste zichtbare character pakken
        TMP_CharacterInfo firstChar =
            textInfo.characterInfo[0];

        // laatste zichtbare character pakken
        TMP_CharacterInfo lastChar =
            textInfo.characterInfo[textInfo.characterCount - 1];

        // positie eerste letter
        Vector3 firstPos =
            dialogueText.transform.TransformPoint(
                firstChar.bottomLeft
            );

        // positie laatste regel
        Vector3 lastPos =
            dialogueText.transform.TransformPoint(
                lastChar.bottomLeft
            );

        // omzetten naar canvas local pos
        Vector3 firstLocal =
            dialogueText.rectTransform.parent
                .InverseTransformPoint(firstPos);

        Vector3 lastLocal =
            dialogueText.rectTransform.parent
                .InverseTransformPoint(lastPos);

        // iets onder de tekst
        float buttonY =
            lastLocal.y - 35f;

        // begin precies onder de eerste letter
        float startX =
            firstLocal.x + buttonXOffset;

        // Previous
        if (prevButton != null)
        {
            prevButton.gameObject.SetActive(
                currentIndex > 0
            );

            prevButton.transform.localPosition =
                new Vector3(
                    startX,
                    buttonY,
                    0
                );
        }

        bool isLastLine =
            currentIndex >= currentLines.Length - 1;

        // Next
        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(
                !isLastLine
            );

            nextButton.transform.localPosition =
                new Vector3(
                    startX + 160f,
                    buttonY,
                    0
                );
        }

        // Continue
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(
                isLastLine
            );

            continueButton.transform.localPosition =
                new Vector3(
                    startX + 160f,
                    buttonY,
                    0
                );
        }
    }

    void EndSequence()
    {
        isPlaying = false;

        canClick = false;

        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        if (prevButton != null)
            prevButton.gameObject.SetActive(false);

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        StartCoroutine(LoadNextSceneRoutine());
    }

    IEnumerator LoadNextSceneRoutine()
    {
        Debug.Log("LoadNextSceneRoutine START");

        if (GameState.Instance == null)
        {
            Debug.LogError("GameState is NULL!");

            yield break;
        }

        int round = GameState.Instance.currentRound;

        int choice = GameState.Instance.lastChoice;

        string sceneName = "";

        if (round == 0)
            sceneName = (choice == 0)
                ? "R1_A"
                : "R1_B";

        else if (round == 1)
            sceneName = (choice == 0)
                ? "R2_A"
                : "R2_B";

        else if (round == 2)
            sceneName = (choice == 0)
                ? "R3_A"
                : "R3_B";

        Debug.Log("Scene chosen: " + sceneName);

        // ambience fade
        SceneAmbience ambience =
            FindObjectOfType<SceneAmbience>();

        if (ambience != null)
        {
            ambience.FadeOutAndStop();
        }

        // screen fade
        if (screenFade == null)
        {
            Debug.LogError(
                "ScreenFade reference is NULL!"
            );
        }
        else
        {
            screenFade.FadeOut();

            yield return new WaitForSeconds(
                screenFade.fadeDuration
            );
        }

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Fade(float from, float to)
    {
        if (motherRenderer == null)
            yield break;

        float t = 0;

        Color c = motherRenderer.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float a =
                Mathf.Lerp(
                    from,
                    to,
                    t / fadeDuration
                );

            c.a = a;

            motherRenderer.color = c;

            yield return null;
        }

        c.a = to;

        motherRenderer.color = c;
    }
}