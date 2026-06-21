using UnityEngine;

public class StuffingDrop : MonoBehaviour
{
    public bool wasPlaced = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Leaf"))
            return;

        DolmaRollController roll = other.GetComponent<DolmaRollController>();

        if (roll == null)
            return;

        // probeer stuffing toe te voegen
        if (roll.HasStuffing())
            return;

        roll.AddStuffing();
        UIQuestionPreview.Instance.LockQuestion();

        wasPlaced = true;
        Destroy(gameObject);
    }
}