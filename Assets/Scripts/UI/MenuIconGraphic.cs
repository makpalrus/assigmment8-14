using UnityEngine;
using UnityEngine.UI;

public enum MenuIcon { Play, Clock, Lock, Back, Reset, Music, Sound, Pause, Home, Knob }

[RequireComponent(typeof(CanvasRenderer))]
public class MenuIconGraphic : MaskableGraphic
{
    public MenuIcon symbol;

    protected override void OnPopulateMesh(VertexHelper mesh)
    {
        mesh.Clear();
        if (symbol == MenuIcon.Knob)
        {
            Circle(mesh, Vector2.zero, .48f);
        }
        else if (symbol == MenuIcon.Play)
        {
            Triangle(mesh, new Vector2(-.25f, -.4f), new Vector2(-.25f, .4f), new Vector2(.4f, 0));
        }
        else if (symbol == MenuIcon.Clock)
        {
            Ring(mesh, .4f, .075f, 0, 360);
            Line(mesh, Vector2.zero, new Vector2(0, .24f), .075f);
            Line(mesh, Vector2.zero, new Vector2(.2f, -.1f), .075f);
        }
        else if (symbol == MenuIcon.Lock)
        {
            Ring(mesh, .26f, .09f, 0, 180);
            Rectangle(mesh, new Vector2(-.34f, -.4f), new Vector2(.34f, .07f));
        }
        else if (symbol == MenuIcon.Back)
        {
            Line(mesh, new Vector2(.36f, 0), new Vector2(-.3f, 0), .09f);
            Line(mesh, new Vector2(-.3f, 0), new Vector2(0, .3f), .09f);
            Line(mesh, new Vector2(-.3f, 0), new Vector2(0, -.3f), .09f);
        }
        else if (symbol == MenuIcon.Music)
        {
            Line(mesh, new Vector2(.12f, -.23f), new Vector2(.12f, .35f), .09f);
            Line(mesh, new Vector2(.12f, .35f), new Vector2(.38f, .24f), .1f);
            Circle(mesh, new Vector2(-.01f, -.23f), .16f);
        }
        else if (symbol == MenuIcon.Sound)
        {
            Rectangle(mesh, new Vector2(-.35f, -.13f), new Vector2(-.14f, .13f));
            Triangle(mesh, new Vector2(-.2f, 0), new Vector2(.09f, .32f), new Vector2(.09f, -.32f));
            Ring(mesh, .38f, .07f, -55, 55);
        }
        else if (symbol == MenuIcon.Pause)
        {
            Rectangle(mesh, new Vector2(-.29f, -.36f), new Vector2(-.09f, .36f));
            Rectangle(mesh, new Vector2(.09f, -.36f), new Vector2(.29f, .36f));
        }
        else if (symbol == MenuIcon.Home)
        {
            Triangle(mesh, new Vector2(-.4f, .02f), new Vector2(0, .4f), new Vector2(.4f, .02f));
            Rectangle(mesh, new Vector2(-.28f, -.35f), new Vector2(.28f, .04f));
        }
        else
        {
            Ring(mesh, .34f, .075f, -50, 230);
            Triangle(mesh, new Vector2(-.4f, -.03f), new Vector2(-.12f, -.12f), new Vector2(-.37f, -.32f));
        }
    }

    void Circle(VertexHelper mesh, Vector2 center, float radius)
    {
        for (int i = 0; i < 24; i++)
        {
            float a = i * Mathf.PI * 2f / 24;
            float b = (i + 1) * Mathf.PI * 2f / 24;
            Triangle(mesh, center, center + new Vector2(Mathf.Cos(b), Mathf.Sin(b)) * radius,
                center + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * radius);
        }
    }

    void Ring(VertexHelper mesh, float radius, float width, float start, float end)
    {
        int segments = 32;
        for (int i = 0; i < segments; i++)
        {
            float a = Mathf.Lerp(start, end, (float)i / segments) * Mathf.Deg2Rad;
            float b = Mathf.Lerp(start, end, (float)(i + 1) / segments) * Mathf.Deg2Rad;
            Vector2 first = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * radius;
            Vector2 second = new Vector2(Mathf.Cos(b), Mathf.Sin(b)) * radius;
            Line(mesh, first, second, width);
        }
    }

    void Rectangle(VertexHelper mesh, Vector2 min, Vector2 max)
    {
        Triangle(mesh, min, new Vector2(min.x, max.y), max);
        Triangle(mesh, min, max, new Vector2(max.x, min.y));
    }

    void Line(VertexHelper mesh, Vector2 from, Vector2 to, float width)
    {
        Vector2 direction = (to - from).normalized;
        Vector2 side = new Vector2(-direction.y, direction.x) * width * .5f;
        Triangle(mesh, from - side, from + side, to + side);
        Triangle(mesh, from - side, to + side, to - side);
    }

    void Triangle(VertexHelper mesh, Vector2 a, Vector2 b, Vector2 c)
    {
        int index = mesh.currentVertCount;
        Rect rect = rectTransform.rect;
        Vector2 size = rect.size;
        mesh.AddVert(rect.center + Vector2.Scale(a, size), color, Vector2.zero);
        mesh.AddVert(rect.center + Vector2.Scale(b, size), color, Vector2.zero);
        mesh.AddVert(rect.center + Vector2.Scale(c, size), color, Vector2.zero);
        mesh.AddTriangle(index, index + 1, index + 2);
    }
}
