using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement; // 👈 BELANGRIJK

public class MotherSequencePlayer : MonoBehaviour
{
    public static MotherSequencePlayer Instance;

    [Header("References")]
    public SpriteRenderer motherRenderer;
    public TextMeshProUGUI dialogueText;

    [Header("Timing")]
    public float timeBetweenLines = 2.5f;
    public float fadeDuration = 0.3f;
    public float startDelay = 1f;

    void Awake()
    {
        Instance = this;
        dialogueText.gameObject.SetActive(false);
    }

    public void PlaySequence(MotherLine[] lines)
    {
        StopAllCoroutines();
        StartCoroutine(Play(lines));
    }

    IEnumerator Play(MotherLine[] lines)
    {
        // camera naar moeder
        CameraPanController.Instance.PanToMother();

        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < lines.Length; i++)
        {
            yield return StartCoroutine(PlayLine(lines[i]));
        }

        dialogueText.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        LoadNextScene(); // 👈 NIEUW
    }

    void LoadNextScene()
    {
        int round = GameState.Instance.currentRound;
        int choice = GameState.Instance.lastChoice;

        string sceneName = "";

        if (round == 0)
            sceneName = (choice == 0) ? "R1_A" : "R1_B";

        else if (round == 1)
            sceneName = (choice == 0) ? "R2_A" : "R2_B";

        else if (round == 2)
            sceneName = (choice == 0) ? "R3_A" : "R3_B";

        Debug.Log("Loading scene: " + sceneName); // 👈 debug

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator PlayLine(MotherLine line)
    {
        yield return StartCoroutine(Fade(1, 0));

        if (line.sprite != null)
            motherRenderer.sprite = line.sprite;

        yield return StartCoroutine(Fade(0, 1));

        dialogueText.gameObject.SetActive(true);
        dialogueText.text = line.text;

        yield return new WaitForSeconds(timeBetweenLines);
    }

    IEnumerator Fade(float from, float to)
    {
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