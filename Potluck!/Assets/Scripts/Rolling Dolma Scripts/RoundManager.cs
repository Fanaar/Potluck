using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    public int round = 0;

    [Header("Rounds")]
    public RoundData[] rounds; // 👈 BELANGRIJK

    [Header("Bowl Visual")]
    public SpriteRenderer bowlRenderer;
    public Sprite bowlFull;
    public Sprite bowlHalf;
    public Sprite bowlEmpty;

    int currentStartIndex;
    int currentEndingIndex;

    void Awake()
    {
        Instance = this;
    }

    public void SetStartIndex(int index)
    {
        currentStartIndex = index;
    }

    public void SetEndingIndex(int index)
    {
        currentEndingIndex = index;
    }

    public MotherLine[] GetCurrentResponse()
    {
        RoundData currentRound = rounds[round];

        return currentRound.responses[currentEndingIndex].lines;
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