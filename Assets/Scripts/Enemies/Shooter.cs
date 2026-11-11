using UnityEngine;

public class Shooter : EnemyBrain
{
    enum State { Scan, Aim, Recover }
    [SerializeField, Min(.1f)] float scanInterval = 1.1f;
    [SerializeField, Min(.1f)] float projectileSpeed = 4f;
    [SerializeField, Min(.1f)] float projectileLifetime = 1.5f;
    [SerializeField, Min(.01f)] float projectileScale = .24f;

    State state;
    Vector2 shotDirection;

    protected override void Start()
    {
        base.Start();
        if (picture)
        {
            Face(Position.x - 1f);
            SetState(State.Scan);
            until = Time.time + scanInterval;
        }
    }

    void SetState(State next)
    {
        state = next;
        if (next == State.Aim) Warn();
        if (next == State.Recover) Recover();
    }

    protected override void Tick()
    {
        switch (state)
        {
            case State.Scan:
                if (BothVisible() && CanSeePlayer())
                {
                    shotDirection = (TargetPosition - Position).normalized;
                    SetState(State.Aim);
                }
                else if (Time.time >= until)
                {
                    Face(Position.x - facing);
                    until = Time.time + scanInterval;
                }
                break;
            case State.Aim:
                if (Time.time < until && BothVisible()) break;
                if (BothVisible())
                    Fire();
                SetState(State.Recover);
                break;
            case State.Recover:
                if (Time.time >= until) SetState(State.Scan);
                break;
        }
    }

    void Fire()
    {
        var shot = new GameObject("Shooter projectile");
        shot.transform.position = Position;
        shot.transform.localScale = Vector3.one * projectileScale;
        var visual = shot.AddComponent<SpriteRenderer>();
        visual.sprite = picture.sprite;
        visual.color = Color.yellow;
        visual.sortingOrder = picture.sortingOrder;
        shot.AddComponent<EnemyProjectile>().Launch(
            shotDirection * projectileSpeed, projectileLifetime, damage, walls, target, targetBody, view);
    }
}
