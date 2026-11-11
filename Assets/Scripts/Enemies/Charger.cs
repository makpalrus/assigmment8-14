using UnityEngine;

public class Charger : EnemyBrain
{
    enum State { Idle, WindUp, Dash, Stunned }

    State state;
    float left, right, destination;
    bool hit;

    protected override void Start()
    {
        base.Start();
        left = Position.x + stats.waypointOffsets.x;
        right = Position.x + stats.waypointOffsets.y;
        if (picture)
            SetState(State.Idle);
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        if (stateLabel) stateLabel.text = next.ToString();
        if (next == State.Dash) GameAudio.Instance.PlayDash();
        if (next == State.WindUp)
        {
            Warn();
            GameAudio.Instance.PlayChargerWarning();
        }
        if (next == State.Stunned) picture.color = Color.gray;
        if (next == State.Idle) Recover();
    }

    protected override void Tick()
    {
        switch (state)
        {
            case State.Idle:
                Face(TargetPosition.x);
                if (BothVisible() && CanSeePlayer() && Mathf.Abs(TargetPosition.y - Position.y) <= stats.laneTolerance)
                {
                    destination = Mathf.Clamp(Position.x + facing * stats.dashDistance, left, right);
                    if (Mathf.Abs(destination - Position.x) < stats.hitReach) break;
                    hit = false;
                    SetState(State.WindUp);
                }
                break;
            case State.WindUp:
                Face(TargetPosition.x);
                if (timer < stats.windUp) break;
                if (BothVisible())
                {
                    destination = Mathf.Clamp(Position.x + facing * stats.dashDistance, left, right);
                    SetState(State.Dash);
                }
                else SetState(State.Stunned);
                break;
            case State.Dash:
                if (!BothVisible())
                {
                    SetState(State.Stunned);
                    break;
                }
                bool reached = StepToward(destination, stats.dashSpeed);
                if (!hit && Vector2.Distance(Position, TargetPosition) <= stats.hitReach && ClearLine(TargetPosition))
                {
                    hit = true;
                    target.TakeDamage(stats.damage);
                }
                if (reached) SetState(State.Stunned);
                break;
            case State.Stunned:
                if (timer >= stats.cooldown) SetState(State.Idle);
                break;
        }
    }

}
