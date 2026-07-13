using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS STATE IS AT THE ROOT OF ALL TREES
// It ensures that states (such as Airborne and Grounded) can transition
// with one another

// It is an empty state with no purpose other than to transition and can hold no logic
public class RootState<T> : State<T>
{
    // Start is called before the first frame update
    public RootState(StateMachine<T> machine, T info, State<T> parent) : base(machine, info, null)
    {
    }
    
}
