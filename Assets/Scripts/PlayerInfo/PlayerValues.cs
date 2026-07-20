using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerValues", menuName = "ScriptableObjects/PlayerValues", order = 1)]
public class PlayerValues : ScriptableObject
{
    [Header("Jump")]
    [SerializeField] private float cayoteTime = 0.4f;
    public float CayoteTime => cayoteTime;
    [Header("NormalJump")] 
    [SerializeField] private float jumpHeight = 7f;

    [SerializeField] private float jumpTime = 0.7f;
    
    public float JumpHeight => jumpHeight;
    public float JumpTime => jumpTime;
    public float JumpVelocity => 2 * JumpHeight / JumpTime;
    public float JumpingGravity => (float)(2 * JumpHeight / Math.Pow(JumpTime, 2));
    
    [Header("WallJump")]
    [SerializeField] private float wallJumpHeight = 3.14f;
    [SerializeField] private float wallJumpTime = 0.4f;
    [SerializeField] private float wallJumpUncontrolledTime = 0.24f;
    [SerializeField] private float wallJumpVelocityX = 20f;
    
    public float WallJumpHeight => wallJumpHeight;
    public float WallJumpTime => wallJumpTime;
    public float WallJumpVelocityY => 2 * WallJumpHeight / WallJumpTime;
    public float WallJumpGravity => (float)(2 * WallJumpHeight / Math.Pow(WallJumpTime, 2));
    public float WallJumpVelocityX => wallJumpVelocityX;
    public float WallJumpUncontrolledTime => wallJumpUncontrolledTime;
    
    [Header("WallSliding")]
    [SerializeField] private float wallSlidingGravity = 2f;
    [SerializeField] private float maxWallSpeed = 0.4f;
    public float WallSlidingGravity => wallSlidingGravity;
    public float MaxWallSpeed => maxWallSpeed;
    
    [Header("MovementSpeeds")]
    [SerializeField] private float groundWalkSpeed = 9f;
    [SerializeField] private float airWalkSpeed = 10f;
    public float GroundWalkSpeed => groundWalkSpeed;
    public float AirWalkSpeed => airWalkSpeed;
    
    [Header("Dash")]
    [SerializeField] private float dashDistance = 8f;
    [SerializeField] private float dashTime = 0.7f;
    
    public float DashDistance => dashDistance;
    public float DashTime => dashTime;
    
    [Header("GravityMultiplier")]
    [SerializeField] private float upwardsGravityMultiplier = 1.4f;
    [SerializeField] private float fallingGravityMultiplier = 1.6f;

    public float UpwardsGravity => JumpingGravity * upwardsGravityMultiplier; // Idk what to call this var but it is inbetween jumping and falling gravity val
    public float FallingGravity => JumpingGravity * fallingGravityMultiplier;
    
    [Header("Friction")]
    [SerializeField] private float groundFriction = 0.6f;
    [SerializeField] private float airFriction = 0.02f;
    public float GroundFriction => groundFriction;
    public float AirFriction => airFriction;

}