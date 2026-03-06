using UnityEngine;
using System.Collections;

public class CameraPanController : MonoBehaviour
{
    public static CameraPanController Instance;

    public Transform playerPos;
    public Transform motherPos;

    [Header("Camera Pan Settings")]
    public float panDuration = 1.5f;
    public AnimationCurve panCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Camera cam;
    Coroutine currentPan;

    void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    public void PanToMother()
    {
        Debug.Log("Panning to mother");

        if (currentPan != null)
            StopCoroutine(currentPan);

        currentPan = StartCoroutine(MoveCamera(motherPos.position));
    }

    public void PanToPlayer()
    {
        Debug.Log("Panning to player");

        if (currentPan != null)
            StopCoroutine(currentPan);

        currentPan = StartCoroutine(MoveCamera(playerPos.position));
    }

    IEnumerator MoveCamera(Vector3 target)
    {
        Vector3 start = cam.transform.position;

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