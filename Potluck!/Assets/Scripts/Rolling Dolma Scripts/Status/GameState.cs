using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public int currentRound = 0;
    public int lastChoice = -1;

    public bool returningFromScene = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}