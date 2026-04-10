using UnityEngine;
using UnityEngine.EventSystems;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab;
    public int maxStars = 5;

    public MoveBetweenPoints[] movers;
    public Collider2D spawnArea; // 👈 nieuwe!

    private int currentStars = 0;

    public FinishBranchScene finishScene;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && currentStars < maxStars
            && AllMoversFinished()
            && IsClickInsideSpawnArea()
            && (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
        {
            SpawnStar();
        }
    }

    bool AllMoversFinished()
    {
        if (movers == null || movers.Length == 0) return false;

        foreach (var m in movers)
        {
            if (m == null || !m.hasFinishedMoving)
                return false;
        }
        return true;
    }

    bool IsClickInsideSpawnArea()
    {
        if (spawnArea == null) return false;

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return spawnArea.OverlapPoint(worldPoint);
    }

    void SpawnStar()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;

        Instantiate(starPrefab, worldPosition, Quaternion.identity);
        currentStars++;

        // 👇 check of we klaar zijn
        if (currentStars >= maxStars && finishScene != null)
        {
            finishScene.FinishScene();
        }
    }
}