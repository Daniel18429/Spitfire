using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStateMachine _stateMachine;
    public Rigidbody2D _Rb { get; private set; }
    
    
    

    void Awake()
    {
        _Rb = GetComponent<Rigidbody2D>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine._CurrentState.HandleInput(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), Input.GetKeyDown(KeyCode.Space), Input.GetKeyDown(KeyCode.LeftShift));
        
            
    }

    void FixedUpdate()
    {
        this.Physics();
    }

    void Physics()
    {
        _Rb.MovePosition(_Rb.position + _stateMachine._CurrentState.PhysicsUpdate() * Time.fixedDeltaTime);
    }
    
}
