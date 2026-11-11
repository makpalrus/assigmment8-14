using UnityEngine;

public sealed class FlyingEnemy : MonoBehaviour
{
    [SerializeField] Sprite artwork;
    [SerializeField, Min(.1f)] float travel = 1.2f;
    [SerializeField, Min(.1f)] float speed = 1.6f;
    [SerializeField, Min(0f)] float bobHeight = .16f;
    [SerializeField, Min(.1f)] float bobFrequency = 4f;

    Rigidbody2D body;
    SpriteRenderer picture;
    Vector2 center;
    int direction = 1;

    void Awake()
    {
        picture = gameObject.AddComponent<SpriteRenderer>();
        picture.sprite = artwork;
        picture.sortingOrder = 6;
        body = gameObject.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        center = body.position;
        var contact = gameObject.AddComponent<CircleCollider2D>();
        contact.isTrigger = true;
        contact.radius = .32f;
    }

    void FixedUpdate()
    {
        float edge = center.x + direction * travel;
        float x = Mathf.MoveTowards(body.position.x, edge, speed * Time.fixedDeltaTime);
        float y = center.y + Mathf.Sin(Time.time * bobFrequency) * bobHeight;
        body.MovePosition(new Vector2(x, y));
        picture.flipX = direction < 0;
        if (x == edge) direction = -direction;
    }
}
