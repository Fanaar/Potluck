using UnityEngine;
using UnityEngine.EventSystems;

public class BookHotspotCursor :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CustomCursorUI.Instance != null)
        {
            CustomCursorUI.Instance.SetHover();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CustomCursorUI.Instance != null)
        {
            CustomCursorUI.Instance.SetDefault();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CustomCursorUI.Instance != null)
        {
            CustomCursorUI.Instance.SetGrab();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (CustomCursorUI.Instance != null)
        {
            CustomCursorUI.Instance.SetHover();
        }
    }
}