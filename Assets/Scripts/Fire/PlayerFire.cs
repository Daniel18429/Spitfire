using UnityEngine;

public class PlayerFire : BigFlame
{
    private float maxFlame;
    private PlayerCost _costs;
    private float _dwindleAmount;

    public void Start()
    {
        _costs = this.GetComponent<MovementController>()._playerInfo.Cost;
        maxFlame = _costs.MaxFire;
        Flame = maxFlame;
        _dwindleAmount = 1;
    }

    protected override void Adjust(float amount)
    {
        base.Adjust(amount);
        if(Flame > maxFlame)
        {
            Flame = maxFlame;
        }
    }
    public void FixedUpdate()
    {
        Consume(_dwindleAmount, Time.fixedDeltaTime);
    }
}