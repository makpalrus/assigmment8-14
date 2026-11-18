using UnityEngine;

public class SaveManager : MonoBehaviour
{
    static SaveManager instance;
    public SaveData Data { get; private set; }
    public bool LastRunWasRecord { get; private set; }

    public static SaveManager Instance
    {
        get
        {
            if (!instance)
                new GameObject("Save Manager").AddComponent<SaveManager>();
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        instance = null;
    }

    void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        Data = SaveSystem.Load();
    }

    public void RunCompleted(int score, float seconds)
    {
        float previousBest = Data.bestRunTime;
        bool newRecord = previousBest == 0f || seconds < previousBest;
        LastRunWasRecord = newRecord;
        Data.previousRunTime = Data.lastRunTime;
        Data.lastRunTime = seconds;
        Data.lastRunScore = score;
        Data.finishedRuns++;
        if (newRecord)
            Data.bestRunTime = seconds;
        Data.bestScore = Mathf.Max(Data.bestScore, score);
        Save();

    }

    public void LevelCompleted(LevelData level)
    {
        Data.completedLevels = Mathf.Max(Data.completedLevels, level.number);
        if (level.nextLevel > 0 && !Data.unlockedLevels.Contains(level.nextLevel))
            Data.unlockedLevels.Add(level.nextLevel);
        Save();
    }

    public void Save()
    {
        SaveSystem.Save(Data);
    }

    public bool ResetProgress()
    {
        SaveData defaults = new SaveData();
        if (!SaveSystem.Save(defaults))
            return false;

        Data = defaults;
        LastRunWasRecord = false;
        PlayerPrefs.DeleteKey("Skybound.MusicVolume");
        PlayerPrefs.DeleteKey("Skybound.SoundVolume");
        PlayerPrefs.Save();
        if (GameAudio.Instance)
        {
            GameAudio.Instance.SetMusicVolume(Data.musicVolume);
            GameAudio.Instance.SetSoundVolume(Data.soundVolume);
        }
        return true;
    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
            Save();
    }

    void OnApplicationFocus(bool focused)
    {
        if (!focused)
            Save();
    }
}
