using UnityEngine;

public sealed class FlyingEnemy : MonoBehaviour
{
    [SerializeField] Sprite artwork;
    public float travel { get { return LevelLoader.ActiveData.itemStats.birdTravel; } }
    public float speed { get { return LevelLoader.ActiveData.itemStats.birdSpeed; } }
    public float bobHeight { get { return LevelLoader.ActiveData.itemStats.birdBobHeight; } }
    public float bobFrequency { get { return LevelLoader.ActiveData.itemStats.birdBobFrequency; } }

    Rigidbody2D body;
    SpriteRenderer picture;
    Vector2 center;
    int direction = 1;

    public void SetArtwork(Sprite sprite)
    {
        artwork = sprite;
        picture.sprite = sprite;
    }

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
