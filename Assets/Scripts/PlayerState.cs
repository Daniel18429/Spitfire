using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController _playerController;
    protected PlayerStateMachine _playerStateMachine;
    private float _startTime;
    public float AliveTime => Time.time - _startTime;

    protected Vector2 _cachedDir;
    protected bool _cachedJump;
    protected bool _cachedDash;
    

    public PlayerState(PlayerController playerController, PlayerStateMachine playerStateMachine)
    {
        _playerController = playerController;
        _playerStateMachine = playerStateMachine;
        _startTime = Time.time;
    }

    public virtual void HandleInput(Vector2 dir, bool jump, bool dash)
    {
        _cachedDir = dir;
        if(!_cachedJump) _cachedJump = jump;
        if(!_cachedDash) _cachedDash = dash;
    }
    
    public virtual void Enter(){}
    public virtual void Exit(){}
    public virtual void FrameUpdate(){}
    public virtual Vector2 PhysicsUpdate()
    {
        return Vector2.zero;
    }
}
