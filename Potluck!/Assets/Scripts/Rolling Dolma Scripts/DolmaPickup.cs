using UnityEngine;
using System.Collections;

public class DolmaPickup : MonoBehaviour
{
    bool dragging = false;
    bool placed = false;

    void OnMouseDown()
    {
        if (placed) return;

        dragging = true;
    }

    void OnMouseUp()
    {
        dragging = false;
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
            {
                return;
            }

            PlaceOnPlate(snapPoint);
        }
    }

    void PlaceOnPlate(Transform snapPoint)
    {

        placed = true;
        dragging = false;

        StartCoroutine(SnapToPlate(snapPoint.position));

        CameraPanController.Instance.PanToMother();
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
}