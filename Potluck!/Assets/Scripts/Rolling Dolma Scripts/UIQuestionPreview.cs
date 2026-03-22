using UnityEngine;
using TMPro;

public class UIQuestionPreview : MonoBehaviour
{
    public static UIQuestionPreview Instance;

    public TextMeshProUGUI questionText;

    string[] starts;
    int currentIndex = 0;
    public bool questionLocked = false;

    public TextMeshProUGUI leftChoiceText;
    public TextMeshProUGUI rightChoiceText;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    void Update()
    {
        if (starts == null) return;

        if (Input.GetKeyDown(KeyCode.A))
            Previous();

        if (Input.GetKeyDown(KeyCode.D))
            Next();
    }

    public void ShowStarts(string[] questionStarts)
    {
        if (questionLocked) return;

        starts = questionStarts;
        currentIndex = 0;
        questionText.gameObject.SetActive(true);
        UpdateText();
    }

    public void Hide()
    {
        if (questionLocked) return;

        questionText.gameObject.SetActive(false);
    }

    void UpdateText()
    {
        if (starts == null) return;
        questionText.text = starts[currentIndex];
    }

    void Next()
    {
        currentIndex++;
        if (currentIndex >= starts.Length)
            currentIndex = 0;

        UpdateText();
    }

    void Previous()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = starts.Length - 1;

        UpdateText();
    }

    public string GetSelectedStart()
    {
        return starts[currentIndex];
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }

    // 🔥 NIEUW: full question tonen
    public void ShowFullQuestion(string ending)
    {
        if (starts == null) return;

        string start = starts[currentIndex];

        questionText.gameObject.SetActive(true);
        questionText.text = start + " " + ending;
    }

    public void LockQuestion()
    {
        questionLocked = true;
    }

    public void UnlockQuestion()
    {
        questionLocked = false;
        questionText.gameObject.SetActive(false);
    }

    public void ShowEndingChoices(string left, string right)
    {
        leftChoiceText.gameObject.SetActive(true);
        rightChoiceText.gameObject.SetActive(true);

        leftChoiceText.text = left + " →";
        rightChoiceText.text = "← " + right;
    }

    public void HideEndingChoices()
    {
        leftChoiceText.gameObject.SetActive(false);
        rightChoiceText.gameObject.SetActive(false);
    }
}