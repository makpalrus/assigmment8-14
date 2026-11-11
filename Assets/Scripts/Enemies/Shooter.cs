using UnityEngine;

public class Shooter : EnemyBrain
{
    enum State { Idle, KeepDistance, Aim, Cooldown }
    State state;
    Vector2 shotDirection;
    Vector2 home;
    [SerializeField] LineRenderer laser;
    [SerializeField] EnemyProjectile projectilePrefab;
    float left, right;
    float lostTime;

    protected override void Start()
    {
        base.Start();
        if (!picture) return;
        home = Position;
        left = home.x + stats.waypointOffsets.x;
        right = home.x + stats.waypointOffsets.y;
        Face(home.x - 1f);
        SetState(State.Idle);
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        if (stateLabel) stateLabel.text = next.ToString();
        lostTime = 0f;
        laser.enabled = next == State.Aim;
        if (next == State.Aim)
        {
            Warn();
            GameAudio.Instance.PlayShooterWarning();
        }
        else picture.color = next == State.Cooldown ? Color.gray : normalColor;
    }

    protected override void Tick()
    {
        switch (state)
        {
            case State.Idle:
                if (CanSeePlayer() && BothVisible()) SetState(State.KeepDistance);
                else if (timer >= stats.scanInterval)
                {
                    Face(Position.x - facing);
                    timer = 0f;
                }
                break;
            case State.KeepDistance:
                if (!CanSeePlayer() || !BothVisible())
                {
                    lostTime += Time.fixedDeltaTime;
                    if (lostTime >= stats.loseTime) SetState(State.Idle);
                    break;
                }
                lostTime = 0f;
                Face(TargetPosition.x);
                float distance = Vector2.Distance(Position, TargetPosition);
                if (distance < stats.preferredDistance)
                    StepToward(Mathf.Clamp(Position.x - facing, left, right), stats.moveSpeed);
                else if (distance > stats.preferredDistance)
                    StepToward(Mathf.Clamp(TargetPosition.x, left, right), stats.moveSpeed);
                Face(TargetPosition.x);
                if (timer >= stats.recoveryTime) SetState(State.Aim);
                break;
            case State.Aim:
                Face(TargetPosition.x);
                shotDirection = (TargetPosition - Position).normalized;
                RaycastHit2D wall = Physics2D.Raycast(Position, shotDirection, stats.visionRange, walls);
                laser.SetPosition(0, Position);
                laser.SetPosition(1, wall.collider ? wall.point : Position + shotDirection * stats.visionRange);
                if (timer < stats.windUp) break;
                if (BothVisible() && ClearLine(TargetPosition)) Fire();
                SetState(State.Cooldown);
                break;
            case State.Cooldown:
                if (timer >= stats.cooldown) SetState(State.KeepDistance);
                break;
        }
    }

    void Fire()
    {
        GameAudio.Instance.PlayShoot();
        EnemyProjectile shot = Instantiate(projectilePrefab, Position, Quaternion.identity);
        shot.transform.localScale = Vector3.one * stats.projectileScale;
        shot.Launch(shotDirection * stats.projectileSpeed,
            stats.projectileLifetime, stats.damage, walls, view);
    }

}
