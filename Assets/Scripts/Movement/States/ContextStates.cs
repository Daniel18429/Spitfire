public class Grounded : State<PlayerInfo>
{
    public Grounded(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
        
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
            if (_info.Input.JumpPressed)
            {
                return Machine.GetStateFromType<Jumping>();
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
    
    protected override void OnEnter() { }
    
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