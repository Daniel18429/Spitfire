using System;
using UnityEngine;

[System.Serializable]
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
        Timers = new PlayerTimers();
    }

    public void Init()
    {
        Context.Init();
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
        if(!JumpPressed) JumpPressed = jumpPressed;
        if (!DashPressed) DashPressed = dashPressed;
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
    private MyTimer CayoteTime;

    public void Tick(float deltaTime)
    {
        CayoteTime.Tick(deltaTime);
    }
}

[Serializable]
public class PlayerContext
{
    public bool IsGrounded;
    public bool OnWall;
    public bool LeftWall;
    public bool RightWall;
    public RaycastHit2D GroundHit;
    public RaycastHit2D WallHit;
    public Vector2 GroundNormal;
    public Vector2 WallNormal;

    public LayerMask CollisionMask;

    public PlayerContext()
    {
    }

    public void Init()
    {
        CollisionMask = LayerMask.GetMask("Ground","Wall");
    }

    public void UpdateContext(GameObject gameObject)
    {
        IsGrounded = OnWall = LeftWall = RightWall = false;
        GroundNormal = WallNormal = Vector2.zero;
        float radius = gameObject.GetComponent<CircleCollider2D>().radius;
        float radiusMargin = 0.99f;
        float distance = (1 - radiusMargin) * 2;
        GroundHit = Physics2D.CircleCast(gameObject.transform.position, radius * radiusMargin, Vector2.down, distance, CollisionMask);
        if (GroundHit.collider != null)
        {
            IsGrounded = true;
            GroundNormal = GroundHit.normal;
        }

        WallHit = Physics2D.CircleCast(gameObject.transform.position, radius * radiusMargin, Vector2.right, distance, CollisionMask);
        if (WallHit.collider != null)
        {
            RightWall = true;
            WallNormal = WallHit.normal;
        }

        WallHit = Physics2D.CircleCast(gameObject.transform.position, radius * radiusMargin, Vector2.left, distance, CollisionMask);
        if (WallHit.collider != null)
        {
            LeftWall = true;
            WallNormal = WallHit.normal;
        }
    }
}

public class PlayerPhysics
{
    public PlayerPhysics(Rigidbody2D rb2d)
    {
        Rigidbody2D = rb2d;
    }
    public Rigidbody2D Rigidbody2D;
    public Vector2 Acceleration;
    public float Gravity = 9.8f;
    public float SpeedCap = 20f;
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