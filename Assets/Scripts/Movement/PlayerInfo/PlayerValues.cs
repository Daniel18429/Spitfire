using System;
using UnityEngine;

[Serializable]
public class PlayerValues
{
    public float JumpingGravity { get; private set; }
    public float UpwardsGravity { get; private set; } // Idk what to call this var but it is inbetween jumping and falling gravity val
    public float FallingGravity { get;  private set; }
    public float JumpVelocity { get; private set; }
    public float JumpHeight { get; private set; } = 7;
    public float JumpTime { get; private set; } = 0.7f;
    public float WallJumpHeight { get; private set; } = 5;
    public float WallJumpTime { get; private set; } = 0.7f;
    public float WallJumpVelocityY { get; private set; } 
    public float WallJumpVelocityX { get; private set; } = 15;
    public float WallJumpGravity { get; private set; }
    
    public float WalkSpeed { get; private set; }
    public float RunSpeed { get; private set; }
    public float DashDistance { get; private set; } = 8f;
    public float DashTime { get; private set; } = 0.33f;
    public PlayerValues()
    {
        JumpVelocity = 2 * JumpHeight / JumpTime;
        WallJumpVelocityY = 2 * WallJumpHeight / WallJumpTime;
        WallJumpGravity = (float)(2 * WallJumpHeight / Math.Pow(WallJumpTime, 2));
        JumpingGravity = (float)(2 * JumpHeight / Math.Pow(JumpTime, 2));
        FallingGravity = JumpingGravity * 1.4f;
        UpwardsGravity = FallingGravity;
    }

}