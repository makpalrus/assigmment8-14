using UnityEngine;

public class Patroller : EnemyBrain
{
    enum State { Patrol, Chase, Attack, Recover }
    [SerializeField] Transform[] waypoints;

    State state;
    float left, right;
    int waypointIndex;
    Vector2 lastSeen;
    public void SetWaypoints(Transform[] points) => waypoints = points;

    protected override void Start()
    {
        base.Start();
        left = waypoints[0].position.x;
        right = waypoints[1].position.x;
        Face(left);
        waypointIndex = 1;
        SetState(State.Patrol);
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        if (stateLabel) stateLabel.text = next.ToString();
        if (next == State.Attack)
        {
            Warn();
            GameAudio.Instance.PlayPatrolWarning();
        }
        else if (next == State.Recover) picture.color = Color.gray;
        else Recover();
    }

    protected override void Tick()
    {
        switch (state)
        {
            case State.Patrol:
                if (CanSeePlayer() && BothVisible())
                {
                    lastSeen = TargetPosition;
                    SetState(State.Chase);
                }
                else Patrol();
                break;
            case State.Chase:
                if (CanSeePlayer())
                {
                    timer = 0f;
                    lastSeen = TargetPosition;
                }
                if (BothVisible() && InReach() && CanSeePlayer())
                    SetState(State.Attack);
                else if (timer > stats.loseTime) SetState(State.Patrol);
                else
                    StepToward(Mathf.Clamp(lastSeen.x, left, right), stats.chaseSpeed);
                break;
            case State.Attack:
                if (timer < stats.windUp) break;
                if (BothVisible() && InReach() && ClearLine(TargetPosition))
                {
                    GameAudio.Instance.PlayStrike();
                    target.TakeDamage(stats.damage);
                }
                SetState(State.Recover);
                break;
            case State.Recover:
                if (timer >= stats.cooldown) SetState(State.Chase);
                break;
        }
    }

    bool InReach() => Vector2.Distance(Position, TargetPosition) <= stats.strikeReach
        && (TargetPosition.x - Position.x) * facing >= 0;

    void Patrol()
    {
        Vector2 destination = waypoints[waypointIndex].position;
        Face(destination.x);
        Vector2 next = Vector2.MoveTowards(Position, destination, stats.patrolSpeed * Time.fixedDeltaTime);
        if (ClearLine(next)) transform.position = next;
        if (Vector2.Distance(Position, destination) < .05f)
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
    }

}
