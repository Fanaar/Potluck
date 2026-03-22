using UnityEngine;

[CreateAssetMenu(menuName = "Dolma/Round")]
public class RoundData : ScriptableObject
{
    [System.Serializable]
    public class Response
    {
        public MotherLine[] lines; // 👈 GOED
    }

    public Response[] responses;
}