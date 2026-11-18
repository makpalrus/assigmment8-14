using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float moveSpeed;
    public float jumpForce;
    public float groundCheckRadius;
    public int maxHearts;
    public float fallY;
    public float invincibilityTime;
    public float blinkInterval;
    public int hazardDamage;
}
