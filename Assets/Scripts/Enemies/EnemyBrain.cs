using UnityEngine;

public abstract class EnemyBrain : MonoBehaviour
{
    [SerializeField] Sprite artwork;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color warningColor = new Color(1f, .2f, .15f);
    [SerializeField] protected LayerMask walls = 1 << 6;
    [SerializeField, Min(0.1f)] protected float visionRange = 4f;
    [SerializeField, Range(1f, 360f)] float visionAngle = 150f;
    [SerializeField, Range(.3f, .8f)] protected float windUp = .6f;
    [SerializeField, Min(.1f)] protected float cooldown = 1.4f;
    [SerializeField, Min(1)] protected int damage = 1;

    protected PlayerHealth target;
    protected Collider2D targetBody;
    protected Camera view;
    protected SpriteRenderer picture;
    protected int facing = 1;
    protected float until;
    protected Vector2 Position => transform.position;
    protected Vector2 TargetPosition => targetBody.bounds.center;

    protected virtual void Start()
    {
        var ui = MobileGameUI.Instance;
        if (!ui || !ui.player)
        {
            Debug.LogError("Enemy requires a player assigned in MobileGameUI.", this);
            enabled = false;
            return;
        }
        var player = ui.player;
        target = player.GetComponent<PlayerHealth>();
        targetBody = player.GetComponent<Collider2D>();
        view = Camera.main;
        picture = gameObject.AddComponent<SpriteRenderer>();
        picture.sprite = artwork;
        picture.color = normalColor;
        picture.sortingOrder = 6;
    }

    void FixedUpdate()
    {
        if (target && targetBody && view && MobileGameUI.Instance && MobileGameUI.Instance.IsPlaying)
            Tick();
    }

    protected abstract void Tick();

    protected bool CanSeePlayer()
    {
        Vector2 toPlayer = TargetPosition - Position;

        if (toPlayer.sqrMagnitude > visionRange * visionRange)
            return false;

        Vector2 forward = Vector2.right * facing;
        if (Vector2.Angle(forward, toPlayer) > visionAngle * .5f)
            return false;

        return ClearLine(TargetPosition);
    }

    protected bool ClearLine(Vector2 destination)
    {
        Vector2 offset = destination - Position;
        RaycastHit2D hit = Physics2D.Raycast(Position, offset.normalized, offset.magnitude, walls);
        return hit.collider == null;
    }

    protected bool BothVisible() => Visible(view, Position) && Visible(view, TargetPosition);

    public static bool Visible(Camera camera, Vector2 point)
    {
        Vector3 p = camera.WorldToViewportPoint(point);
        return p.z > 0 && p.x > 0 && p.x < 1 && p.y > 0 && p.y < 1;
    }

    protected void Face(float x)
    {
        if (!Mathf.Approximately(x, Position.x))
            facing = x > Position.x ? 1 : -1;
        picture.flipX = facing < 0;
    }

    protected bool StepToward(float x, float speed)
    {
        Face(x);
        Vector2 next = new Vector2(Mathf.MoveTowards(Position.x, x, speed * Time.fixedDeltaTime), Position.y);
        if (!ClearLine(next))
            return true;
        transform.position = next;
        return Mathf.Approximately(next.x, x);
    }

    protected void Warn()
    {
        picture.color = warningColor;
        until = Time.time + windUp;
    }

    protected void Recover()
    {
        picture.color = normalColor;
        until = Time.time + cooldown;
    }
}
