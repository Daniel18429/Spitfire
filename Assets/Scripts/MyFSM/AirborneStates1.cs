using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : State<PlayerInfo>
{
    public Falling(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine,
        info, parent)
    {
    }

    public override void Transition()
    {
        if (_info.jumping) MachineTransition<Jumping>();
    }

}

public class Jumping : State<PlayerInfo>
{
    public Jumping(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    public override void Transition()
    {
        if (!_info.jumping) MachineTransition<Falling>();
    }
}