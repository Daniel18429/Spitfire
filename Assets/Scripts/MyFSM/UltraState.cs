using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraState<T> : State<T>
{
    // Start is called before the first frame update
    public UltraState(StateMachine<T> machine, T info) : base(machine, info, null)
    {
    }
    
}
