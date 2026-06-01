using UnityEngine;

public class WordHoverManager : MonoBehaviour
{
    public int totalWords = 5;

    private int hoveredWords = 0;

    [Header("Activate After All Words")]
    public GameObject targetToActivate;

    public void WordHovered()
    {
        hoveredWords++;

        Debug.Log("Hovered words: " + hoveredWords);

        if (hoveredWords >= totalWords)
        {
            if (targetToActivate != null)
            {
                targetToActivate.SetActive(true);

                Debug.Log("Alle woorden geraakt → target actief");
            }
        }
    }
}