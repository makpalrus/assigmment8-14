using UnityEngine;
using UnityEngine.UI;

public static class ForestUI
{
    public static readonly Color Ink = new Color32(68, 65, 28, 255);
    public static readonly Color Green = new Color32(100, 181, 34, 255);

    public static void StyleButton(Button button)
    {
        Image image = button.GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("UI/ForestButton");
        image.type = Image.Type.Sliced;
        image.pixelsPerUnitMultiplier = 4f;
        image.color = Color.white;
        button.targetGraphic = image;

        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(1f, 1f, .9f, 1f);
        colors.selectedColor = Color.white;
        colors.pressedColor = new Color(.8f, .88f, .7f, 1f);
        colors.disabledColor = new Color(.6f, .62f, .55f, .85f);
        button.colors = colors;
        foreach (Text label in button.GetComponentsInChildren<Text>(true))
        {
            StyleText(label, Color.white);
            label.text = label.text.ToUpperInvariant();
        }
    }

    public static void StyleText(Text text, Color color)
    {
        text.color = color;
        text.fontStyle = FontStyle.Bold;
    }

    public static void AddIcon(Transform parent, MenuIcon symbol, Vector2 position, float size, Color color)
    {
        GameObject item = new GameObject(symbol + " Icon", typeof(RectTransform));
        RectTransform rect = (RectTransform)item.transform;
        rect.SetParent(parent, false);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(size, size);
        MenuIconGraphic graphic = item.AddComponent<MenuIconGraphic>();
        graphic.symbol = symbol;
        graphic.color = color;
        graphic.raycastTarget = false;
    }

    public static RectTransform Panel(Transform parent, Vector2 size, Vector2 position)
    {
        GameObject item = new GameObject("Parchment Panel", typeof(RectTransform), typeof(Image));
        RectTransform rect = (RectTransform)item.transform;
        rect.SetParent(parent, false);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        rect.SetAsFirstSibling();
        Image image = item.GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("UI/ForestPanel");
        image.type = Image.Type.Sliced;
        image.pixelsPerUnitMultiplier = 2f;
        image.raycastTarget = false;
        return rect;
    }

    public static void StyleScreen(GameObject screen)
    {
        Image shade = screen.GetComponent<Image>();
        if (shade)
            shade.color = new Color(.05f, .12f, .04f, .7f);
    }

    public static void Header(Text title, Vector2 position)
    {
        RectTransform rect = title.rectTransform;
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(540, 100);
        StyleText(title, Ink);
        title.fontSize = 48;
    }
}
