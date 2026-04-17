using UnityEngine;
using System.Collections;

public class TriggerProgressManager : MonoBehaviour
{
    [Header("Progress Settings")]
    public int totalTriggers = 4;
    private int currentTriggers = 0;

    [Header("Final Object")]
    public SpriteRenderer finalObject;
    public float fadeDuration = 1f;

    [Header("Finish Settings")]
    public bool triggerSceneAfterFade = true;
    public float delayBeforeScene = 7f;
    public FinishBranchScene finishSceneScript;

    private bool hasActivatedFinal = false;

    public void RegisterTrigger()
    {
        currentTriggers++;

        Debug.Log("Triggers: " + currentTriggers + "/" + totalTriggers);

        if (currentTriggers >= totalTriggers && !hasActivatedFinal)
        {
            hasActivatedFinal = true;
            StartCoroutine(FinalSequence());
        }
    }

    IEnumerator FinalSequence()
    {
        // 1. Fade in final object
        yield return StartCoroutine(FadeInFinal());

        // 2. Wacht X seconden
        if (triggerSceneAfterFade)
        {
            yield return new WaitForSeconds(delayBeforeScene);

            // 3. Trigger scene switch
            if (finishSceneScript != null)
            {
                finishSceneScript.FinishScene();
            }
            else
            {
                Debug.LogWarning("FinishSceneScript niet gekoppeld!");
            }
        }
    }

    IEnumerator FadeInFinal()
    {
        float time = 0;

        Color color = finalObject.color;
        color.a = 0;
        finalObject.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            color.a = Mathf.Lerp(0, 1, t);
            finalObject.color = color;

            yield return null;
        }

        color.a = 1;
        finalObject.color = color;
    }
}