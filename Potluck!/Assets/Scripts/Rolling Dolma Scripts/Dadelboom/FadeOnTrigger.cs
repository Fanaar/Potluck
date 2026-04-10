using UnityEngine;
using System.Collections;

public class FadeOnTrigger : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool isFading = false;
    public FinishBranchScene finishScene;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Debug.Log("FadeOnTrigger gestart op: " + gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger geraakt met: " + other.name);

        if (other.CompareTag("Target"))
        {
            Debug.Log("Target geraakt → start fade");

            if (!isFading)
            {
                StartCoroutine(FadeOut());
            }
        }
        else
        {
            Debug.Log("Geen juiste tag: " + other.tag);
        }
    }

    IEnumerator FadeOut()
    {
        isFading = true;

        float duration = 1f;
        float time = 0f;
        Color startColor = sr.color;

        Debug.Log("Fade gestart");

        while (time < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / duration);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            time += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(startColor.r, startColor.g, startColor.b, 0f);

        Debug.Log("Fade klaar");

        finishScene.FinishScene();
    }
}