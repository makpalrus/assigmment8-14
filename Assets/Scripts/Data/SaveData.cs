using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int version = 1;
    public int bestScore;
    public int completedLevels;
    public List<int> unlockedLevels = new List<int> { 1 };
    public float musicVolume = 1f;
    public float soundVolume = 1f;
    public float lastRunTime;
    public float previousRunTime;
    public float bestRunTime;
    public int lastRunScore;
    public int finishedRuns;
}
