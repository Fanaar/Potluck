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
    }

    public string GetFullQuestion()
    {
        return currentQuestionStart + " " + currentQuestionEnding;
    }

    public void ResetQuestion()
    {
        currentQuestionStart = "";
        currentQuestionEnding = "";
    }
}