using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }
    public const string MainMenuScene = "MainMenu";
    public GameObject mainMenu, hud, pauseMenu, gameOver;
    public Button playButton, quitButton, pauseButton, resumeButton, restartButton, menuButton, retryButton, gameOverMenuButton;
    public Text counter;
    public Image[] hearts;
    public Text timerLabel, resultTitle, yourTime, bestTimeText, retryLabel, recordLabel;
    public Player player;
    public Camera view;
    public Transform[] patrolPoints;
    public static float TotalTime;
    float elapsed;
    public bool IsPlaying { get; private set; }
    public int Collected { get; private set; }
    public int TotalItems { get; private set; }
    void Awake()
    {
        Instance = this;
        Collected = 0;
        TotalItems = LevelLoader.Current.goalAmount;
        UpdateCounter();
        RefreshHearts(player.GetComponent<PlayerHealth>().stats.maxHearts);
        Play();
    }
    public void Click()
    {
        GameAudio.Instance.Click();
    }

    void Update()
    {
        if (!IsPlaying) return;
        elapsed += Time.deltaTime;
        TotalTime += Time.deltaTime;
        timerLabel.text = "TIME  " + FormatTime(TotalTime);
        LevelData level = LevelLoader.Current;
        if (level.timeLimit <= 0f || elapsed < level.timeLimit) return;
        if (level.goal == Goal.Survive) LevelLoader.CompleteLevel();
        else ShowGameOver();
    }
    public static string FormatTime(float time)
    {
        if (time <= 0f) return "--:--.--";
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        int hundredths = (int)((time % 1f) * 100f);
        return minutes.ToString("00") + ":" + seconds.ToString("00") + "." + hundredths.ToString("00");
    }
    void SetScreen(GameObject screen)
    {
        hud.SetActive(false);
        pauseMenu.SetActive(screen == pauseMenu);
        gameOver.SetActive(screen == gameOver);
        hud.SetActive(screen == hud);
        player.ClearInput();
    }

    public void Play()
    {
        SetScreen(hud);
        Time.timeScale = 1;
        IsPlaying = true;
        GameAudio.SetPaused(false);
    }
    public void Pause()
    {
        if (!IsPlaying) return;
        IsPlaying = false;
        SetScreen(pauseMenu);
        Time.timeScale = 0;
        GameAudio.SetPaused(true);
    }
    public void Resume()
    {
        if (!pauseMenu.activeSelf) return;
        SaveManager.Instance.Save();
        Play();
    }
    public void Restart()
    {
        if (!LevelLoader.RunActive)
        {
            LevelLoader.StartRun();
            return;
        }
        Reload(SceneManager.GetActiveScene().name);
    }
    public void Menu()
    {
        LevelLoader.EndRun();
        Reload(MainMenuScene);
    }
    void Reload(string sceneName)
    {
        SaveManager.Instance.Save();
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
    public void AddItem()
    {
        if (!IsPlaying)
            return;

        Collected++;
        UpdateCounter();
        GameAudio.Instance.PlayCoin();
        if (Collected == TotalItems)
        {
            GameAudio.Instance.PlayUnlock();
            if (LevelLoader.Current.goal == Goal.CollectStars)
                LevelLoader.CompleteLevel();
        }
    }
    void UpdateCounter()
    {
        counter.text = Collected + " / " + TotalItems;
    }
    public void ShowVictory()
    {
        EndGame(true);
        resultTitle.text = "YOU WIN!";
        yourTime.text = FormatTime(TotalTime);
        bestTimeText.text = FormatTime(SaveManager.Instance.Data.bestTime);
        retryLabel.text = "PLAY AGAIN";
        recordLabel.text = "";
        if (LevelLoader.NewRecord) recordLabel.text = "NEW RECORD!";
        if (!LevelLoader.FullRun) recordLabel.text = "START FROM LEVEL 1 FOR A RECORD";
    }
    public void RefreshHearts(int count)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < count) hearts[i].color = new Color32(255, 101, 127, 255);
            else hearts[i].color = new Color32(72, 84, 106, 255);
        }
    }
    public void ShowGameOver()
    {
        resultTitle.text = "TRY AGAIN!";
        yourTime.text = FormatTime(TotalTime);
        recordLabel.text = "";
        bestTimeText.text = FormatTime(SaveManager.Instance.Data.bestTime);
        retryLabel.text = "RESTART";
        EndGame(false);
    }
    void EndGame(bool victory)
    {
        IsPlaying = false;
        SetScreen(gameOver);
        Time.timeScale = 0;
        GameAudio.SetPaused(true);
        if (victory) GameAudio.Instance.PlayFinish();
        else GameAudio.Instance.PlayLose();
    }
    void OnApplicationFocus(bool focused)
    {
        if (!focused && IsPlaying) Pause();
    }
    void OnApplicationPause(bool paused)
    {
        if (paused && IsPlaying) Pause();
    }
    public void Quit()
    {
        Time.timeScale = 1;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            Time.timeScale = 1;
        }
    }
}
