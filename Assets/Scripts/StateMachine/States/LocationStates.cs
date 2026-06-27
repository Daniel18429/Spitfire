using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airborne : State<PlayerInfo>
{
    // Start is called before the first frame update
    public Airborne(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent = null) : base(machine, info, parent)
    {
    }

    protected override State<PlayerInfo> GetInitialState()
    {
        return Machine.GetStateFromType<Jumping>();
    }

    protected override State<PlayerInfo> Transition()
    {
        if (_info.ground) return Machine.GetStateFromType<Grounded>();
        return null;
    }
}

public class Grounded : State<PlayerInfo>
{
    public Grounded(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent = null) : base(machine, info, parent)
    {
    }

    protected override State<PlayerInfo> Transition()
    {
        if(!_info.ground) return Machine.GetStateFromType<Airborne>();
        else
        {
            return null;
        }
    }
    
    protected override State<PlayerInfo> GetInitialState()
    {
        Debug.Log("called initial state");
        return Machine.GetStateFromType<Idle>();
    }
}