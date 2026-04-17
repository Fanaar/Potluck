using UnityEngine;
using System.Collections;

public class FadeOnTrigger : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool isFading = false;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    [Header("Finish Settings")]
    public FinishBranchScene finishScene;
    public float delayBeforeFinish = 5f;
    public bool useDelay = true;

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

        float time = 0f;
        Color startColor = sr.color;

        Debug.Log("Fade gestart");

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            time += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(startColor.r, startColor.g, startColor.b, 0f);

        Debug.Log("Fade klaar");

        // ⏱️ NIEUW: delay
        if (useDelay)
        {
            yield return new WaitForSeconds(delayBeforeFinish);
        }

        // 🎬 Scene switch
        if (finishScene != null)
        {
            finishScene.FinishScene();
        }
        else
        {
            Debug.LogWarning("FinishScene script niet gekoppeld!");
        }
    }
}