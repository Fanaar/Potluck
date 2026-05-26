using UnityEngine;
using System.Collections;

public class DolmaPickup : MonoBehaviour
{
    bool dragging = false;
    bool placed = false;

    [Header("Scene References")]
    public CameraPanController cameraPan;

    void OnMouseEnter()
    {
        if (placed) return;

        CustomCursorUI.Instance.SetHover();
    }

    void OnMouseExit()
    {
        if (placed) return;

        if (!dragging)
        {
            CustomCursorUI.Instance.SetDefault();
        }
    }

    void OnMouseDown()
    {
        if (placed) return;

        dragging = true;

        CustomCursorUI.Instance.SetGrab();
    }

    void OnMouseUp()
    {
        dragging = false;

        if (!placed)
        {
            CustomCursorUI.Instance.SetHover();
        }
    }

    void Update()
    {
        if (!dragging) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        transform.position = mousePos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (placed) return;

        if (other.CompareTag("Plate"))
        {
            Transform snapPoint = other.transform.Find("DolmaSnapPoint");

            if (snapPoint == null)
                return;

            PlaceOnPlate(snapPoint);
        }
    }

    void PlaceOnPlate(Transform snapPoint)
    {
        placed = true;
        dragging = false;

        CustomCursorUI.Instance.SetDefault();

        StartCoroutine(SnapToPlate(snapPoint.position));
        StartCoroutine(HandleMotherSequence());
    }

    IEnumerator SnapToPlate(Vector3 target)
    {
        Vector3 start = transform.position;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 6f;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }

    IEnumerator HandleMotherSequence()
    {
        if (cameraPan != null)
        {
            cameraPan.PanToMother();
            yield return new WaitForSeconds(cameraPan.panDuration);
        }

        MotherSequencePlayer player = null;

        // wacht tot player bestaat EN ready is
        while (player == null || !player.IsReady)
        {
            player = FindObjectOfType<MotherSequencePlayer>();
            yield return null;
        }

        MotherLine[] lines = RoundManager.Instance.GetCurrentResponse();

        if (lines != null && lines.Length > 0)
        {
            player.PlaySequence(lines);
        }
    }
}