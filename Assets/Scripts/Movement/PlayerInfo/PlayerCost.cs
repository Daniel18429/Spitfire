using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerCost", menuName = "ScriptableObjects/PlayerCost", order = 1)]
public class PlayerCost : ScriptableObject
{

    [Header("General")] 
    [SerializeField] private float maxFire = 60;
    public float MaxFire => maxFire;
    
    [Header("Instant Costs")] 
    [SerializeField] private float dashCost = 5f;
    [SerializeField] private float jumpCost = 1f;
    [SerializeField] private float slideCost = 1f;
    
    public float DashCost => dashCost;
    public float JumpCost => jumpCost;
    public float SlideCost => slideCost;
}
