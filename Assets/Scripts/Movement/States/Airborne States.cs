using UnityEngine;
public class Falling : HorizontalMove
{
    private float gravity = 14f;
    public Falling(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
        // In case they have changed
        moveSpeed = _info.Val.AirWalkSpeed;
        gravity = _info.Val.UpwardsGravity;
        
        
        _info.Physics.Gravity = gravity;
    }
    protected override void OnExit() { }
    protected override State<PlayerInfo> Transition() => null;
}

public class Jumping : State<PlayerInfo>
{
    public Jumping(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override State<PlayerInfo> GetInitialState()
    {
        return Machine.GetStateFromType<NormalJump>();
    }
    
}

public class WallJump : HorizontalMove
{
    
    private MyTimer _noHorizontalMoveTimer = new MyTimer();
    public WallJump(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
        
        moveSpeed = _info.Val.AirWalkSpeed;
        
        
        _noHorizontalMoveTimer.Reset(_info.Val.WallJumpUncontrolledTime);
        _info.Physics.Gravity = _info.Val.WallJumpGravity;
        Vector2 velocity = _info.Physics.Rigidbody2D.velocity;
        velocity.y = _info.Val.WallJumpVelocityY;
        int wallDir = 0;
        if (_info.Context.LeftWall) wallDir = -1;
        else if (_info.Context.RightWall) wallDir = 1;
        velocity.x = -wallDir * _info.Val.WallJumpVelocityX;
        _info.Physics.Rigidbody2D.velocity = velocity;
        _info.Context.RightWall = _info.Context.LeftWall = false;
    }

    protected override void OnExit()
    {
    }

    protected override State<PlayerInfo> Transition()
    {
        if ((!_info.Input.JumpPressed && _noHorizontalMoveTimer.Done) || _info.Physics.Rigidbody2D.velocity.y < 0)
        {
            return Machine.GetStateFromType<Falling>();
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
        _noHorizontalMoveTimer.Tick(deltaTime);
        if (_noHorizontalMoveTimer.Done)
        {
            Debug.Log(_info.Physics.Rigidbody2D.velocity.x);
            base.OnFixedUpdate(deltaTime);
        }
    }
}


public class NormalJump : HorizontalMove
{
    private MyTimer _jumpDurationTimer = new MyTimer();
    private float _jumpDuration;
    private float _jumpForce = 10f;
    private float gravity = 10f;
    public NormalJump(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
        moveSpeed = _info.Val.AirWalkSpeed;
        _jumpDuration = _info.Val.JumpTime;
        _jumpForce = _info.Val.JumpVelocity;
        gravity = _info.Val.JumpingGravity;
        
        
        _info.Context.IsGrounded = false; // MAKING SURE WE DON'T RESET INTO INFINITE CYCLE
        _jumpDurationTimer.Reset(_jumpDuration);
        Vector2 velocity = _info.Physics.Rigidbody2D.velocity;
        velocity.y = _jumpForce;
        _info.Physics.Rigidbody2D.velocity = velocity;

        _info.Physics.Gravity = gravity;
    }

    protected override void OnExit()
    {
        _jumpDurationTimer.Reset(0);
    }

    protected override State<PlayerInfo> Transition()
    {
        if (!_info.Input.JumpPressed || _jumpDurationTimer.Done)
        {
            return Machine.GetStateFromType<Falling>();
        }
        else
        {
            return null;
        }
    }

    protected override void OnUpdate(float deltaTime) { }

    protected override void OnFixedUpdate(float deltaTime)
    {
        base.OnFixedUpdate(deltaTime);
        _jumpDurationTimer.Tick(deltaTime);
    }
}