using UnityEngine;

public class DragDropController : MonoBehaviour
{
    public static DragDropController Instance;

    GameObject draggedObject;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (draggedObject == null) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        draggedObject.transform.position = mousePos;

        if (Input.GetMouseButtonUp(0))
        {
            StopDrag();
        }
    }

    public void StartDrag(GameObject obj)
    {
        draggedObject = obj;
    }

    void StopDrag()
    {
        draggedObject = null;
    }
}