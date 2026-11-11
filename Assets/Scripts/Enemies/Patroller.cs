using UnityEngine;

public class Patroller : EnemyBrain
{
    enum State { Patrol, Chase, WindUp, Recover }
    [SerializeField] Transform[] waypoints;
    [SerializeField] Vector2 waypointOffsets = new Vector2(-.85f, .85f);
    [SerializeField, Min(.1f)] float patrolSpeed = 1.1f;
    [SerializeField, Min(.1f)] float chaseSpeed = 2f;
    [SerializeField, Min(.1f)] float strikeReach = 1.05f;

    State state;
    float left, right;
    int waypointIndex;

    protected override void Start()
    {
        base.Start();
        left = Position.x + waypointOffsets.x;
        right = Position.x + waypointOffsets.y;
        if (waypoints != null && waypoints.Length == 2 && waypoints[0] && waypoints[1])
        {
            left = waypoints[0].position.x;
            right = waypoints[1].position.x;
        }
        if (picture)
        {
            Face(left);
            waypointIndex = 1;
            SetState(State.Patrol);
        }
    }

    void SetState(State next)
    {
        state = next;
        if (next == State.WindUp) Warn();
        if (next == State.Recover) Recover();
    }

    protected override void Tick()
    {
        switch (state)
        {
            case State.Patrol:
                if (CanSeePlayer() && BothVisible())
                    SetState(State.Chase);
                else if (StepToward(waypointIndex == 0 ? right : left, patrolSpeed))
                    waypointIndex = (waypointIndex + 1) % 2;
                break;
            case State.Chase:
                if (!CanSeePlayer() || !BothVisible())
                    SetState(State.Patrol);
                else if (InReach())
                {
                    SetState(State.WindUp);
                }
                else
                    StepToward(Mathf.Clamp(TargetPosition.x, left, right), chaseSpeed);
                break;
            case State.WindUp:
                if (Time.time < until && BothVisible()) break;
                if (BothVisible() && InReach() && ClearLine(TargetPosition))
                    target.TakeDamage(damage);
                SetState(State.Recover);
                break;
            case State.Recover:
                if (Time.time >= until) SetState(State.Patrol);
                break;
        }
    }

    bool InReach() => Vector2.Distance(Position, TargetPosition) <= strikeReach
        && (TargetPosition.x - Position.x) * facing >= 0;
}
