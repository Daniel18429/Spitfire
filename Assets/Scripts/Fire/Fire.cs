using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] public float Flame;
    protected float maxFlame;
    protected float dwindleAmount;
    public bool extinguished;
    protected float innerRadius;
    protected float outerRadius;
    
    public virtual void Start()
    {
        Flame = maxFlame;
        Debug.Log(Flame);
    }

    public void FixedUpdate()
    {
        Dwindle();
    }
        
    public virtual void Dwindle()
    {
        Consume(dwindleAmount *  Time.fixedDeltaTime);
        Collider2D[] overlap = Physics2D.OverlapCircleAll(transform.position, outerRadius, LayerMask.GetMask("Player"));
        foreach (Collider2D obj in overlap)
        {
            if(obj.gameObject == this.gameObject) continue;
            PlayerFire fire = obj.GetComponent<PlayerFire>();
            if (fire != null)
            {
                Debug.Log(fire.name);
                float dist =  Vector2.Distance(fire.transform.position, transform.position);
                if (dist <= innerRadius)
                {
                    fire.Fuel(Flame);
                    this.Extinguish();
                }
            }
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
        Consume(Flame);
    }

    protected void Adjust(float amount)
    {
        Flame += amount;
        if(Flame > maxFlame) Flame = maxFlame;
    }
}