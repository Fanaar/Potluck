using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public TextMeshProUGUI dialogueText;

    void Awake()
    {
        Instance = this;
        dialogueText.gameObject.SetActive(false);
    }

    public void ShowText(string text)
    {
        dialogueText.gameObject.SetActive(true);
        dialogueText.text = text;
    }

    public void Hide()
    {
        dialogueText.gameObject.SetActive(false);
    }
}