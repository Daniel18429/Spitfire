using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] public PlayerInfo PlayerInfo { get; private set; }
    [SerializeField] private PlayerValues values;
    [SerializeField] private PlayerCost cost;

    public void Awake()
    {
        PlayerInfo = new PlayerInfo(this.gameObject, values, cost);
        PlayerInfo.Init();
        
    }
}