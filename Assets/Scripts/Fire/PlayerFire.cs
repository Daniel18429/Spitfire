using UnityEngine;

public class PlayerFire : Fire
{

    public override void Start()
    {
        maxFlame = 100000;
        dwindleAmount = 0;
        base.Start();
    }
// UTIL FUNCTIONS
}