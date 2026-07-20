using UnityEngine;

using static Tree<PlayerInfo>;

public class MovementController : MonoBehaviour
{
    [SerializeField] public PlayerInfo _playerInfo { get; private set; }
    [SerializeField] private StateMachine<PlayerInfo> _stateMachine = new StateMachine<PlayerInfo>();
    [SerializeField] private PlayerValues values;
    [SerializeField] private PlayerCost cost;
    
    public void Awake()
    {
        _playerInfo = new PlayerInfo(this.gameObject, values, cost);
        _playerInfo.Init();
        StateNode<PlayerInfo>[] children =
        {
            Node<Dash>(),
            Node<Grounded>(
                Node<Idle>(),
                Node<Walking>(),
                Node<Sliding>()),
            Node<Airborne>(
                Node<Jumping>(
                    Node<NormalJump>(),
                    Node<WallJump>()),
                Node<WallJump>()),
            Node<Walled>(
                Node<WallSliding>())
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
            Input.GetKey(KeyCode.Space), Input.GetMouseButtonDown(1), Input.GetKey(KeyCode.LeftShift));
        _stateMachine.Update(Time.deltaTime);
    }

    public void FixedUpdate()
    {
        _playerInfo.Context.UpdateContext(gameObject);
        _stateMachine.FixedUpdate(Time.fixedDeltaTime);
        _playerInfo.FixedUpdate(Time.fixedDeltaTime);
    }
}