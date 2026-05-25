using UnityEngine;
using UnityEngine.UI;

public class CustomCursorUI : MonoBehaviour
{
    public static CustomCursorUI Instance;

    [Header("UI")]
    public Image cursorImage;

    [Header("Cursor Sprites")]
    public Sprite defaultCursor;
    public Sprite hoverCursor;
    public Sprite grabCursor;

    [Header("Cursor Offset")]
    public Vector2 cursorOffset;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Cursor.visible = false;

        SetDefault();
    }

    void Update()
    {
        transform.position =
            (Vector2)Input.mousePosition + cursorOffset;
    }

    public void SetDefault()
    {
        cursorImage.sprite = defaultCursor;
    }

    public void SetHover()
    {
        cursorImage.sprite = hoverCursor;
    }

    public void SetGrab()
    {
        cursorImage.sprite = grabCursor;
    }
}