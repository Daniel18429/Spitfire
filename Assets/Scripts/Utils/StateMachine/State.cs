using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls what is happening in each state and where to go on a transition
public abstract class State<T>
{
    // VARIABLES
    public readonly State<T> Parent;
    public State<T> ActiveChild { get; private set; } // Null == Leaf Node
    protected StateMachine<T> Machine { get; private set; }
    protected T _info; // Instance of context state has access to
    
    
    protected State(StateMachine<T> machine, T info, State<T> parent = null)
    {
        Parent = parent;
        Machine = machine;
        Machine.AddState(this);
        _info = info;
    }
    
    // FUNCTIONS THAT CAN BE OVERRIDDEN BY CHILDREN
    protected virtual void OnEnter(){} // What happens on entering the state
    protected virtual void OnExit(){} // What happens on exiting the state
    protected virtual void OnUpdate(float deltaTime){} // What happens on update in the state
    protected virtual void OnFixedUpdate(float deltaTime){} // What happens on fixed update in the state (physics)
    protected virtual State<T> GetInitialState() => null; // What is the assumed active child (Null == set this as leaf)

    protected virtual State<T> Transition() => null; // Under what conditions does this state move to another
    

    // Enter only this state
    public void Enter()
    {
        if(Parent != null) Parent.ActiveChild = this;
        OnEnter();
    }

    // Enter this state and activate children through assumptions
    public void RecursiveEnter()
    {
        Enter();
        State<T> s = GetInitialState();
        if (s != null)
        {
            s.RecursiveEnter();
        }
    }

    // Never used
    public void Exit()
    {
        if(Parent != null) Parent.ActiveChild = null;
        OnExit();
    }

    // Exit out this state and all active children
    public void RecursiveExit() // Recursively exit CHILDREN. Only needs to be called once on the Active Child of the LCA
    {
        if(ActiveChild != null) ActiveChild.RecursiveExit();
        Exit();
    }
    

    public void Update(float deltaTime)
    {
        OnUpdate(deltaTime);
        ActiveChild?.Update(deltaTime);
    }

    public void FixedUpdate(float deltaTime)
    {
        OnFixedUpdate(deltaTime);
        ActiveChild?.FixedUpdate(deltaTime);
    }

    public State<T> CallTransition()
    {
        State<T> selfTransition = Transition();
        if(selfTransition != null) return selfTransition;
        if (ActiveChild != null)
        {
            State<T> childTransition = ActiveChild.CallTransition();
            if (childTransition != null) return childTransition;
        }

        return null;
    }
    
    // Get active leaf
    public State<T> Leaf()
    {
        State<T> s = this;
        while(s.ActiveChild != null) s = s.ActiveChild;
        return s;
    }
    
    // Get path to root (Leaf = 0, Root = end)
    public List<State<T>> PathToRoot()
    {
        List<State<T>> path = new List<State<T>>();
        for (State<T> s= this; s != null; s = s.Parent) path.Add(s);
        return path;
    }

}