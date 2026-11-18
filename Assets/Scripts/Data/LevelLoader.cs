using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-2000)]
public class LevelLoader : MonoBehaviour
{
    public static LevelData Current { get; private set; }
    public static LevelLoader Instance { get; private set; }
    public static int SelectedLevel { get; private set; } = 1;
    public static float RunTime { get; private set; }
    public static int RunScore { get; private set; }
    public static bool RunActive { get; private set; }
    public float Elapsed { get; private set; }
    UnityEngine.UI.Text timerLabel;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        Current = null;
        Instance = null;
        SelectedLevel = 1;
        RunTime = 0f;
        RunScore = 0;
        RunActive = false;
    }

    public static LevelData GetLevel(int number)
    {
        return Resources.Load<LevelData>("Data/Level_" + number.ToString("00"));
    }

    public static LevelData ActiveData
    {
        get
        {
            if (SceneManager.GetActiveScene().name == "Game" && Current)
                return Current;

            string scene = SceneManager.GetActiveScene().name;
            int number = scene == "Level2" ? 2 : scene == "Level3" ? 3 : 1;
            return GetLevel(number);
        }
    }

    public static void StartRun()
    {
        SelectedLevel = 1;
        RunTime = 0f;
        RunScore = 0;
        RunActive = true;
        Open(1);
    }

    public static void Open(int number)
    {
        if (!RunActive || number != SelectedLevel)
            return;

        SelectedLevel = number;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public static void CompleteLevel(int score)
    {
        if (!RunActive)
            return;

        RunScore += score;
        SaveManager.Instance.LevelCompleted(Current);
        if (Current.nextLevel > 0)
        {
            SelectedLevel = Current.nextLevel;
            Open(SelectedLevel);
            return;
        }

        SaveManager.Instance.RunCompleted(RunScore, RunTime);
        RunActive = false;
        MobileGameUI.Instance.ShowVictory();
    }

    public static void EndRun()
    {
        RunActive = false;
    }

    void Awake()
    {
        Instance = this;
        Current = GetLevel(SelectedLevel);
        if (!Current || !Current.layoutPrefab)
            throw new System.InvalidOperationException("Level data or layout is missing.");

        GameObject layout = Instantiate(Current.layoutPrefab);
        MobileGameUI ui = layout.GetComponentInChildren<MobileGameUI>();
        timerLabel = LevelSelectUI.CreateTimer(ui.counter);
        Player player = layout.GetComponentInChildren<Player>();
        player.transform.position = Current.playerSpawn;
        player.GetComponent<PlayerHealth>().SetSpawn(Current.playerSpawn);

        foreach (EnemySpawn spawn in Current.enemies)
        {
            GameObject enemy = new GameObject(spawn.type.ToString());
            enemy.transform.position = spawn.position;
            enemy.transform.localScale = spawn.scale;
            if (spawn.type == EnemyType.Bird)
            {
                enemy.tag = "Damage";
                enemy.AddComponent<FlyingEnemy>().SetArtwork(spawn.artwork);
                continue;
            }

            EnemyBrain brain;
            switch (spawn.type)
            {
                case EnemyType.Patroller: brain = enemy.AddComponent<Patroller>(); break;
                case EnemyType.Shooter: brain = enemy.AddComponent<Shooter>(); break;
                default: brain = enemy.AddComponent<Charger>(); break;
            }
            brain.Configure(spawn);
        }
    }

    void Update()
    {
        if (!MobileGameUI.Instance || !MobileGameUI.Instance.IsPlaying)
            return;

        Elapsed += Time.deltaTime;
        if (RunActive)
            RunTime += Time.deltaTime;
        timerLabel.text = "TIME  " + FormatTime(RunTime);
        if (Current.timeLimit > 0f && Elapsed >= Current.timeLimit)
            MobileGameUI.Instance.ShowGameOver();
    }

    public static string FormatTime(float seconds)
    {
        double minutes = System.Math.Floor(seconds / 60d);
        int remainder = (int)(seconds % 60f);
        int hundredths = (int)((seconds % 1f) * 100f);
        return minutes.ToString("00") + ":" + remainder.ToString("00") + "." + hundredths.ToString("00");
    }
}
