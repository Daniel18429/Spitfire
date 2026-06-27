using System;
using UnityEngine;

public class PlayerInfo
{
    public PlayerInput Input;
    public PlayerContext Context;
    public PlayerPhysics Physics;
    public PlayerTimers Timers;
    public PlayerInfo(GameObject gameObject)
    {
        Physics = new PlayerPhysics(gameObject.GetComponent<Rigidbody2D>());
        Input = new PlayerInput();
        Context = new PlayerContext();
    }
}

public class PlayerInput
{
    public Vector2 MoveDirection;
    public bool JumpPressed;
    public bool DashPressed;

    public void CacheInput(Vector2 moveDirection, bool jumpPressed, bool dashPressed)
    {
        MoveDirection = moveDirection;
        if(!jumpPressed) JumpPressed = jumpPressed;
        if (!dashPressed) DashPressed = dashPressed;
    }

    public void Reset()
    {
        MoveDirection = Vector2.zero;
        JumpPressed = false;
        DashPressed = false;
    }
}

public class PlayerTimers
{
    
}

public class PlayerContext
{
    
}

public class PlayerDebug
{
    public bool Moving;
    public bool Jumping;
    public bool Dashing;
}

public class PlayerPhysics
{
    public PlayerPhysics(Rigidbody2D rb2d)
    {
        Rigidbody2D = rb2d;
    }
    public Rigidbody2D Rigidbody2D;
    public Vector2 Acceleration;
    public float Gravity;
    public float SpeedCap;
    public float YMax;
    public float XFriction;
    public float YFriction;

    public void PhysicsUpdate(float deltaTime)
    {
        Rigidbody2D.velocity += Acceleration * deltaTime;
        Rigidbody2D.velocity -= new Vector2(0, Gravity) * deltaTime;
        if (Rigidbody2D.velocity.magnitude > SpeedCap)
        {
            Rigidbody2D.velocity = Vector2.ClampMagnitude(Rigidbody2D.velocity, SpeedCap);
        }
        
    }
}