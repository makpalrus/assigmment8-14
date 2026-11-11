using UnityEngine;
using UnityEngine.UI;

public static class PauseAudioControls
{
    static readonly Color TrackColor = new Color32(42, 49, 75, 255);
    static readonly Color FillColor = new Color32(130, 180, 255, 255);

    public static void Create(Button resume, Button restart, Button menu, GameAudio audio)
    {
        if (!audio)
            return;

        RectTransform resumeRect = (RectTransform)resume.transform;
        Transform parent = resumeRect.parent;
        Text template = resume.GetComponentInChildren<Text>(true);
        resumeRect.anchoredPosition = new Vector2(0, 115);
        PlaceButton(restart, -125);
        PlaceButton(menu, 125);

        AddSlider(parent, template, "Music", -5, audio.MusicVolume, audio.SetMusicVolume);
        AddSlider(parent, template, "Sound", -125, audio.SoundVolume, audio.SetSoundVolume);
    }

    static void PlaceButton(Button button, float x)
    {
        RectTransform rect = (RectTransform)button.transform;
        rect.anchoredPosition = new Vector2(x, -255);
        rect.sizeDelta = new Vector2(230, 92);
    }

    static void AddSlider(Transform parent, Text template, string title, float y, float value,
        UnityEngine.Events.UnityAction<float> onChanged)
    {
        RectTransform row = CreateRect(parent, title, new Vector2(0, y), new Vector2(460, 106));
        Text label = CreateRect(row, "Label", new Vector2(0, 31), new Vector2(460, 44)).gameObject.AddComponent<Text>();
        label.font = template.font;
        label.fontSize = 36;
        label.alignment = TextAnchor.MiddleLeft;
        label.color = Color.white;
        label.text = title;
        label.raycastTarget = false;

        RectTransform control = CreateRect(row, "Volume", new Vector2(0, -22), new Vector2(460, 60));
        Image hitArea = control.gameObject.AddComponent<Image>();
        hitArea.color = Color.clear;
        RectTransform track = CreateRect(control, "Track", Vector2.zero, new Vector2(420, 14));
        AddImage(track, TrackColor);
        RectTransform fill = CreateRect(track, "Fill", Vector2.zero, Vector2.zero);
        fill.anchorMin = Vector2.zero;
        fill.anchorMax = Vector2.one;
        AddImage(fill, FillColor);

        RectTransform handleArea = CreateRect(control, "Handle Area", Vector2.zero, new Vector2(420, 60));
        RectTransform handle = CreateRect(handleArea, "Handle", Vector2.zero, new Vector2(38, 48));
        Image thumb = AddImage(handle, Color.white);

        Slider slider = control.gameObject.AddComponent<Slider>();
        slider.fillRect = fill;
        slider.handleRect = handle;
        slider.targetGraphic = thumb;
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
