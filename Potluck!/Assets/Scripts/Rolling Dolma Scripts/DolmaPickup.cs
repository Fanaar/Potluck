using UnityEngine;

public class DolmaPickup : MonoBehaviour
{
    bool dragging = false;

    void OnMouseDown()
    {
        dragging = true;
        Debug.Log("Dolma drag start");
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
}