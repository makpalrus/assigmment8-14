using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button playButton, quitButton;
    public GameObject levelSelect, confirmPanel;
    public Button[] levelButtons;
    public Image[] levelIcons;
    public Sprite playIcon, lockIcon;
    public Text bestTimeText;

    void Awake()
    {
        Time.timeScale = 1f;
        levelSelect.SetActive(false);
    }

    public void Play()
    {
        bestTimeText.text = "BEST SCORE  " + GameUI.FormatTime(SaveManager.Instance.Data.bestTime);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool unlocked = SaveManager.Instance.Data.unlockedLevels.Contains(i + 1);
            levelButtons[i].interactable = unlocked;
            if (unlocked) levelIcons[i].sprite = playIcon;
            else levelIcons[i].sprite = lockIcon;
        }
        confirmPanel.SetActive(false);
        levelSelect.SetActive(true);
    }

    public void Click() { GameAudio.Instance.Click(); }
    public void SelectLevel(int number) { LevelLoader.StartLevel(number); }
    public void Back() { levelSelect.SetActive(false); }
    public void AskReset() { confirmPanel.SetActive(true); }
    public void CancelReset() { confirmPanel.SetActive(false); }

    public void ConfirmReset()
    {
        if (SaveManager.Instance.ResetProgress()) SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
