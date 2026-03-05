using UnityEngine;
using TMPro;

public class UIQuestionPreview : MonoBehaviour
{
    public static UIQuestionPreview Instance;

    public TextMeshProUGUI questionText;

    string[] starts;
    int currentIndex = 0;

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
        starts = questionStarts;
        currentIndex = 0;
        questionText.gameObject.SetActive(true);
        UpdateText();
    }

    public void Hide()
    {
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
}