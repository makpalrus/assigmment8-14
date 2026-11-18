using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static string FilePath
    {
        get { return Path.Combine(Application.persistentDataPath, "save.json"); }
    }

    public static SaveData Load()
    {
        if (!File.Exists(FilePath))
            return new SaveData();

        try
        {
            string json = File.ReadAllText(FilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            if (data == null || data.version != 1 || data.unlockedLevels == null ||
                !data.unlockedLevels.Contains(1) || data.bestScore < 0 ||
                data.completedLevels < 0 || data.completedLevels > 3 ||
                float.IsNaN(data.musicVolume) || float.IsNaN(data.soundVolume) ||
                float.IsInfinity(data.musicVolume) || float.IsInfinity(data.soundVolume) ||
                data.musicVolume < 0f || data.musicVolume > 1f ||
                data.soundVolume < 0f || data.soundVolume > 1f ||
                data.unlockedLevels.Exists(level => level < 1 || level > 3))
                throw new InvalidDataException("Invalid save values.");

            if (InvalidTime(data.lastRunTime) || InvalidTime(data.previousRunTime) ||
                InvalidTime(data.bestRunTime) || data.lastRunScore < 0 || data.finishedRuns < 0)
                throw new InvalidDataException("Invalid run result.");
            return data;
        }
        catch (Exception error)
        {
            Debug.LogWarning("Save reset to defaults. " + error.Message);
            return new SaveData();
        }
    }

    static bool InvalidTime(float seconds)
    {
        return seconds < 0f || float.IsNaN(seconds) || float.IsInfinity(seconds);
    }

    public static bool Save(SaveData data)
    {
        string temporary = Path.Combine(Application.persistentDataPath, "save.tmp");
        try
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            File.WriteAllText(temporary, JsonUtility.ToJson(data, true));
            if (File.Exists(FilePath))
                File.Replace(temporary, FilePath, null);
            else
                File.Move(temporary, FilePath);
            return true;
        }
        catch (Exception error)
        {
            Debug.LogWarning("Could not save progress. " + error.Message);
            return false;
        }
    }
}
