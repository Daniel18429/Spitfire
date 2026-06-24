

using UnityEngine.UI;

public class Idle : State<PlayerInfo>
{
    public Idle(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    public override void Transition()
    {
        if (_info.moving) MachineTransition<Moving>();
    }
}

public class Moving : State<PlayerInfo>
{
    public Moving(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    public override void Transition()
    {
        if (!_info.moving) MachineTransition<Idle>();
    }
}