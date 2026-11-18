using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int number;
    public string title;
    public GameObject layoutPrefab;
    public Vector3 playerSpawn;
    public PlayerStats playerStats;
    public ItemStats itemStats;
    public EnemySpawn[] enemies;
    public int goalAmount;
    public float timeLimit;
    public int nextLevel;
}

public enum EnemyType { Patroller, Shooter, Charger, Bird }

[System.Serializable]
public class EnemySpawn
{
    public EnemyType type;
    public Vector3 position;
    public Vector3 scale;
    public Sprite artwork;
    public Color color;
    public EnemyStats stats;
    public LayerMask walls;
}
