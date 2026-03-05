using UnityEngine;

[CreateAssetMenu(menuName = "Dolma/Question")]
public class QuestionData : ScriptableObject
{
    public string questionStart;
    public string questionEnding;

    [TextArea(3, 6)]
    public string motherResponse;
}