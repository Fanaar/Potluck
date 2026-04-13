using UnityEngine;

public class DoughInteraction : MonoBehaviour
{
    private DoughController dough;

    void Awake()
    {
        dough = GetComponent<DoughController>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        RollingPinController pin = other.GetComponent<RollingPinController>();

        if (pin != null && pin.IsPickedUp())
        {
            Vector2 movement = pin.GetMovementDelta();
            dough.Roll(movement);
        }
    }
}