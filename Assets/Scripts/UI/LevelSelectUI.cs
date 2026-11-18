using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class LevelSelectUI
{
    static readonly Color Ink = new Color32(22, 43, 65, 255);
    static readonly Color Blue = new Color32(65, 178, 215, 255);
    static readonly Color Gold = new Color32(255, 197, 71, 255);

    public static GameObject Create(Button play, Button quit)
    {
        Transform parent = play.transform.parent;
        Text template = play.GetComponentInChildren<Text>(true);
        Transform canvas = play.GetComponentInParent<Canvas>().transform;
        RectTransform forest = FullPanel(canvas, "Forest Background", Color.white);
        forest.SetAsFirstSibling();
        Image forestImage = forest.GetComponent<Image>();
        forestImage.sprite = Resources.Load<Sprite>("UI/MenuForest");
        forestImage.raycastTarget = false;
        AspectRatioFitter fit = forest.gameObject.AddComponent<AspectRatioFitter>();
        fit.aspectRatio = forestImage.sprite.rect.width / forestImage.sprite.rect.height;
        fit.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
        Transform home = canvas.Find("Main Menu");
        if (home)
            home.GetComponent<Image>().color = Color.clear;

        foreach (Text text in parent.GetComponentsInChildren<Text>())
        {
            if ((text.text.Contains("Collect") && text.text.Contains("Explore")) ||
                text.text.Contains("joystick") || text.text == "A LITTLE ADVENTURE")
                text.gameObject.SetActive(false);
            if (text.text == "SKYBOUND")
            {
                text.color = new Color32(32, 68, 31, 255);
                text.fontStyle = FontStyle.Bold;
                Shadow shadow = text.gameObject.AddComponent<Shadow>();
                shadow.effectColor = new Color32(255, 247, 200, 180);
                shadow.effectDistance = new Vector2(0, -3);
            }
        }

        quit.gameObject.SetActive(false);
        RectTransform playRect = (RectTransform)play.transform;
        playRect.anchoredPosition = new Vector2(0, -150);
        playRect.sizeDelta = new Vector2(600, 300);
        Image playImage = play.GetComponent<Image>();
        playImage.sprite = Resources.Load<Sprite>("UI/WoodenPlay");
        playImage.type = Image.Type.Simple;
        playImage.preserveAspect = true;
        playImage.color = Color.white;
        template.gameObject.SetActive(false);

        RectTransform screen = FullPanel(parent, "Level Select", new Color(.05f, .12f, .04f, .7f));
        ForestUI.Panel(screen, new Vector2(860, 1000), Vector2.zero);
        Label(Rect(screen, "Title", new Vector2(0, 385), new Vector2(800, 90)), template, "LEVELS", 52, ForestUI.Ink);
        float bestTime = SaveManager.Instance.Data.bestRunTime;
        string bestResult = bestTime > 0f ? "BEST SCORE  " + LevelLoader.FormatTime(bestTime) : "BEST SCORE  --:--";
        Label(Rect(screen, "Best Score", new Vector2(0, 310), new Vector2(800, 55)), template, bestResult, 30, ForestUI.Ink);
        for (int number = 1; number <= 3; number++)
        {
            bool unlocked = number == 1;
            Color cardColor = unlocked ? Blue : new Color32(67, 83, 101, 255);
            Button button = MakeButton(screen, template, "", new Vector2(0, 200 - (number - 1) * 175), new Vector2(620, 145), cardColor);
            button.interactable = unlocked;
            Transform card = button.transform;
            Label(Rect(card, "Level Name", Vector2.zero, new Vector2(430, 60)), template, "LEVEL " + number, 40, Color.white);
            Icon(card, unlocked ? MenuIcon.Play : MenuIcon.Lock, new Vector2(-250, 0), 52, unlocked ? Ink : Color.white);
            if (unlocked)
                button.onClick.AddListener(LevelLoader.StartRun);
        }

        Button back = MakeButton(screen, template, "BACK", new Vector2(-165, -345), new Vector2(290, 100), Blue);
        back.onClick.AddListener(() => screen.gameObject.SetActive(false));
        Button reset = MakeButton(screen, template, "RESET", new Vector2(165, -345), new Vector2(290, 100), new Color32(219, 227, 233, 255));

        RectTransform confirm = FullPanel(screen, "Confirm Reset", new Color(.05f, .12f, .04f, .85f));
        ForestUI.Panel(confirm, new Vector2(1100, 650), Vector2.zero);
        Label(Rect(confirm, "Question", new Vector2(0, 100), new Vector2(850, 220)), template,
            "RESET PROGRESS? Run results, unlocked levels and sound settings will be reset.", 38, ForestUI.Ink);
        Button cancel = MakeButton(confirm, template, "CANCEL", new Vector2(-165, -100), new Vector2(300, 110), Blue);
        Button delete = MakeButton(confirm, template, "DELETE", new Vector2(165, -100), new Vector2(300, 110), new Color32(239, 102, 99, 255));
        cancel.onClick.AddListener(() => confirm.gameObject.SetActive(false));
        delete.onClick.AddListener(() =>
        {
            if (SaveManager.Instance.ResetProgress())
                SceneManager.LoadScene("MainMenu");
        });
        confirm.gameObject.SetActive(false);
        reset.onClick.AddListener(() => confirm.gameObject.SetActive(true));
        screen.gameObject.SetActive(false);
        return screen.gameObject;
    }

    public static Text CreateTimer(Text counter)
    {
        RectTransform panel = Rect(counter.transform.parent.parent, "Level Timer", new Vector2(0, -198), new Vector2(340, 76));
        panel.anchorMin = new Vector2(.5f, 1f);
        panel.anchorMax = panel.anchorMin;
        Image background = panel.gameObject.AddComponent<Image>();
        background.color = new Color32(19, 30, 48, 235);
        background.raycastTarget = false;
        Icon(panel, MenuIcon.Clock, new Vector2(-140, 0), 36, Gold);
        return Label(Rect(panel, "Time", new Vector2(22, 0), new Vector2(290, 65)), counter, "TIME  00:00.00", 31, Color.white);
    }

    public static void ShowResult(GameObject screen, Text template, float seconds)
    {
        SafeArea safeArea = screen.GetComponentInChildren<SafeArea>(true);
        Transform parent = safeArea ? safeArea.transform : screen.transform;
        Label(Rect(parent, "Your Time Label", new Vector2(0, 215), new Vector2(700, 45)), template, "YOUR TIME", 32, ForestUI.Ink);
        Label(Rect(parent, "Your Time", new Vector2(0, 155), new Vector2(800, 80)), template, LevelLoader.FormatTime(seconds), 68, ForestUI.Ink);
        Label(Rect(parent, "Best Score Label", new Vector2(0, 80), new Vector2(700, 45)), template, "BEST SCORE", 30, ForestUI.Ink);
        Label(Rect(parent, "Best Score", new Vector2(0, 25), new Vector2(800, 70)), template,
            LevelLoader.FormatTime(SaveManager.Instance.Data.bestRunTime), 52, ForestUI.Ink);
        if (SaveManager.Instance.LastRunWasRecord)
            Label(Rect(parent, "New Record", new Vector2(0, -50), new Vector2(800, 55)), template,
                "NEW RECORD!", 40, new Color32(67, 125, 23, 255));
    }

    static Button MakeButton(Transform parent, Text template, string title, Vector2 position, Vector2 size, Color color)
    {
        RectTransform rect = Rect(parent, title.Length > 0 ? title : "Level Card", position, size);
        Image background = rect.gameObject.AddComponent<Image>();
        background.color = color;
        Shadow shadow = rect.gameObject.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, .25f);
        shadow.effectDistance = new Vector2(0, -7);
        Button button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = background;
        if (title.Length > 0)
            Label(Rect(rect, "Label", Vector2.zero, size), template, title, 36, Ink);
        ForestUI.StyleButton(button);
        if (title == "DELETE")
            background.color = new Color(1f, .65f, .65f, 1f);
        return button;
    }

    static void Icon(Transform parent, MenuIcon symbol, Vector2 position, float size, Color color)
    {
        MenuIconGraphic icon = Rect(parent, symbol.ToString() + " Icon", position, new Vector2(size, size)).gameObject.AddComponent<MenuIconGraphic>();
        icon.symbol = symbol;
        icon.color = symbol == MenuIcon.Clock ? color : Color.white;
        icon.raycastTarget = false;
    }

    static Text Label(RectTransform rect, Text template, string value, int size, Color color)
    {
        Text text = rect.gameObject.AddComponent<Text>();
        text.font = template.font;
        text.fontSize = size;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = color;
        text.fontStyle = FontStyle.Bold;
        text.text = value;
        text.raycastTarget = false;
        return text;
    }

    static RectTransform FullPanel(Transform parent, string name, Color color)
    {
        RectTransform rect = Rect(parent, name, Vector2.zero, Vector2.zero);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        Image image = rect.gameObject.AddComponent<Image>();
        image.color = color;
        return rect;
    }

    static RectTransform Rect(Transform parent, string name, Vector2 position, Vector2 size)
    {
        GameObject item = new GameObject(name, typeof(RectTransform));
        RectTransform rect = (RectTransform)item.transform;
        rect.SetParent(parent, false);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        return rect;
    }
}
