using UnityEngine;
using System;

public class WallSliding : HorizontalMove
{
    private float _slideSpeed = 2f;
    public WallSliding(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
        moveSpeed = 1;
    }

    protected override void OnEnter()
    {
        _slideSpeed = _info.Val.MaxWallSpeed;
        _info.Physics.Gravity = _info.Val.WallSlidingGravity;
        _info.Timers.CayoteTime.End();
    }
    
    protected override void OnExit() { }

    protected override State<PlayerInfo> Transition()
    {
        return null;
    }
    
    protected override State<PlayerInfo> GetInitialState() => null;

    protected override void OnUpdate(float deltaTime) { }

    protected override void OnFixedUpdate(float deltaTime)
    {
        base.OnFixedUpdate(deltaTime);
        if (_info.Physics.Rigidbody2D.velocity.y < -_slideSpeed && _info.Input.MoveDirection.x != 0)
        {
            _info.Physics.Rigidbody2D.velocity = new Vector2(_info.Physics.Rigidbody2D.velocity.x, -_slideSpeed);
        }
    }
}