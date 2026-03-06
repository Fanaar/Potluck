using UnityEngine;

public class QuestionSystem : MonoBehaviour
{
    public static QuestionSystem Instance;

    public string currentQuestionStart;
    public string currentQuestionEnding;

    void Awake()
    {
        Instance = this;
    }

    public void SetQuestionStart(string start)
    {
        currentQuestionStart = start;
    }

    public void SetQuestionEnding(string ending)
    {
        currentQuestionEnding = ending;

        UpdateQuestionUI();
    }

    void UpdateQuestionUI()
    {
        string fullQuestion = currentQuestionStart + " " + currentQuestionEnding;

        UIQuestionPreview.Instance.ShowFullQuestion(fullQuestion);
    }

    public string GetFullQuestion()
    {
        return currentQuestionStart + " " + currentQuestionEnding;
    }
}