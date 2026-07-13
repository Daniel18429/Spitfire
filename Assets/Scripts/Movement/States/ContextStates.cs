using UnityEngine;

public class Grounded : State<PlayerInfo>
{
    private float jumpCost = 10f;
    private float slideCost = 10f;
    public Grounded(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
        _info.Physics.Friction = 0.06f;
    }

    protected override void OnExit()
    {
        _info.Timers.CayoteTime.Reset(0.2f);
    }

    protected override State<PlayerInfo> Transition()
    {
        if (!_info.Context.IsGrounded)
        {
            if (_info.Context.LeftWall || _info.Context.RightWall)
            {
                return Machine.GetStateFromType<Walled>();
            }
            else
            {
                
                return Machine.GetStateFromType<Airborne>();
            }
        }
        else
        {
            if (_info.Input.JumpPressed && _info.Fire.HasFlame(_info.Cost.JumpCost))
            {
                return Machine.GetStateFromType<Jumping>();
            }

            else if (_info.Input.DashPressed && _info.Fire.HasFlame(_info.Cost.DashCost) && _info.Timers.DashCooldown.Done)
            {
                return Machine.GetStateFromType<Dash>();
            }
            else if (_info.Input.SlidePressed && _info.Fire.HasFlame(_info.Cost.SlideCost))
            {
                return Machine.GetStateFromType<Sliding>();
            }
            else
            {
                return null;
            }
        }
    }
    
    protected override State<PlayerInfo> GetInitialState()
    {
        return Machine.GetStateFromType<Idle>();
    }

    protected override void OnUpdate(float deltaTime) { }
    protected override void OnFixedUpdate(float deltaTime) { }
}

public class Airborne : State<PlayerInfo>
{
    public Airborne(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }

    protected override void OnEnter()
    {
        _info.Physics.Friction = 0.02f;
        
    }
    
    protected override void OnExit() { }

    protected override State<PlayerInfo> Transition()
    {
        if(_info.Context.IsGrounded) return Machine.GetStateFromType<Grounded>();
        else if (_info.Context.LeftWall || _info.Context.RightWall && ActiveChild != Machine.GetStateFromType<Jumping>())
        {
            return Machine.GetStateFromType<Walled>();
        }
        else
        {
            if (_info.Input.JumpPressed && _info.Fire.HasFlame(_info.Cost.JumpCost) && !_info.Timers.CayoteTime.Done)
            {
                return Machine.GetStateFromType<Jumping>();
            }
            else if (_info.Input.DashPressed && _info.Fire.HasFlame(_info.Cost.DashCost) && _info.Timers.DashCooldown.Done)
            {
                return Machine.GetStateFromType<Dash>();
            }
            else
            {
                return null;
            }
        }
    }
    protected override State<PlayerInfo> GetInitialState() => Machine.GetStateFromType<Falling>();
    protected override void OnUpdate(float deltaTime) { }
    protected override void OnFixedUpdate(float deltaTime) { }
}

public class Walled : HorizontalMove
{
    private float _slideSpeed = 2f;
    public Walled(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
        moveSpeed = 1;
    }

    protected override void OnEnter()
    {
    }
    
    protected override void OnExit() { }

    protected override State<PlayerInfo> Transition()
    {
        if(_info.Context.IsGrounded) return Machine.GetStateFromType<Grounded>();
        else if (_info.Context.LeftWall || _info.Context.RightWall)
        {
            if (_info.Input.JumpPressed && _info.Fire.HasFlame(_info.Cost.JumpCost))
            {
                return Machine.GetStateFromType<WallJump>();
            }
            else
            {
                return null; 
            }
        }
        else
        {
            return Machine.GetStateFromType<Airborne>();
        }
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