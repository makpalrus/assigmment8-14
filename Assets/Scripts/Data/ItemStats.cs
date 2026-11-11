using UnityEngine;

[CreateAssetMenu(fileName = "ItemStats", menuName = "Game/Item Stats")]
public class ItemStats : ScriptableObject
{
    public int heartHealing;
    public float platformTravel;
    public float platformSpeed;
    public float birdTravel;
    public float birdSpeed;
    public float birdBobHeight;
    public float birdBobFrequency;
}
