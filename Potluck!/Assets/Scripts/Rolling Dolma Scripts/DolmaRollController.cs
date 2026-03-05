using UnityEngine;

public class DolmaRollController : MonoBehaviour
{
    public Sprite leafWithStuffing;
    public Sprite bottomFolded;
    public Sprite leftFolded;
    public Sprite rightFolded;
    public Sprite allFolded;
    public Sprite finishedDolma;

    SpriteRenderer sr;

    Vector2 dragStart;

    int state = 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        dragStart = Input.mousePosition;
    }

    void OnMouseUp()
    {
        Vector2 dragEnd = Input.mousePosition;

        Vector2 dir = dragEnd - dragStart;

        ProcessDrag(dir);
    }

    void ProcessDrag(Vector2 dir)
    {
        if (state == 0 && dir.y > 50)
        {
            sr.sprite = bottomFolded;
            state = 1;
            return;
        }

        if (state == 1)
        {
            if (dir.x < -50)
            {
                sr.sprite = leftFolded;
                state = 2;

                QuestionSystem.Instance.SetQuestionEnding(
                    "toen je Irak moest verlaten?"
                );
            }

            if (dir.x > 50)
            {
                sr.sprite = rightFolded;
                state = 2;

                QuestionSystem.Instance.SetQuestionEnding(
                    "toen je hier opnieuw moest beginnen?"
                );
            }

            return;
        }

        if (state == 2)
        {
            sr.sprite = allFolded;
            state = 3;
            return;
        }

        if (state == 3)
        {
            sr.sprite = finishedDolma;
            state = 4;

            Debug.Log("Question: " + QuestionSystem.Instance.GetFullQuestion());
        }
    }

    public void AddStuffing()
    {
        sr.sprite = leafWithStuffing;
    }
}