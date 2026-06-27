using UnityEngine;

public class MovementController : MonoBehaviour
{
    private PlayerInfo _playerInfo;
    private StateMachine<PlayerInfo> _stateMachine = new StateMachine<PlayerInfo>();
    
    public void Awake()
    {
        _playerInfo = new PlayerInfo(this.gameObject);
        StateNode<PlayerInfo>[] children =
        {
            new StateNode<PlayerInfo>(typeof(Idle)),
            new StateNode<PlayerInfo>(typeof(Walking)),
        };
        StateMachineBuilder<PlayerInfo> builder = new StateMachineBuilder<PlayerInfo>(_stateMachine, _playerInfo);
        builder.BuildTree(children);
        _stateMachine.Initialize(_stateMachine.GetStateFromType<Walking>());
    }

    public void Start()
    {
        
    }

    public void Update()
    {
        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _playerInfo.Input.CacheInput(moveDirection, 
            Input.GetKey(KeyCode.Space), Input.GetKey(KeyCode.LeftShift));
        _stateMachine.Update(Time.deltaTime);
    }

    public void FixedUpdate()
    {
        _stateMachine.FixedUpdate(Time.fixedDeltaTime);
        _playerInfo.Physics.PhysicsUpdate(Time.fixedDeltaTime);
        _playerInfo.Input.Reset();
        
    }
}