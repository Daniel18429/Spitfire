public class Template : State<PlayerInfo>
{
    public Template(StateMachine<PlayerInfo> machine, PlayerInfo info, State<PlayerInfo> parent) : base(machine, info, parent)
    {
    }
    
    protected override void OnEnter() { }
    
    protected override void OnExit() { }

    protected override State<PlayerInfo> Transition() => null;
    
    protected override State<PlayerInfo> GetInitialState() => null;

    protected override void OnUpdate(float deltaTime) { }
    protected override void OnFixedUpdate(float deltaTime) { }
}