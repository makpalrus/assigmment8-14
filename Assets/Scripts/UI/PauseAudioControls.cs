using UnityEngine;
using UnityEngine.UI;

public static class PauseAudioControls
{
    static readonly Color TrackColor = new Color32(185, 152, 86, 255);
    static readonly Color FillColor = new Color32(107, 184, 38, 255);

    public static void Create(Button resume, Button restart, Button menu, GameAudio audio)
    {
        if (!audio)
            return;

        RectTransform resumeRect = (RectTransform)resume.transform;
        Transform parent = resumeRect.parent;
        Text template = resume.GetComponentInChildren<Text>(true);
        ForestUI.StyleScreen(parent.parent.gameObject);
        ForestUI.Panel(parent, new Vector2(800, 840), new Vector2(0, -10));
        foreach (Text title in parent.GetComponentsInChildren<Text>(true))
        {
            if (title.name == "Title")
                ForestUI.Header(title, new Vector2(0, 305));
        }
        resumeRect.anchoredPosition = new Vector2(0, 145);
        resumeRect.sizeDelta = new Vector2(500, 125);
        ForestUI.StyleButton(resume);
        ForestUI.AddIcon(resume.transform, MenuIcon.Play, new Vector2(-160, 0), 44, Color.white);
        PlaceButton(restart, -145);
        PlaceButton(menu, 145);

        AddSlider(parent, template, "MUSIC", -5, audio.MusicVolume, audio.SetMusicVolume);
        AddSlider(parent, template, "SOUND", -140, audio.SoundVolume, audio.SetSoundVolume);
    }

    static void PlaceButton(Button button, float x)
    {
        RectTransform rect = (RectTransform)button.transform;
        rect.anchoredPosition = new Vector2(x, -285);
        rect.sizeDelta = new Vector2(270, 110);
        ForestUI.StyleButton(button);
        button.GetComponentInChildren<Text>().fontSize = 32;
        button.GetComponentInChildren<Text>().rectTransform.anchoredPosition = Vector2.zero;
    }

    static void AddSlider(Transform parent, Text template, string title, float y, float value,
        UnityEngine.Events.UnityAction<float> onChanged)
    {
        RectTransform row = CreateRect(parent, title, new Vector2(0, y), new Vector2(460, 106));
        Text label = CreateRect(row, "Label", new Vector2(0, 31), new Vector2(460, 44)).gameObject.AddComponent<Text>();
        label.font = template.font;
        label.fontSize = 36;
        label.alignment = TextAnchor.MiddleLeft;
        ForestUI.StyleText(label, ForestUI.Ink);
        label.text = title;
        label.raycastTarget = false;
        label.rectTransform.anchoredPosition = new Vector2(35, 31);
        label.rectTransform.sizeDelta = new Vector2(380, 44);
        ForestUI.AddIcon(row, title == "MUSIC" ? MenuIcon.Music : MenuIcon.Sound,
            new Vector2(-207, 31), 40, ForestUI.Ink);

        RectTransform control = CreateRect(row, "Volume", new Vector2(0, -22), new Vector2(460, 60));
        Image hitArea = control.gameObject.AddComponent<Image>();
        hitArea.color = Color.clear;
        RectTransform track = CreateRect(control, "Track", Vector2.zero, new Vector2(420, 14));
        AddImage(track, TrackColor);
        RectTransform fill = CreateRect(track, "Fill", Vector2.zero, Vector2.zero);
        fill.anchorMin = Vector2.zero;
        fill.anchorMax = Vector2.one;
        AddImage(fill, FillColor);

        RectTransform handleArea = CreateRect(control, "Handle Area", Vector2.zero, new Vector2(420, 30));
        RectTransform handle = CreateRect(handleArea, "Handle", Vector2.zero, new Vector2(30, 0));
        MenuIconGraphic knob = handle.gameObject.AddComponent<MenuIconGraphic>();
        knob.symbol = MenuIcon.Knob;
        knob.color = ForestUI.Green;
        knob.raycastTarget = false;

        Slider slider = control.gameObject.AddComponent<Slider>();
        slider.fillRect = fill;
        slider.handleRect = handle;
        slider.targetGraphic = knob;
        slider.SetValueWithoutNotify(value);
        slider.onValueChanged.AddListener(onChanged);
    }

    static Image AddImage(RectTransform rect, Color color)
    {
        Image image = rect.gameObject.AddComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        return image;
    }

    static RectTransform CreateRect(Transform parent, string name, Vector2 position, Vector2 size)
    {
        GameObject item = new GameObject(name, typeof(RectTransform));
        RectTransform rect = (RectTransform)item.transform;
        rect.SetParent(parent, false);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        return rect;
    }
}
