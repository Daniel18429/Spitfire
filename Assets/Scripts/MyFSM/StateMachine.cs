using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine<T>
{
    public State<T> CurrentState { get; private set; }
    private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

    public void Initialize(State<T> initialState)
    {
        CurrentState = initialState;
        initialState.RecursiveEnter();
    }
    
    public void TransitionToType<TState>() where TState : State<T>
    {
        Debug.Log("trANSITION");
        TransitionState(GetStateFromType<TState>());
    }

    public State<T> GetStateFromType<TState>() where TState : State<T>
    {
        if (!states.TryGetValue(typeof(TState), out State<T> nextState)) return null;
        return nextState;
    }

    public void TransitionState(State<T> nextState)
    {
        if (CurrentState == nextState || nextState == null)
        {
            Debug.Log(nextState);
            return;
        }
        State<T> lca = Lca(CurrentState, nextState);
        if(lca.ActiveChild != null) lca.ActiveChild.RecursiveExit();
        List<State<T>> path = lcaToNextState(lca, nextState);
        Debug.Log("Still going");
        for (int i = 1; i < path.Count - 1; i++)
        {
            Debug.Log("ENTERING");
            path[i].Enter();
        }
        nextState.RecursiveEnter();
    }

    public void AddState(State<T> state)
    {
        Debug.Log("Adding State");
        states.Add(state.GetType(), state);
    }

    public List<State<T>> lcaToNextState(State<T> lca, State<T> nextState)
    {
        List<State<T>> path = nextState.PathToRoot();
        while (path.Count > 0)
        {
            if (path[^1] == lca) break;
            path.RemoveAt(path.Count - 1);
        }
        path.Reverse();
        return path;
    }

    public State<T> Lca(State<T> a, State<T> b)
    {
        var aSet = new HashSet<State<T>>();
        for(var s = a; s != null; s = s.Parent) aSet.Add(s);
            
        for(var s = b; s != null; s = s.Parent)
            if (aSet.Contains(s))
                return s;
        return null;
    }

    public void Update(float deltaTime)
    {
        CurrentState.Update(deltaTime);
    }

    public void FixedUpdate(float fixedDeltaTime)
    {
        CurrentState.FixedUpdate(fixedDeltaTime);
    }
}