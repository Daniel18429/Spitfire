using System;
using UnityEngine;

public class Dash : State<PlayerInfo>
{
    private Vector2 _dashDir;
    private float _dashSpeed;
    private float _dashTime;
    private MyTimer _dashTimer = new MyTimer();
    
    public Dash(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
        _dashSpeed = _info.Val.DashDistance / _info.Val.DashTime;
        _dashTime =  _info.Val.DashTime;
    }

    protected override void OnEnter()
    {
        _dashDir = _info.Input.mouseDir.normalized;
        _dashTimer.Reset(_dashTime);
    }

    protected override void OnExit()
    {
        _info.Timers.DashCooldown.Reset(2.0f);
        _info.Physics.Rigidbody2D.velocity = _info.Physics.Rigidbody2D.velocity.normalized * (_dashSpeed * 0.3f);
    }

    protected override State<PlayerInfo> Transition()
    {
        if (_dashTimer.Done)
        {
            return Machine.GetStateFromType<Airborne>();
        }
        else
        {
            return null;
        }
    }
    
    protected override State<PlayerInfo> GetInitialState() => null;

    protected override void OnUpdate(float deltaTime) { }

    protected override void OnFixedUpdate(float deltaTime)
    {
        _info.Physics.Rigidbody2D.velocity = _dashDir * _dashSpeed;
        _dashTimer.Tick(deltaTime);
    }
}