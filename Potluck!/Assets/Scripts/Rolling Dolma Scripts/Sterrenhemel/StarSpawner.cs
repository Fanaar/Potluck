using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class StarSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EndingMover
    {
        public Transform target;

        public Vector3 moveOffset = new Vector3(0, -5f, 0);

        public float moveDuration = 3f;

        public float rotationSpeed = 30f;
    }

    public GameObject starPrefab;
    public int maxStars = 5;

    public MoveBetweenPoints[] movers;
    public Collider2D spawnArea;

    private int currentStars = 0;

    public FinishBranchScene finishScene;

    [Header("TMP Texts In Order")]
    public TextMeshProUGUI[] textsToFade;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    [Header("Ending Objects")]
    public List<EndingMover> endingMovers =
        new List<EndingMover>();

    [Header("Finish Delay")]
    public float delayBeforeFinish = 2f;

    void Start()
    {
        foreach (TextMeshProUGUI txt in textsToFade)
        {
            if (txt != null)
            {
                Color c = txt.color;
                c.a = 0f;
                txt.color = c;
            }
        }
    }

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
        if (movers == null || movers.Length == 0)
            return false;

        foreach (var m in movers)
        {
            if (m == null || !m.hasFinishedMoving)
                return false;
        }

        return true;
    }

    bool IsClickInsideSpawnArea()
    {
        if (spawnArea == null)
            return false;

        Vector2 worldPoint =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return spawnArea.OverlapPoint(worldPoint);
    }

    void SpawnStar()
    {
        Vector3 worldPosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        worldPosition.z = 0f;

        Instantiate(starPrefab, worldPosition, Quaternion.identity);

        // fade juiste tekst in
        if (currentStars < textsToFade.Length)
        {
            StartCoroutine(FadeText(textsToFade[currentStars]));
        }

        currentStars++;

        // klaar?
        if (currentStars >= maxStars)
        {
            StartCoroutine(PlayEndingSequence());
        }
    }

    IEnumerator FadeText(TextMeshProUGUI text)
    {
        float timer = 0f;

        Color startColor = text.color;
        startColor.a = 0f;

        Color endColor = text.color;
        endColor.a = 1f;

        text.color = startColor;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            text.color = Color.Lerp(
                startColor,
                endColor,
                timer / fadeDuration
            );

            yield return null;
        }

        text.color = endColor;
    }

    IEnumerator PlayEndingSequence()
    {
        List<Vector3> startPositions =
            new List<Vector3>();

        foreach (var mover in endingMovers)
        {
            if (mover.target != null)
            {
                startPositions.Add(mover.target.position);
            }
            else
            {
                startPositions.Add(Vector3.zero);
            }
        }

        float longestDuration = 0f;

        foreach (var mover in endingMovers)
        {
            if (mover.moveDuration > longestDuration)
            {
                longestDuration = mover.moveDuration;
            }
        }

        float timer = 0f;

        // ===== MOVE + ROTATE =====
        while (timer < longestDuration)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < endingMovers.Count; i++)
            {
                EndingMover mover = endingMovers[i];

                if (mover.target == null)
                    continue;

                float t = Mathf.Clamp01(
                    timer / mover.moveDuration
                );

                Vector3 startPos = startPositions[i];
                Vector3 endPos =
                    startPos + mover.moveOffset;

                // bewegen
                mover.target.position =
                    Vector3.Lerp(startPos, endPos, t);

                // roteren
                mover.target.Rotate(
                    0f,
                    0f,
                    mover.rotationSpeed * Time.deltaTime
                );
            }

            yield return null;
        }

        // ===== BLIJF ROTEREN TOT FINISH =====
        float waitTimer = 0f;

        while (waitTimer < delayBeforeFinish)
        {
            waitTimer += Time.deltaTime;

            foreach (var mover in endingMovers)
            {
                if (mover.target == null)
                    continue;

                mover.target.Rotate(
                    0f,
                    0f,
                    mover.rotationSpeed * Time.deltaTime
                );
            }

            yield return null;
        }

        // ===== FINISH =====
        if (finishScene != null)
        {
            finishScene.FinishScene();
        }
    }
}
