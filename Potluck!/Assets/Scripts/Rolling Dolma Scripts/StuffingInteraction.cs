using UnityEngine;

public class StuffingInteraction : MonoBehaviour
{
    public string[] questionStarts;
    public Texture2D cursorStuffing;
    public GameObject stuffingPrefab;

    void OnMouseEnter()
    {
        UIQuestionPreview.Instance.ShowStarts(questionStarts);
    }

    void OnMouseExit()
    {
        UIQuestionPreview.Instance.Hide();
    }

    void OnMouseDown()
    {
        // voorkomt oneindig spawnen
        if (DragDropController.Instance.IsDragging())
            return;

        string selected = UIQuestionPreview.Instance.GetSelectedStart();
        QuestionSystem.Instance.SetQuestionStart(selected);

        Cursor.SetCursor(cursorStuffing, Vector2.zero, CursorMode.Auto);

        SpawnStuffing();
    }

    void SpawnStuffing()
    {
        Vector3 spawnPos = transform.position + Vector3.up * 0.4f;

        GameObject stuffing = Instantiate(stuffingPrefab, spawnPos, Quaternion.identity);

        DragDropController.Instance.StartDrag(stuffing);
    }
}