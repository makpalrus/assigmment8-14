using UnityEngine;
using TMPro;

public abstract class EnemyBrain : MonoBehaviour
{
    [SerializeField] protected Color normalColor = Color.white;
    [SerializeField] Color warningColor = new Color(1f, .2f, .15f);
    [SerializeField] protected LayerMask walls = 1 << 6;

    [SerializeField] protected EnemyStats stats;
    [SerializeField] protected TMP_Text stateLabel;
    [SerializeField] protected PlayerHealth target;
    [SerializeField] protected Collider2D targetBody;
    [SerializeField] protected Camera view;
    [SerializeField] protected SpriteRenderer picture;
    protected int facing = 1;
    protected float timer;
    float senseTimer;
    bool seesPlayer;
    protected Vector2 Position => transform.position;
    protected Vector2 TargetPosition => targetBody.bounds.center;

    public void SetTarget(Player player, Camera camera)
    {
        target = player.GetComponent<PlayerHealth>();
        targetBody = player.GetComponent<Collider2D>();
        view = camera;
    }

    protected virtual void Start()
    {
        if (!stats || !target || !targetBody || !view || !picture)
        {
            Debug.LogError("Assign enemy stats, renderer and level references in the Inspector.", this);
            enabled = false;
            return;
        }
        picture.color = normalColor;
        if (stateLabel) stateLabel.gameObject.SetActive(Application.isEditor || Debug.isDebugBuild);
    }

    void FixedUpdate()
    {
        if (target && targetBody && view && GameUI.Instance && GameUI.Instance.IsPlaying)
        {
            timer += Time.fixedDeltaTime;
            senseTimer -= Time.fixedDeltaTime;
            if (senseTimer <= 0f)
            {
                senseTimer = stats.senseInterval;
                seesPlayer = CheckSight();
            }
            Tick();
        }
    }

    protected abstract void Tick();

    protected bool CanSeePlayer() => seesPlayer;

    bool CheckSight()
    {
        Vector2 toPlayer = TargetPosition - Position;

        if (toPlayer.magnitude > stats.visionRange)
            return false;

        Vector2 forward = Vector2.right * facing;
        if (Vector2.Angle(forward, toPlayer) > stats.visionAngle * .5f)
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
    }

    protected void Recover()
    {
        picture.color = normalColor;
    }

    void LateUpdate()
    {
        if (!Application.isEditor && !Debug.isDebugBuild) return;
        if (stateLabel && view)
        {
            stateLabel.transform.rotation = view.transform.rotation;
        }
        if (targetBody) Debug.DrawRay(Position, TargetPosition - Position, seesPlayer ? Color.green : Color.red);
    }

    void OnDrawGizmosSelected()
    {
        if (!stats) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stats.visionRange);
        Vector3 forward = Vector3.right * facing;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -stats.visionAngle / 2f) * forward * stats.visionRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, stats.visionAngle / 2f) * forward * stats.visionRange);
    }
}
