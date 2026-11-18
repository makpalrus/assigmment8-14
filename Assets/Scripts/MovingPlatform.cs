using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(BoxCollider2D))]
public sealed class MovingPlatform : MonoBehaviour
{
    public float travel { get { return LevelLoader.ActiveData.itemStats.platformTravel; } }
    public float speed { get { return LevelLoader.ActiveData.itemStats.platformSpeed; } }

    Rigidbody2D body;
    Vector2 center;
    int direction = 1;

    public float HorizontalVelocity { get; private set; }

    void Awake()
    {
        body = gameObject.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        center = body.position;
    }

    void FixedUpdate()
    {
        Vector2 current = body.position;
        float edge = center.x + direction * travel;
        float next = Mathf.MoveTowards(current.x, edge, speed * Time.fixedDeltaTime);
        HorizontalVelocity = (next - current.x) / Time.fixedDeltaTime;
        body.MovePosition(new Vector2(next, center.y));
        if (next == edge) direction = -direction;
    }
}
