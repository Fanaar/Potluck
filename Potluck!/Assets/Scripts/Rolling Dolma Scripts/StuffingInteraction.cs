using UnityEngine;

public class StuffingInteraction : MonoBehaviour
{
    public string[] questionStarts;

    public Texture2D cursorDefault;
    public Texture2D cursorStuffing;

    private bool hovering;

    void OnMouseEnter()
    {
        hovering = true;
        UIQuestionPreview.Instance.ShowStarts(questionStarts);
    }

    void OnMouseExit()
    {
        hovering = false;
        UIQuestionPreview.Instance.Hide();
    }

    void OnMouseDown()
    {
        if (!hovering) return;

        string selected = UIQuestionPreview.Instance.GetSelectedStart();

        QuestionSystem.Instance.SetQuestionStart(selected);

        Cursor.SetCursor(cursorStuffing, Vector2.zero, CursorMode.Auto);

        DragDropController.Instance.StartDrag(gameObject);
    }
}