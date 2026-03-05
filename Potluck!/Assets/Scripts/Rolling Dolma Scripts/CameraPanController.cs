using UnityEngine;

public class CameraPanController : MonoBehaviour
{
    public static CameraPanController Instance;

    public Transform playerPos;
    public Transform motherPos;

    void Awake()
    {
        Instance = this;
    }

    public void PanToMother()
    {
        StartCoroutine(MoveCamera(motherPos.position));
    }

    public void PanToPlayer()
    {
        StartCoroutine(MoveCamera(playerPos.position));
    }

    System.Collections.IEnumerator MoveCamera(Vector3 target)
    {
        float t = 0;

        Vector3 start = Camera.main.transform.position;

        while (t < 1)
        {
            t += Time.deltaTime;

            Camera.main.transform.position =
                Vector3.Lerp(start, target, t);

            yield return null;
        }
    }
}