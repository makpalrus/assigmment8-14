using UnityEngine;

public class Charger : EnemyBrain
{
    enum State { Guard, WindUp, Dash, Recover }
    Vector2 waypointOffsets { get { return stats.waypointOffsets; } }
    float dashSpeed { get { return stats.dashSpeed; } }
    float dashDistance { get { return stats.dashDistance; } }
    float laneTolerance { get { return stats.laneTolerance; } }
    float hitReach { get { return stats.hitReach; } }

    State state;
    float left, right, destination;
    bool hit;

    protected override void Start()
    {
        base.Start();
        left = Position.x + waypointOffsets.x;
        right = Position.x + waypointOffsets.y;
        if (picture)
            SetState(State.Guard);
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
            case State.Guard:
                Face(TargetPosition.x);
                if (BothVisible() && CanSeePlayer() && Mathf.Abs(TargetPosition.y - Position.y) <= laneTolerance)
                {
                    destination = Mathf.Clamp(Position.x + facing * dashDistance, left, right);
                    if (Mathf.Abs(destination - Position.x) < hitReach) break;
                    hit = false;
                    SetState(State.WindUp);
                }
                break;
            case State.WindUp:
                if (BothVisible() && Time.time < until) break;
                if (BothVisible()) SetState(State.Dash);
                else SetState(State.Recover);
                break;
            case State.Dash:
                if (!BothVisible())
                {
                    SetState(State.Recover);
                    break;
                }
                bool reached = StepToward(destination, dashSpeed);
                if (!hit && Vector2.Distance(Position, TargetPosition) <= hitReach && ClearLine(TargetPosition))
                {
                    hit = true;
                    target.TakeDamage(damage);
                }
                if (reached) SetState(State.Recover);
                break;
            case State.Recover:
                if (Time.time >= until) SetState(State.Guard);
                break;
        }
    }

}
