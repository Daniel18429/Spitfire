using System;
using UnityEngine;

using static Tree<PlayerInfo>;

public class AttackingManagement : MonoBehaviour
{
    private StateMachine<PlayerInfo> _sM;
    private PlayerInfo _playerInfo;
    public void Start()
    {
        StateNode<PlayerInfo>[] children =
        {
            Node<AttackIdle>(),
            Node<Firebolt>()
        };
        _playerInfo = GetComponent<PlayerData>().PlayerInfo;
    }

    public void Update()
    {
        _sM.Update(Time.deltaTime);
    }

    public void FixedUpdate()
    {
        _sM.FixedUpdate(Time.fixedDeltaTime);
    }
}