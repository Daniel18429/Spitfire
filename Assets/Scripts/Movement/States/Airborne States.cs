using UnityEngine;
public class Falling : HorizontalMove
{
    private float gravity = 14f;
    public Falling(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
        moveSpeed = 5f;
        gravity = _info.Val.UpwardsGravity;
    }

    protected override void OnEnter()
    {
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
    public WallJump(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
    }
    
    protected override void OnExit() { }

    protected override State<PlayerInfo> Transition() => null;
    
    protected override State<PlayerInfo> GetInitialState() => null;

    protected override void OnUpdate(float deltaTime) { }

    protected override void OnFixedUpdate(float deltaTime)
    {
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
        moveSpeed = 5f;

        _jumpDuration = _info.Val.JumpTime;
        _jumpForce = _info.Val.JumpVelocity;
        gravity = _info.Val.JumpingGravity;
    }

    protected override void OnEnter()
    {
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