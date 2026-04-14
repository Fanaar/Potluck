using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CameraPanController : MonoBehaviour
{
    [Header("Targets (auto gevonden als leeg)")]
    public Transform playerPos;
    public Transform motherPos;

    [Header("Camera Pan Settings")]
    public float panDuration = 1.5f;
    public AnimationCurve panCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Camera cam;
    private Coroutine currentPan;

    void Awake()
    {
        BindCamera();
    }

    void Start()
    {
        RebindReferences();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindReferences();
    }

    void BindCamera()
    {
        cam = Camera.main;

        if (cam == null)
        {
            Debug.LogWarning("Camera.main not found!");
        }
    }

    void RebindReferences()
    {
        BindCamera();

        if (playerPos == null)
        {
            GameObject playerObj = GameObject.Find("PlayerPos");
            if (playerObj != null)
                playerPos = playerObj.transform;
        }

        if (motherPos == null)
        {
            GameObject motherObj = GameObject.Find("MotherPos");
            if (motherObj != null)
                motherPos = motherObj.transform;
        }

        if (playerPos == null || motherPos == null)
        {
            Debug.LogWarning("CameraPanController: Missing references after rebind!");
        }
    }

    public void PanToMother()
    {
        if (cam == null || motherPos == null) return;

        if (currentPan != null)
            StopCoroutine(currentPan);

        currentPan = StartCoroutine(MoveCamera(motherPos.position));
    }

    public void PanToPlayer()
    {
        if (cam == null || playerPos == null) return;

        if (currentPan != null)
            StopCoroutine(currentPan);

        currentPan = StartCoroutine(MoveCamera(playerPos.position));
    }

    IEnumerator MoveCamera(Vector3 target)
    {
        Vector3 start = cam.transform.position;
        target.z = start.z; // behoud camera depth

        float time = 0f;

        while (time < panDuration)
        {
            time += Time.deltaTime;

            float normalizedTime = time / panDuration;
            float curveValue = panCurve.Evaluate(normalizedTime);

            cam.transform.position = Vector3.Lerp(start, target, curveValue);

            yield return null;
        }

        cam.transform.position = target;
        currentPan = null;
    }
}