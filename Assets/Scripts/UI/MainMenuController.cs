using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public string levelScene = "SampleScene";
    public Button playButton;
    public Button quitButton;
    GameObject levelSelect;

    void Awake()
    {
        Time.timeScale = 1f;
        playButton.onClick.AddListener(Play);
        quitButton.onClick.AddListener(Quit);
        levelSelect = LevelSelectUI.Create(playButton, quitButton);
    }
    public void Play()
    {
        Time.timeScale = 1f;
        levelSelect.SetActive(true);
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
