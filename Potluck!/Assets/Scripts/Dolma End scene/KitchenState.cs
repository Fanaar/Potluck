using UnityEngine;

public class KitchenState : MonoBehaviour
{
    public static KitchenState Instance;

    [HideInInspector] public bool lidRemoved = false;
    [HideInInspector] public bool panRemoved = false;
    [HideInInspector] public bool stoveOff = false;

    [Header("Cinematic")]
    public StoveCinematic cinematic;

    private bool cinematicStarted = false;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckCompletion()
    {
        if (cinematicStarted)
            return;

        if (lidRemoved &&
            panRemoved &&
            stoveOff)
        {
            cinematicStarted = true;

            if (cinematic != null)
            {
                cinematic.StartCinematic();
            }
        }
    }
}