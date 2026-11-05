using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(BoxCollider2D))]
public sealed class MovingPlatform : MonoBehaviour
{

    public ItemStats stats;
    Rigidbody2D body;
    Vector2 center;
    int direction = 1;

    public float HorizontalVelocity { get; private set; }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        center = body.position;
    }

    void FixedUpdate()
    {
        Vector2 current = body.position;
        float edge = center.x + direction * stats.platformTravel;
        float next = Mathf.MoveTowards(current.x, edge, stats.platformSpeed * Time.fixedDeltaTime);
        HorizontalVelocity = (next - current.x) / Time.fixedDeltaTime;
        body.MovePosition(new Vector2(next, center.y));
        if (next == edge) direction = -direction;
    }
}
