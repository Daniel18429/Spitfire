using UnityEngine;
using System;
public class PlayerPhysics
{
    public PlayerPhysics(Rigidbody2D rb2d)
    {
        Rigidbody2D = rb2d;
    }
    public Rigidbody2D Rigidbody2D;
    public Vector2 Acceleration;
    public float Gravity = 9.8f;
    public float SpeedCap = 50f;
    public float YMax;
    public float Friction = 0.1f;

    public void PhysicsUpdate(float deltaTime)
    {
        Rigidbody2D.velocity += Acceleration * deltaTime;
        Rigidbody2D.velocity -= new Vector2(0, Gravity) * deltaTime;
        if (Rigidbody2D.velocity.magnitude > SpeedCap)
        {
            Rigidbody2D.velocity = Vector2.ClampMagnitude(Rigidbody2D.velocity, SpeedCap);
        }

        if (Rigidbody2D.velocity.x != 0)
        {
            int xVelocitySign =  Math.Sign(Rigidbody2D.velocity.x);
            
            Vector2 velocity = Rigidbody2D.velocity;
            if (xVelocitySign == 1)
            {
                velocity.x -= Friction;
            }
            else
            {
                velocity.x += Friction;
            }
            Rigidbody2D.velocity = velocity;

            if (Math.Sign(Rigidbody2D.velocity.x) != xVelocitySign)
            {
                Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y);
            }
            
        }
    }
}