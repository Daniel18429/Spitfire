
using UnityEngine;

public class FlameOrb : Fire
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        BigFlame bigFlame = collision.GetComponent<BigFlame>();
        if (bigFlame != null && !extinguished)
        {
            bigFlame.Fuel(Flame);
            Extinguish();
        }
    }
}
