using UnityEngine;
using System.Collections.Generic;

public enum Goal { ReachExit, CollectStars, Survive }

[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public int count = 1;
    public float delay;
}
[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int number;
    public string title;
    public GameObject layoutPrefab;
    public Vector3 playerSpawn;
    public PlayerStats playerStats;
    public ItemStats itemStats;
    public Goal goal = Goal.ReachExit;
    public Vector2[] spawnPoints;
    public List<Wave> waves = new List<Wave>();
    public int goalAmount;
    public float timeLimit;
    public int nextLevel;
}
