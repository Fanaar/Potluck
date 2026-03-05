using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    public int round = 0;

    public SpriteRenderer bowlRenderer;

    public Sprite bowlFull;
    public Sprite bowlHalf;
    public Sprite bowlEmpty;

    void Awake()
    {
        Instance = this;
    }

    public void NextRound()
    {
        round++;

        if (round == 1)
            bowlRenderer.sprite = bowlHalf;

        if (round == 2)
            bowlRenderer.sprite = bowlEmpty;

        if (round >= 3)
            EndScene();
    }

    void EndScene()
    {
        Debug.Log("Final dolma moment");

        SceneTransition.Instance.LoadNextScene();
    }
}