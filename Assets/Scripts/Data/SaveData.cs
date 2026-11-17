using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int version = 1;
    public float bestTime;
    public int completedLevels;
    public List<int> unlockedLevels = new List<int> { 1 };
    public float musicVolume = 1f;
    public float soundVolume = 1f;
    public float masterVolume = 1f;
}
