using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    // General Component Variables
    private Rigidbody2D _rb;
    private CircleCollider2D _col;
    
    // State Machine Variables
    public enum MovementState
    {
        Walking,
        Dashing,
        Jumping,
        Idle,
        Sliding,
    };
    [SerializeField] private MovementState _state = MovementState.Sliding;
    private float nullMoveTime = 0.0f;
    [SerializeField] private bool _onGround = false;
    
    // Direction Variables
    private Vector2 _inputDir;
    private Vector2 _dir;
    private Vector2 _surfaceNormal;
    
    // Physics Variables
    private float _xSpeedCap;
    private float _ySpeedCap;
    private float _xFriction = 0.1f;
    private float _gravity = 9.8f;
    [SerializeField] private float _xAcceleration;
    [SerializeField] private float _yAcceleration;
    
    // Walk Variables
    private float _walkingSpeed = 10f;
    
    // Slide Variables
    private float _slideSpeed = 8f;
    
    // Jump Variables
    private int _airJumps;
    private int _maxAirJumps = 1;
    [SerializeField] private float _cayoteTime;
    private float _maxCayoteTime = 0.1f;
    private float _jumpForce = 9f;
    private float _jumpTime;
    private float _maxJumpTime = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CircleCollider2D>();
        _gravity = 9.81f;
        _xSpeedCap = Mathf.Infinity;
        _ySpeedCap = Mathf.Infinity;
    }
    

    // Update is called once per frame
    void Update()
    {
        VarSet();
        RecieveInput(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), Input.GetKeyDown(KeyCode.Space), Input.GetKeyDown(KeyCode.LeftShift));
        StateMachine();
    }

    void FixedUpdate()
    {
        Physics();
    }
    
    
    // Reset all variables
    void VarSet()
    {
        _dir = _rb.velocity;
        _cayoteTime -= Time.deltaTime;
        _jumpTime -= Time.deltaTime;
        GroundCheck();
    }
    
    void GroundCheck()
    {
        LayerMask layer = LayerMask.GetMask("Objects"); 
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _col.radius * 0.99f, Vector2.down, 0.02f,layer);
        if (!hit.collider || !hit.collider.transform.gameObject.CompareTag("Ground"))
        {
            _onGround = false;
            return;
        }
        _onGround = true;
        _cayoteTime = _maxCayoteTime;
        _airJumps = _maxAirJumps;
    }
    
    // Updating based on inputs
    void RecieveInput(Vector2 inpDir, bool jump, bool dash)
    {
        // Inputless
        if (nullMoveTime > 0.0f)
        {
            nullMoveTime -= Time.deltaTime;
            return;
        }
        
        // Utilize Inputs
        _inputDir = inpDir;
        if (jump) Jump();
        if(dash) Dash();
        
        
    }

    void Dash()
    {
        // TEMPORARY
        if(_onGround && _state != MovementState.Sliding && _surfaceNormal.x != 0) SetNextState(MovementState.Sliding);
    }

    void SwitchState()
    {
        // TEMPORARY
        SetNextState(MovementState.Walking);
    }
    
    

    void StateMachine()
    {
        switch (_state)
        {
            case MovementState.Idle:
                break;
            case MovementState.Walking:
                _xAcceleration = 0;
                _yAcceleration = 0;
                
                _xFriction = 0.2f;
                Vector2 parVector = Vector2.right;
                if (_onGround)
                {
                    if (_surfaceNormal.x > 0)
                    {
                        parVector = new Vector2(_surfaceNormal.y, -_surfaceNormal.x);
                    }
                    else if (_surfaceNormal.x < 0)
                    {
                        parVector = new Vector2(-_surfaceNormal.y, _surfaceNormal.x);
                    }

                    _dir = parVector.normalized * _walkingSpeed * _inputDir.x * Mathf.Sign(parVector.x);
                }
                else
                {
                    if (_inputDir.x == 0)
                    {
                        _xFriction = 10f;
                    }
                    else if (Mathf.Sign(_inputDir.x) == Mathf.Sign(_dir.x) && Mathf.Abs(_dir.x) > Mathf.Abs(_walkingSpeed))
                    {
                        _xFriction = 1f;
                    }
                    else
                    {
                        _dir = new Vector2(_inputDir.x * _walkingSpeed, _dir.y);
                    }
                    // if (_inputDir.x == 0)
                    // {
                    //     _xFriction = 10f;
                    // }
                    // else if (Mathf.Abs(_dir.x) <= _walkingSpeed)
                    // {
                    //     _dir = new Vector2(_inputDir.x * _walkingSpeed, _dir.y);
                    // }
                    // else if (Mathf.Sign(_inputDir.x) == Mathf.Sign(_dir.x))
                    // {
                    //     _xFriction = 2f;
                    // }
                    // else
                    // {
                    //     _xFriction = 20f;
                    // }
                }
                break;
            case MovementState.Dashing:
                break;
            case MovementState.Sliding:
                if (!_onGround) SwitchState();
                _xFriction = 0.1f;
                if (_surfaceNormal.x == 0) _xFriction = 4.0f;
                 
                // Parallel Vector Calculation
                Vector2 parallelVector = Vector2.right;
                if (_surfaceNormal.x > 0)
                {
                    parallelVector = new Vector2(_surfaceNormal.y, -_surfaceNormal.x);
                }
                else if (_surfaceNormal.x < 0)
                {
                    parallelVector = new Vector2(-_surfaceNormal.y, _surfaceNormal.x);
                } 
                
                Vector2 slideDir = Vector2.zero;
                if (_surfaceNormal.y != 0) slideDir = parallelVector.normalized * _slideSpeed / _surfaceNormal.y;
                float slideSign = Mathf.Sign(slideDir.x);
                if (_inputDir.x == 0)
                {
                    _xAcceleration = 0;
                    _yAcceleration = 0;
                    if (Mathf.Abs(_dir.x) > 0 && Mathf.Sign(_dir.x) != slideSign)
                    {
                        SwitchState();
                    }
                    
                    // If on parallel surface and moving slower than walking, switch state
                    if (_surfaceNormal.x == 0 && Mathf.Abs(_dir.x) < _walkingSpeed) SwitchState();
                }
                else if (Mathf.Sign(slideDir.x) == Mathf.Sign(_inputDir.x))
                {
                    _xAcceleration = slideDir.x;
                    _yAcceleration = slideDir.y;
                    if (_surfaceNormal.x == 0) 
                    {
                        _xAcceleration = 0;
                        _yAcceleration = 0;
                        
                        // If on parallel surface and moving slower than walking, switch state
                        if(Mathf.Abs(_dir.x) < _walkingSpeed) SwitchState();
                    }
                    if (Mathf.Abs(_dir.x) > 0 && Mathf.Sign(_dir.x) != slideSign)
                    {
                        SwitchState();
                    }
                }
                else if (slideSign == -Mathf.Sign(_inputDir.x))
                {
                    _xFriction = Mathf.Max(Mathf.Abs(slideDir.x), _xFriction);
                    _xAcceleration = 0;
                    _yAcceleration = 0;
                }
                break; 
            case MovementState.Jumping:
                _xAcceleration = 0;
                _yAcceleration = 0;
                if (_jumpTime <= 0) SwitchState();
                break;
        }
    }

    void SetNextState(MovementState newState)
    {
        _state = newState;
        switch (_state)
        {
            case MovementState.Sliding:
                // Calculate parallel slide vector
                Vector2 parallelVector = Vector2.right;
                if (_surfaceNormal.x > 0)
                {
                    parallelVector = new Vector2(_surfaceNormal.y, -_surfaceNormal.x);
                }
                else if (_surfaceNormal.x < 0)
                {
                    parallelVector = new Vector2(-_surfaceNormal.y, _surfaceNormal.x);
                } 
                Vector2 slideDir = parallelVector.normalized * _surfaceNormal.y * 1;
                _dir = new Vector2(_dir.x +  slideDir.x, _dir.y +  slideDir.y); // Give instant "Boost"
                break;
        }
    }

    void Jump()
    {
        if (_cayoteTime < 0 && _airJumps <= 0) return; // Not on ground and no airjumps left
        if (_cayoteTime > 0) _cayoteTime = 0; // Still on ground
        else _airJumps--; // Use airJump
        Vector2 jumpDir = Vector2.up;
        switch (_state)
        {
            case MovementState.Dashing:
                jumpDir = _surfaceNormal;
                break;
        }
        jumpDir *= _jumpForce;
        _dir = new Vector2(_dir.x + jumpDir.x, jumpDir.y);
        _jumpTime = _maxJumpTime;
        SetNextState(MovementState.Jumping);
    }
    

    // PHYSICS
    void Physics()
    {
        _rb.velocity = _dir;
        
        // "Friction", applying x acceleration
        if (_rb.velocity.x != 0)
        {
            float sign = Mathf.Sign(_rb.velocity.x);
            _rb.velocity = new Vector2(_rb.velocity.x - sign * Mathf.Abs(_xFriction) * Time.deltaTime, _rb.velocity.y);
            if (sign > 0 && _rb.velocity.x < 0)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }

            if (sign < 0 && _rb.velocity.x > 0)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
        }
        _rb.velocity = new Vector2(_rb.velocity.x + _xAcceleration * Time.deltaTime, _rb.velocity.y + _yAcceleration * Time.deltaTime);
        
        // Gravity
        if(!_onGround) _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y - _gravity * Time.deltaTime);
        
        // Setting the Speed Cap
        if (Mathf.Abs(_rb.velocity.x) > _xSpeedCap)
        {
            _rb.velocity = new Vector2(_xSpeedCap * Mathf.Sign(_rb.velocity.x), _rb.velocity.y);
        }
        if (Mathf.Abs(_rb.velocity.y) > _ySpeedCap)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _ySpeedCap * Mathf.Sign(_rb.velocity.y));
        }
    }
    
    //------- COLLISION FUNCTIONS -------
    void OnCollisionStay2D(Collision2D col)
    {
        GetSurfaceNormal(col);

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        GetSurfaceNormal(col);
    }
    void OnCollisionExit2D(Collision2D col)
    {
        _onGround = false;
    }

    //------- COLLISION HELPER FUNCTIONS -------
    void GetSurfaceNormal(Collision2D col)
    {
        if(col.gameObject.CompareTag("Ground") && col.contactCount > 0) _surfaceNormal = col.contacts[0].normal;
    }
}