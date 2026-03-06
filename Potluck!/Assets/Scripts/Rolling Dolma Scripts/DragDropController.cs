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

        // volg muis
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        draggedObject.transform.position = mousePos;

        // loslaten
        if (Input.GetMouseButtonUp(0))
        {
            StopDrag();
        }
    }

    public void StartDrag(GameObject obj)
    {
        draggedObject = obj;
    }

    public bool IsDragging()
    {
        return draggedObject != null;
    }

    public void StopDrag()
    {
        if (draggedObject == null) return;

        StuffingDrop drop = draggedObject.GetComponent<StuffingDrop>();

        if (drop != null && drop.wasPlaced == false)
        {
            Destroy(draggedObject);
            UIQuestionPreview.Instance.UnlockQuestion();
        }

        draggedObject = null;
    }
}