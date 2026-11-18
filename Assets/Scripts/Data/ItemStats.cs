using UnityEngine;

[CreateAssetMenu(fileName = "ItemStats", menuName = "Game/Item Stats")]
public class ItemStats : ScriptableObject
{
    public int coinScore;
    public int heartHealing;
    public float coinBobHeight;
    public float coinBobFrequency;
    public float platformTravel;
    public float platformSpeed;
    public float birdTravel;
    public float birdSpeed;
    public float birdBobHeight;
    public float birdBobFrequency;
}
