using UnityEngine;

public class StuffingInteraction : MonoBehaviour
{
    [Header("Question")]
    public string[] questionStarts;

    [Header("Cursor")]
    public Texture2D cursorStuffing;

    [Header("Stuffing")]
    public GameObject stuffingPrefab;

    [Header("Audio")]
    public AudioSource audioSource;

    [Tooltip("Er wordt willekeurig 1 clip afgespeeld")]
    public AudioClip[] pickupClips;

    void OnMouseEnter()
    {
        UIQuestionPreview.Instance.ShowStarts(questionStarts);
    }

    void OnMouseExit()
    {
        UIQuestionPreview.Instance.Hide();
    }

    void OnMouseDown()
    {
        if (DragDropController.Instance.IsDragging())
            return;

        PlayRandomPickupSound();

        string selected = UIQuestionPreview.Instance.GetSelectedStart();
        int index = UIQuestionPreview.Instance.GetCurrentIndex();

        RoundManager.Instance.SetStartIndex(index);

        UIQuestionPreview.Instance.LockQuestion();

        SpawnStuffing();
    }

    void PlayRandomPickupSound()
    {
        if (audioSource == null)
            return;

        if (pickupClips == null || pickupClips.Length == 0)
            return;

        AudioClip randomClip = pickupClips[
            Random.Range(0, pickupClips.Length)
        ];

        audioSource.PlayOneShot(randomClip);
    }

    void SpawnStuffing()
    {
        Vector3 spawnPos = transform.position + Vector3.up * 0.4f;

        GameObject stuffing = Instantiate(
            stuffingPrefab,
            spawnPos,
            Quaternion.identity
        );

        DragDropController.Instance.StartDrag(stuffing);
    }
}