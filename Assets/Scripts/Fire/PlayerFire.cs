using UnityEngine;

public class PlayerFire : Fire
{

    public override void Start()
    {
        maxFlame = 60;
        Debug.Log(maxFlame);
        dwindleAmount = 1;
        base.Start();
    }
// UTIL FUNCTIONS
}