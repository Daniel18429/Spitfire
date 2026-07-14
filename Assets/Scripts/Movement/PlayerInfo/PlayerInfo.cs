using System;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public PlayerInput Input;
    public PlayerContext Context;
    public PlayerPhysics Physics;
    public PlayerTimers Timers;
    public PlayerFire Fire;
    public PlayerCost Cost;
    public GameObject Player;
    
    
    public PlayerInfo(GameObject gameObject)
    {
        Player = gameObject;
        Physics = new PlayerPhysics(gameObject.GetComponent<Rigidbody2D>());
        Input = new PlayerInput();
        Context = new PlayerContext();
        Timers = new PlayerTimers();
        Fire = gameObject.GetComponent<PlayerFire>();
        Cost = new PlayerCost();
    }

    public void Init()
    {
        Context.Init();
        Input.Start(Player);
    }

    public void FixedUpdate(float deltaTime)
    {
        Physics.PhysicsUpdate(deltaTime);
        Timers.Tick(deltaTime);
        Context.UpdateContext(Player);
        Input.Reset();
    }
}

public class PlayerCost
{
    public float DashCost { get; private set; } = 5f;
    public float JumpCost { get; private set; } = 1f;
    public float SlideCost { get; private set; } = 1f;
}

public class Values
{

    public float JumpingGravity { get; private set; }
    public float UpwardsGravity { get; private set; } // Idk what to call this var but it is inbetween jumping and falling gravity val
    public float FallingGravity { get;  private set; }
    public float JumpVelocity { get; private set; }
    public float JumpHeight { get; private set; } = 10;
    public float JumpTime { get; private set; } = 0.8f;
    
    public float WalkSpeed { get; private set; }
    public float RunSpeed { get; private set; }
    public float DashDistance { get; private set; }
    public float DashTime { get; private set; }
    public Values()
    {
        JumpVelocity = 2 * JumpHeight / JumpTime;
        JumpingGravity = JumpHeight / JumpTime;
        FallingGravity = JumpingGravity * 2;

    }

}

public class PlayerInput 
{
    public Vector2 mouseDir;
    private Transform objToMouse;
    public Vector2 MoveDirection; 
    public bool JumpPressed;
    public bool DashPressed;
    public bool SlidePressed;

    public void Start(GameObject player)
    {
        objToMouse = player.transform;
    }

    public void CacheInput(Vector2 moveDirection, bool jumpPressed, bool dashPressed, bool slidePressed)
    {
        MoveDirection = moveDirection;
        if(!JumpPressed) JumpPressed = jumpPressed;
        if (!DashPressed) DashPressed = dashPressed;
        if(!SlidePressed) SlidePressed = slidePressed;
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        mouseDir = (mouseWorldPos - objToMouse.position).normalized;
    }

    public void Reset()
    {
        MoveDirection = Vector2.zero;
        JumpPressed = false;
        DashPressed = false;
        SlidePressed = false;
    }
}

public class PlayerTimers
{
    public MyTimer CayoteTime = new MyTimer();
    public MyTimer DashCooldown = new MyTimer();

    public void Tick(float deltaTime)
    {
        CayoteTime.Tick(deltaTime);
        DashCooldown.Tick(deltaTime);
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