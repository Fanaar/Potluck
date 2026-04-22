using UnityEngine;

public class ControlsToggleUI : MonoBehaviour
{
    [Header("UI Panel (Controls / Tutorial)")]
    [SerializeField] private GameObject controlsPanel;

    private bool isOpen = true; // start zichtbaar

    void Start()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(true);
        }

        // Cursor zichtbaar bij start (optioneel)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleControls();
        }
    }

    public void ToggleControls()
    {
        isOpen = !isOpen;

        if (controlsPanel != null)
        {
            controlsPanel.SetActive(isOpen);
        }

        // Cursor gedrag
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}