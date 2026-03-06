using UnityEngine;

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

        transform.position = snapPoint.position;

        CameraPanController.Instance.PanToMother();
    }
}