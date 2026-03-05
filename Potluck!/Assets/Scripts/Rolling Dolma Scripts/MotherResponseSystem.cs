using UnityEngine;

public class MotherResponseSystem : MonoBehaviour
{
    public QuestionData[] questions;

    public void PlayResponse()
    {
        string start = QuestionSystem.Instance.currentQuestionStart;
        string end = QuestionSystem.Instance.currentQuestionEnding;

        foreach (var q in questions)
        {
            if (q.questionStart == start && q.questionEnding == end)
            {
                StartCoroutine(PlayMotherAnimation(q.motherResponse));
                return;
            }
        }
    }

    System.Collections.IEnumerator PlayMotherAnimation(string response)
    {
        CameraPanController.Instance.PanToMother();

        yield return new WaitForSeconds(1f);

        DialogueUI.Instance.ShowText(response);

        yield return new WaitForSeconds(5f);

        RoundManager.Instance.NextRound();
    }
}