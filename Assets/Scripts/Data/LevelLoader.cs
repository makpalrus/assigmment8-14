using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[DefaultExecutionOrder(-2000)]
public class LevelLoader : MonoBehaviour
{
    public static LevelData Current;
    public static int SelectedLevel = 1;
    static int completedInRun;
    public static bool NewRecord;
    public static bool FullRun;
    public static bool RunActive;
    public LevelData[] levels;
    GameUI ui;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        Current = null;
        SelectedLevel = 1;
        completedInRun = 0;
        NewRecord = false;
        FullRun = false;
        RunActive = false;
        GameUI.TotalTime = 0f;
    }

    public static void StartRun()
    {
        StartLevel(1);
    }

    public static void StartLevel(int number)
    {
        if (!SaveManager.Instance.Data.unlockedLevels.Contains(number)) return;
        completedInRun = 0;
        NewRecord = false;
        FullRun = false;
        GameUI.TotalTime = 0f;
        RunActive = true;
        Open(number);
    }

    public static void Open(int number)
    {
        SelectedLevel = number;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public static void CompleteLevel()
    {
        if (!RunActive) return;
        if (Current.number == completedInRun + 1) completedInRun++;
        SaveManager.Instance.LevelCompleted(Current);
        if (Current.nextLevel > 0) Open(Current.nextLevel);
        else
        {
            RunActive = false;
            FullRun = completedInRun == 3;
            if (FullRun) NewRecord = SaveManager.Instance.FinishRun(GameUI.TotalTime);
            GameUI.Instance.ShowVictory();
        }
    }

    public static void EndRun()
    {
        RunActive = false;
    }

    void Awake()
    {
        Current = levels[SelectedLevel - 1];
        RunActive = true;
        GameObject layout = Instantiate(Current.layoutPrefab);
        ui = layout.GetComponentInChildren<GameUI>();
        ui.player.transform.position = Current.playerSpawn;
        ui.player.GetComponent<PlayerHealth>().SetSpawn(Current.playerSpawn);
    }

    IEnumerator Start()
    {
        int point = 0;
        foreach (Wave wave in Current.waves)
        {
            if (wave.delay > 0f) yield return new WaitForSeconds(wave.delay);
            for (int i = 0; i < wave.count; i++)
            {
                Vector2 position = Current.spawnPoints[point % Current.spawnPoints.Length];
                GameObject enemy = Instantiate(wave.enemyPrefab, position, Quaternion.identity);
                EnemyBrain brain = enemy.GetComponent<EnemyBrain>();
                if (brain != null) brain.SetTarget(ui.player, ui.view);
                Patroller patroller = enemy.GetComponent<Patroller>();
                if (patroller != null) patroller.SetWaypoints(ui.patrolPoints);
                point++;
            }
        }
    }
}
