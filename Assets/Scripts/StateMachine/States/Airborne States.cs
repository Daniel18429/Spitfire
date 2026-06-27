public class Falling : State<PlayerInfo>
{
    public Falling(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }
    
    protected override void OnEnter() { }
    
    protected override void OnExit() { }

    protected override State<PlayerInfo> Transition() => null;

    protected override void OnUpdate(float deltaTime) { }
    protected override void OnFixedUpdate(float deltaTime) { }
    
}

public class Jumping : State<PlayerInfo>
{
    private MyTimer _jumpDurationTimer = new MyTimer();
    private float _jumpDuration;
    private float _jumpForce;
    public Jumping(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
        _jumpDurationTimer.Reset(_jumpDuration);
    }

    protected override void OnExit()
    {
        _jumpDurationTimer.Reset(0);
    }

    protected override State<PlayerInfo> Transition()
    {
        if (!_info.Input.JumpPressed || _jumpDurationTimer.Done)
        {
            return Machine.GetStateFromType<Falling>();
        }
        else
        {
            return null;
        }
    }

    protected override void OnUpdate(float deltaTime) { }

    protected override void OnFixedUpdate(float deltaTime)
    {
        
    }
}
