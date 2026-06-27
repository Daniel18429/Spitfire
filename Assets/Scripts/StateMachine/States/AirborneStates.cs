using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : State<PlayerInfo>
{
    public Falling(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine,
        info, parent)
    {
    }

    protected override State<PlayerInfo> Transition()
    {
        if (_info.jumping) return Machine.GetStateFromType<Jumping>();
        return null;
    }

}

public class Jumping : State<PlayerInfo>
{
    public Jumping(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override State<PlayerInfo> Transition()
    {
        if (!_info.jumping) return Machine.GetStateFromType<Falling>();
        return null;
    }
}