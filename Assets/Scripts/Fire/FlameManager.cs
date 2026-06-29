using UnityEngine;

public class Fire : MonoBehaviour
{
    public float Flame { get; private set; }
    private float regularDecreasePerSecond;
    private float maxFlame;
    public bool extinguished;
    
    public void Start()
    {
        Flame = maxFlame;
    }

    public void FixedUpdate()
    {
        Consume(regularDecreasePerSecond, Time.fixedDeltaTime);
    }
    
    // UTIL FUNCTIONS
    public bool Consume(float amount)
    {
        if(amount <= 0) throw new System.ArgumentException("amount must be greater than or equal to 0");
        if(Flame >= amount)
        {
            Adjust(-amount);
            return true;
        }

        return false;
    }

    public bool Consume(float amount, float deltaTime)
    {
        
        if(amount <= 0) throw new System.ArgumentException("amount must be greater than or equal to 0");
        if (Flame >= amount * deltaTime)
        {
            Adjust(-amount * deltaTime);
            return true;
        }
        return false;
    }

    public void Extinguish()
    {
        return;
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

    public void Light()
    {
        return;
    }

    public void Update()
    {
        Light();
    }

    private void Adjust(float amount)
    {
        Flame += amount;
        if(Flame > maxFlame) Flame = maxFlame;
    }
}

public class PlayerFire : Fire
{
    
}