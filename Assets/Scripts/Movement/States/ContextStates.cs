using UnityEngine;

public class Grounded : State<PlayerInfo>
{
    private float jumpCost = 10f;
    private float slideCost = 10f;
    public Grounded(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
        _info.Physics.Friction = 0.06f;
    }
    
    protected override void OnExit() { }

    protected override State<PlayerInfo> Transition()
    {
        if (!_info.Context.IsGrounded)
        {
            return Machine.GetStateFromType<Airborne>();
        }
        else
        {
            if (_info.Input.JumpPressed && _info.Fire.HasFlame(_info.Cost.JumpCost))
            {
                return Machine.GetStateFromType<Jumping>();
            }
            else if (_info.Input.DashPressed && _info.Fire.HasFlame(_info.Cost.DashCost))
            {
                return Machine.GetStateFromType<Sliding>();
            }
            else
            {
                return null;
            }
        }
    }
    
    protected override State<PlayerInfo> GetInitialState()
    {
        return Machine.GetStateFromType<Idle>();
    }

    protected override void OnUpdate(float deltaTime) { }
    protected override void OnFixedUpdate(float deltaTime) { }
}

public class Airborne : State<PlayerInfo>
{
    public Airborne(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
        _info.Physics.Friction = 0.02f;
    }
    
    protected override void OnExit() { }

    protected override State<PlayerInfo> Transition()
    {
        if(_info.Context.IsGrounded) return Machine.GetStateFromType<Grounded>();
        else return null;
    }
    protected override State<PlayerInfo> GetInitialState() => Machine.GetStateFromType<Falling>();
    protected override void OnUpdate(float deltaTime) { }
    protected override void OnFixedUpdate(float deltaTime) { }
}