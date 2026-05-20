using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SceneAmbience : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip ambienceClip;

    [Header("Fade")]
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;
    public float targetVolume = 1f;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = ambienceClip;
        audioSource.loop = true;
        audioSource.volume = 0f;

        audioSource.Play();

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0;

        while (t < fadeInDuration)
        {
            t += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(
                0,
                targetVolume,
                t / fadeInDuration
            );

            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void FadeOutAndStop()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        float t = 0;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(
                startVolume,
                0,
                t / fadeOutDuration
            );

            yield return null;
        }

        audioSource.Stop();
    }
}