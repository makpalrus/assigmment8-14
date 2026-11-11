using UnityEngine;

public sealed class FlyingEnemy : MonoBehaviour
{
    public ItemStats stats;

    Rigidbody2D body;
    SpriteRenderer picture;
    Vector2 center;
    int direction = 1;

    void Awake()
    {
        picture = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        center = body.position;
    }

    void FixedUpdate()
    {
        float edge = center.x + direction * stats.birdTravel;
        float x = Mathf.MoveTowards(body.position.x, edge, stats.birdSpeed * Time.fixedDeltaTime);
        float y = center.y + Mathf.Sin(Time.time * stats.birdBobFrequency) * stats.birdBobHeight;
        body.MovePosition(new Vector2(x, y));
        picture.flipX = direction < 0;
        if (x == edge) direction = -direction;
    }
}
