using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class State<T>
{
    public readonly State<T> Parent;
    public State<T> ActiveChild { get; private set; }
    protected StateMachine<T> Machine { get; private set; }
    protected T _info;
    
    protected State(StateMachine<T> machine, T info, State<T> parent = null)
    {
        Parent = parent;
        Machine = machine;
        Machine.AddState(this);
        _info = info;
    }
    
    protected virtual void OnEnter(){}
    protected virtual void OnExit(){}
    protected virtual void OnUpdate(float deltaTime){}
    protected virtual void OnFixedUpdate(float deltaTime){}
    protected virtual State<T> GetInitialState() => null;
    public virtual void Transition()
    {
    }

    public void Enter()
    {
        if(Parent != null) Parent.ActiveChild = this;
        OnEnter();
    }

    public void RecursiveEnter()
    {
        Enter();
        State<T> s = GetInitialState();
        if (s != null)
        {
            s.RecursiveEnter();
        }
    }

    public void Exit()
    {
        if(Parent != null) Parent.ActiveChild = null;
        OnExit();
    }

    public void RecursiveExit() // Recursively exit CHILDREN. Only needs to be called once on the Active Child of the LCA
    {
        if(ActiveChild != null) ActiveChild.RecursiveExit();
        Exit();
    }

    protected void MachineTransition<TState>() where TState : State<T>
    {
        Machine.TransitionToType<TState>();
    }
    

    public void Update(float deltaTime)
    {
        OnUpdate(deltaTime);
        ActiveChild?.Update(deltaTime);
        Transition();
    }

    public void FixedUpdate(float deltaTime)
    {
        OnFixedUpdate(deltaTime);
        ActiveChild?.FixedUpdate(deltaTime);
        Transition();
    }
    

    public State<T> Leaf()
    {
        State<T> s = this;
        while(s.ActiveChild != null) s = s.ActiveChild;
        return s;
    }
    
    public List<State<T>> PathToRoot()
    {
        List<State<T>> path = new List<State<T>>();
        for (State<T> s= this; s != null; s = s.Parent) path.Add(s);
        return path;
    }

}