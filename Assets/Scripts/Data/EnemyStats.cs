using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Game/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public float visionRange;
    public float visionAngle;
    public float windUp;
    public float cooldown;
    public int damage;
    public float patrolSpeed;
    public float chaseSpeed;
    public float strikeReach;
    public Vector2 waypointOffsets;
    public float scanInterval;
    public float projectileSpeed;
    public float projectileLifetime;
    public float projectileScale;
    public float dashSpeed;
    public float dashDistance;
    public float laneTolerance;
    public float hitReach;
}
