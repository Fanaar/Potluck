using UnityEngine;
using System.Collections;

public class SpriteSwitcher : MonoBehaviour
{
    [Header("Child Objects")]
    [SerializeField] private GameObject spriteA;
    [SerializeField] private GameObject spriteB;

    [Header("Switch Settings")]
    [SerializeField] private float switchInterval = 0.5f;

    private bool showingA = true;

    private void Start()
    {
        // Start met A zichtbaar
        spriteA.SetActive(true);
        spriteB.SetActive(false);

        StartCoroutine(SwitchSprites());
    }

    private IEnumerator SwitchSprites()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchInterval);

            showingA = !showingA;

            spriteA.SetActive(showingA);
            spriteB.SetActive(!showingA);
        }
    }
}
