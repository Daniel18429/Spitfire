using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private PlayerInfo _playerInfo;
    [SerializeField] private StateMachine<PlayerInfo> _stateMachine = new StateMachine<PlayerInfo>();
    
    public void Awake()
    {
        _playerInfo = new PlayerInfo(this.gameObject);
        _playerInfo.Init();
        StateNode<PlayerInfo>[] children =
        {
            new StateNode<PlayerInfo>(typeof(Grounded),
                new StateNode<PlayerInfo>(typeof(Idle)),
                new StateNode<PlayerInfo>(typeof(Walking))
                ),
            new StateNode<PlayerInfo>(typeof(Airborne),
                new StateNode<PlayerInfo>(typeof(Jumping)),
                new StateNode<PlayerInfo>(typeof(Falling)))
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
        _playerInfo.Context.UpdateContext(this.gameObject);
        _stateMachine.FixedUpdate(Time.fixedDeltaTime);
        _playerInfo.Physics.PhysicsUpdate(Time.fixedDeltaTime);
        _playerInfo.Input.Reset();
        
    }
}