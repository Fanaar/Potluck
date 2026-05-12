using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class MotherSequencePlayer : MonoBehaviour
{
    [Header("Scene References")]
    public CameraPanController cameraPan;

    [Header("Dialogue References")]
    public SpriteRenderer motherRenderer;
    public TextMeshProUGUI dialogueText;

    [Header("Timing")]
    public float fadeDuration = 0.3f;
    public float startDelay = 1f;

    [Header("Screen Fade")]
    public ScreenFade screenFade;
    public bool IsReady { get; private set; } = false;

    private MotherLine[] currentLines;
    private int currentIndex = 0;
    private bool isPlaying = false;
    private bool canClick = false;

    void Start()
    {
        RebindReferences();

        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);

        IsReady = true; // 🔥 BELANGRIJK
    }

    void Update()
    {
        if (!isPlaying || !canClick) return;

        if (Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    void RebindReferences()
    {
        if (cameraPan == null)
            cameraPan = FindObjectOfType<CameraPanController>();

        if (motherRenderer == null)
        {
            GameObject mother = GameObject.Find("Mother");
            if (mother != null)
                motherRenderer = mother.GetComponent<SpriteRenderer>();
        }

        if (dialogueText == null)
        {
            GameObject textObj = GameObject.Find("DialogueText");
            if (textObj != null)
                dialogueText = textObj.GetComponent<TextMeshProUGUI>();
        }

        if (motherRenderer == null || dialogueText == null)
        {
            Debug.LogError("MotherSequencePlayer: Missing references!");
        }
    }

    public void PlaySequence(MotherLine[] lines)
    {
        RebindReferences(); // 🔥 altijd opnieuw binden

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
            yield return new WaitForSeconds(cameraPan.panDuration);
        }

        yield return new WaitForSeconds(startDelay);

        ShowLine(currentLines[currentIndex]);

        canClick = true;
    }

    void NextLine()
    {
        currentIndex++;

        if (currentIndex >= currentLines.Length)
        {
            EndSequence();
            return;
        }

        ShowLine(currentLines[currentIndex]);
    }

    void ShowLine(MotherLine line)
    {
        StopAllCoroutines();
        StartCoroutine(ShowLineRoutine(line));
    }

    IEnumerator ShowLineRoutine(MotherLine line)
    {
        canClick = false;

        // fade out
        yield return StartCoroutine(Fade(1, 0));

        // sprite
        if (line.sprite != null && motherRenderer != null)
            motherRenderer.sprite = line.sprite;

        // fade in
        yield return StartCoroutine(Fade(0, 1));

        // text
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(true);
            dialogueText.text = line.text;
        }

        canClick = true;
    }

    void EndSequence()
    {
        isPlaying = false;
        canClick = false;

        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);

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
            sceneName = (choice == 0) ? "R1_A" : "R1_B";
        else if (round == 1)
            sceneName = (choice == 0) ? "R2_A" : "R2_B";
        else if (round == 2)
            sceneName = (choice == 0) ? "R3_A" : "R3_B";

        Debug.Log("Scene chosen: " + sceneName);

        // CHECK SCREENFADE
        if (screenFade == null)
        {
            Debug.LogError("ScreenFade reference is NULL!");
        }
        else
        {
            Debug.Log("Calling FadeOut()");

            screenFade.FadeOut();

            Debug.Log("Waiting for fade duration: " + screenFade.fadeDuration);

            yield return new WaitForSeconds(screenFade.fadeDuration);

            Debug.Log("Fade wait finished");
        }

        Debug.Log("Loading scene NOW");

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
            float a = Mathf.Lerp(from, to, t / fadeDuration);
            c.a = a;
            motherRenderer.color = c;
            yield return null;
        }

        c.a = to;
        motherRenderer.color = c;
    }
}