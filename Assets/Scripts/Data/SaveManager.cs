using UnityEngine;

[DefaultExecutionOrder(-4000)]
public class SaveManager : MonoBehaviour
{
    static SaveManager instance;
    public SaveData Data { get; private set; }

    public static SaveManager Instance
    {
        get
        {
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

    public void LevelCompleted(LevelData level)
    {
        Data.completedLevels = Mathf.Max(Data.completedLevels, level.number);
        if (level.nextLevel > 0 && !Data.unlockedLevels.Contains(level.nextLevel))
            Data.unlockedLevels.Add(level.nextLevel);
        Save();
    }

    public bool FinishRun(float seconds)
    {
        if (seconds <= 0f) return false;
        if (Data.bestTime > 0f && seconds >= Data.bestTime) return false;
        Data.bestTime = seconds;
        Save();
        return true;
    }
    public void Save()
    {
        SaveSystem.Save(Data);
    }

    public bool ResetProgress()
    {
        SaveData defaults = new SaveData();
        if (!SaveSystem.Delete())
            return false;

        Data = defaults;
        if (GameAudio.Instance) GameAudio.Instance.ApplyVolume();
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
