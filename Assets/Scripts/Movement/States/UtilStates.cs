using System;
using Unity.VisualScripting;
using UnityEngine;

public class HorizontalMove : State<PlayerInfo>
{
    protected float moveSpeed;
    protected float maxMoveSpeed = Mathf.Infinity;
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
        if (moveSpeed == 0)
        {
            throw new Exception("MoveSpeed is Zero you Twat!");
        }
        Vector2 vel = _info.Physics.Rigidbody2D.velocity;
        // Not moving or Input aligned with Move Dir
        if (vel.x == 0 || _info.Input.MoveDirection.x == 0 ||  Math.Sign(vel.x) == Math.Sign(_info.Input.MoveDirection.x))
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
                vel.x = moveSpeed * _info.Input.MoveDirection.x;
            }
        }
        int wallSide = 0;
        if (_info.Context.LeftWall) wallSide = -1;
        else if(_info.Context.RightWall) wallSide = 1;
        if (Mathf.Sign(_info.Input.MoveDirection.x) == wallSide)
        {
            vel.x = 0;
        }
        _info.Physics.Rigidbody2D.velocity = vel;
    }
}