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
        Vector2 velocity = _info.Physics.Rigidbody2D.velocity;
        velocity.x = _info.Input.MoveDirection.x * _walkSpeed;
        _info.Physics.Rigidbody2D.velocity = velocity;
    }
}

// IMPLEMENT
public class Sliding : State<PlayerInfo>
{
    private Vector2 tangent;
    public Sliding(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
        _info.Fire.Consume(_info.Cost.DashCost);
        if (_info.Context.GroundNormal.x == 0)
        {
            tangent = Vector2.zero;
        }
        else if (_info.Context.GroundNormal.x > 0)
        {
            // Rotated 90 degrees clockwise
            tangent = new Vector2(_info.Context.GroundNormal.y, -_info.Context.GroundNormal.x); 
        }
        else if (_info.Context.GroundNormal.x < 0)
        {
            // Rotated 90 degrees counter-clockwise
            tangent = new Vector2(-_info.Context.GroundNormal.y, _info.Context.GroundNormal.x); 
        }
    }
    
    protected override void OnExit() { }

    protected override State<PlayerInfo> Transition()
    {
        if (!_info.Input.DashPressed)
        {
            return Machine.GetStateFromType<Walking>();
        }
        else
        {
            return null;
        }
    }
    protected override State<PlayerInfo> GetInitialState() => null;

    protected override void OnUpdate(float deltaTime)
    {
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
        Vector2 velocity = _info.Physics.Rigidbody2D.velocity;
        velocity += tangent * Mathf.Abs(tangent.y);
        _info.Physics.Rigidbody2D.velocity = velocity;
    }
}