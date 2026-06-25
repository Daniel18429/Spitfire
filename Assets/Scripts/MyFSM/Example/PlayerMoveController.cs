using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    public PlayerInfo PlayerInfo1 = new PlayerInfo();
    public StateMachine<PlayerInfo> StateMachine1 = new StateMachine<PlayerInfo>();
    private Grounded ground;
    private Airborne airborne;
    private Jumping jumping;
    private Falling falling;
    private Idle idle;
    private Moving moving;

    public void Start()
    {
        StateNode<PlayerInfo>[] children ={
            new StateNode<PlayerInfo>(typeof(Grounded),
                new StateNode<PlayerInfo>(typeof(Idle)),
                new StateNode<PlayerInfo>(typeof(Moving))),
            new StateNode<PlayerInfo>(typeof(Airborne),
                new StateNode<PlayerInfo>(typeof(Jumping)),
                new StateNode<PlayerInfo>(typeof(Falling)))
            };
        StateMachineBuilder<PlayerInfo> build = new StateMachineBuilder<PlayerInfo>(StateMachine1, PlayerInfo1);
        build.BuildTree(children);
        StateMachine1.Initialize(StateMachine1.GetStateFromType<Grounded>());
    }
 
    public void Update()
    {
        StateMachine1.Update(Time.deltaTime);
    }

    public void FixedUpdate()
    {
        StateMachine1.FixedUpdate(Time.fixedDeltaTime);
    }
}

[Serializable]
public class PlayerInfo
{
    public bool ground = false;
    public bool dead = false;
    public bool jumping = false;
    public bool moving = false;
}