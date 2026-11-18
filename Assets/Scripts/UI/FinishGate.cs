using UnityEngine;

public class FinishGate : MonoBehaviour
{
    public TextMesh message;
    SpriteRenderer flag;
    bool finished;

    void Awake()
    {
        flag = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GameUI ui = GameUI.Instance;
        bool ready = ui.Collected >= ui.TotalItems;
        flag.color = ready ? new Color32(100, 255, 150, 255) : new Color32(160, 160, 175, 255);
        if (!ready) message.text = "COLLECT ALL COINS";
        else if (LevelLoader.Current.nextLevel > 0) message.text = "LEVEL " + LevelLoader.Current.nextLevel + " >";
        else message.text = "FINISH!";
    }

    void OnTriggerStay2D(Collider2D other)
    {
        GameUI ui = GameUI.Instance;
        if (finished || !ui.IsPlaying || ui.Collected < ui.TotalItems) return;
        if (other.GetComponentInParent<Player>() == null) return;
        finished = true;
        if (LevelLoader.Current.nextLevel > 0) GameAudio.Instance.PlayFinish();
        LevelLoader.CompleteLevel();
    }
}
