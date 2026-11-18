using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobileGameUI : MonoBehaviour
{
    public static MobileGameUI Instance { get; private set; }
    public const string MainMenuScene = "MainMenu";
    public GameObject mainMenu, hud, pauseMenu, gameOver;
    public Button playButton, quitButton, pauseButton, resumeButton, restartButton, menuButton, retryButton, gameOverMenuButton;
    public Text counter;
    public HeartGraphic[] hearts;
    public Player player;
    public bool IsPlaying { get; private set; }
    public int Collected { get; private set; }
    public int TotalItems { get; private set; }
    public int Score { get; private set; }
    InputAction pauseAction;
    void Awake()
    {
        Instance = this;
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        menuButton.onClick.AddListener(Menu);
        PauseAudioControls.Create(resumeButton, restartButton, menuButton, GameAudio.Instance);
        ForestUI.StyleButton(pauseButton);
        ((RectTransform)pauseButton.transform).sizeDelta = new Vector2(240, 110);
        ForestUI.AddIcon(pauseButton.transform, MenuIcon.Pause, new Vector2(-65, 0), 32, Color.white);
        pauseButton.GetComponentInChildren<Text>().rectTransform.anchoredPosition = new Vector2(20, 0);
        pauseButton.GetComponentInChildren<Text>().fontSize = 32;
        ForestUI.StyleButton(retryButton);
        ForestUI.StyleButton(gameOverMenuButton);
        ForestUI.StyleScreen(gameOver);
        SafeArea resultSafe = gameOver.GetComponentInChildren<SafeArea>(true);
        if (resultSafe)
        {
            ForestUI.Panel(resultSafe.transform, new Vector2(1020, 850), Vector2.zero);
            foreach (Text title in resultSafe.GetComponentsInChildren<Text>(true))
            {
                if (title.name == "Title")
                    ForestUI.Header(title, new Vector2(0, 280));
            }
        }
        retryButton.onClick.AddListener(Restart);
        gameOverMenuButton.onClick.AddListener(Menu);
        pauseAction = new InputAction("Pause", InputActionType.Button, "<Keyboard>/escape");
        pauseAction.AddBinding("<Gamepad>/start");
        pauseAction.Enable();
        Collected = 0;
        TotalItems = LevelLoader.ActiveData.goalAmount;
        UpdateCounter();
        RefreshHearts(player.GetComponent<PlayerHealth>().maxHearts);
        Play();
    }
    void Update()
    {
        if (!pauseAction.WasPressedThisFrame() || gameOver.activeSelf) return;
        if (IsPlaying) Pause(); else Resume();
    }
    void SetScreen(GameObject screen)
    {
        // Hiding the controls first releases the virtual stick and button.
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
        Score += LevelLoader.ActiveData.itemStats.coinScore;
        UpdateCounter();
        GameAudio.Play(GameSound.Coin);
        if (Collected == TotalItems)
            GameAudio.Play(GameSound.Unlock);
    }
    void UpdateCounter()
    {
        counter.text = Collected + " / " + TotalItems;
    }
    public void ShowVictory()
    {
        EndGame(true);
        foreach (Text label in gameOver.GetComponentsInChildren<Text>(true))
        {
            if (label.name == "Title")
            {
                label.text = "YOU WIN!";
                label.rectTransform.anchoredPosition = new Vector2(0, 315);
            }
        }
        if (LevelLoader.Instance)
        {
            ((RectTransform)retryButton.transform).anchoredPosition = new Vector2(0, -150);
            ((RectTransform)gameOverMenuButton.transform).anchoredPosition = new Vector2(0, -280);
            retryButton.GetComponentInChildren<Text>().text = "PLAY AGAIN";
            LevelSelectUI.ShowResult(gameOver, counter, LevelLoader.RunTime);
        }
    }
    public void RefreshHearts(int count)
    {
        for (int i = 0; i < hearts.Length; i++) hearts[i].color = i < count ? new Color32(255, 101, 127, 255) : new Color32(72, 84, 106, 255);
    }
    public void ShowGameOver()
    {
        EndGame(false);
    }
    void EndGame(bool victory)
    {
        IsPlaying = false;
        SetScreen(gameOver);
        Time.timeScale = 0;
        GameAudio.SetPaused(true);
        GameAudio.Play(victory ? GameSound.Finish : GameSound.Lose);
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
        pauseAction?.Dispose();
        if (Instance == this)
        {
            Instance = null;
            Time.timeScale = 1;
        }
    }
}




