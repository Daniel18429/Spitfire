using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState _CurrentState;

    public void Initialize(PlayerState playerState)
    {
        _CurrentState = playerState;
        _CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        _CurrentState.Exit();
        _CurrentState = newState;
        _CurrentState.Enter();
    }
    
}
