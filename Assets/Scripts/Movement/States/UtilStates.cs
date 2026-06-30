using System;
using UnityEngine;

public class HorizontalMove : State<PlayerInfo>
{
    protected float moveSpeed;
    protected float maxMoveSpeed = Mathf.Infinity;
    protected float slowDown;
    public HorizontalMove(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
    }
    
    protected override void OnExit() { }

    protected override void OnUpdate(float deltaTime) { }

    protected override void OnFixedUpdate(float deltaTime)
    {
        Vector2 vel = _info.Physics.Rigidbody2D.velocity;
        if (vel.x == 0 || Math.Sign(vel.x) == Math.Sign(_info.Input.MoveDirection.x))
        {
            if (Math.Abs(vel.x) > maxMoveSpeed)
            {
                vel.x = maxMoveSpeed * Math.Sign(vel.x);
            }
            else if (Math.Abs(vel.x) < moveSpeed)
            {
                vel.x = moveSpeed * _info.Input.MoveDirection.x;
            }
            else
            {
                // Do Nothing
            }
        }
        else
        {
            if (Math.Abs(vel.x) > maxMoveSpeed)
            {
                vel.x = maxMoveSpeed * Math.Sign(vel.x);
            }
            else if(Math.Abs(vel.x) < moveSpeed)
            {
                vel.x = moveSpeed * _info.Input.MoveDirection.x;
            }
            else
            {
                // REDUCE SPEED
                vel.x -= slowDown * deltaTime * Math.Sign(vel.x);
            }
        }
        _info.Physics.Rigidbody2D.velocity = vel;
    }
    
    
}