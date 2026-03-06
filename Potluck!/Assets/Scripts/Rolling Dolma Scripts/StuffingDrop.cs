using UnityEngine;

public class StuffingDrop : MonoBehaviour
{
    public bool wasPlaced = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Leaf"))
        {
            DolmaRollController roll = other.GetComponent<DolmaRollController>();

            if (roll != null)
            {
                roll.AddStuffing();
                UIQuestionPreview.Instance.LockQuestion();
            }

            wasPlaced = true;
            Destroy(gameObject);
        }
    }
}