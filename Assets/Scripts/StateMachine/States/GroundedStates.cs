
public class Idle : State<PlayerInfo>
{
    public Idle(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override State<PlayerInfo> Transition()
    {
        if(_info.moving) return Machine.GetStateFromType<Moving>();
        return null;
    }

    protected void OnUpdate()
    {
        
    }
}

public class Moving : State<PlayerInfo>
{
    public Moving(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override State<PlayerInfo> Transition()
    {
        if (!_info.moving) return Machine.GetStateFromType<Idle>();
        return null;
    }
}