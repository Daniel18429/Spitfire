using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] public float Flame;
    public bool extinguished;

    public void FixedUpdate()
    {
        if (Flame <= 0)
        {
            Extinguish();
        }
    }
    
    
    public void Fuel(float amount)
    {
        if(amount <= 0) throw new System.ArgumentException("amount must be greater than or equal to 0");
        Adjust(amount);
    }

    public void Fuel(float amount, float deltaTime)
    {
        if(amount <= 0) throw new System.ArgumentException("amount must be greater than or equal to 0");
        Adjust(amount * deltaTime);
    }
    
    public void Consume(float amount)
    {
        Adjust(-amount);
    }

    public void Consume(float amount, float deltaTime)
    {
        Adjust(-amount * deltaTime);
    }

    public bool HasFlame(float amount)
    {
        return Flame >= amount;
    }
    
    public virtual void Extinguish()
    {
        Flame = 0;
        extinguished = true;
        Destroy(gameObject);
    }

    protected virtual void Adjust(float amount)
    {
        Flame += amount;
    }
}