using UnityEngine;

public class Idle : State<PlayerInfo>
{
    public Idle(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }
    
    protected override void OnEnter() { }
    
    protected override void OnExit() { }

    protected override State<PlayerInfo> Transition()
    {
        if(_info.Input.MoveDirection != Vector2.zero) return Machine.GetStateFromType<Walking>();
        else
        {
            return null;
        }
    }

    protected override void OnUpdate(float deltaTime) { }
    protected override void OnFixedUpdate(float deltaTime) { }
}

public class Walking : State<PlayerInfo>
{
    private float _walkSpeed = 5.0f;
    public Walking(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
    }
    
    protected override void OnExit() { }

    protected override State<PlayerInfo> Transition()
    {
        if(_info.Physics.Rigidbody2D.velocity == Vector2.zero && _info.Input.MoveDirection == Vector2.zero) return Machine.GetStateFromType<Idle>();
        else
        {
            return null;
        }
    }

    protected override void OnUpdate(float deltaTime) { }

    protected override void OnFixedUpdate(float deltaTime)
    {
        _info.Physics.Rigidbody2D.velocity = _info.Input.MoveDirection * _walkSpeed;
    }
}