using System.Collections.Generic;
using System.Linq;
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
            return;
        }
        State<T> lca = Lca(CurrentState, nextState);
        if(lca.ActiveChild != null) lca.ActiveChild.RecursiveExit();
        List<State<T>> path = lcaToNextState(lca, nextState);
        for (int i = 1; i < path.Count - 1; i++)
        {
            path[i].Enter();
        }
        nextState.RecursiveEnter();
        CurrentState = nextState;
        List<State<T>> temp = CurrentState.Leaf().PathToRoot();
        string msg = "";
        for (int i = 0; i < temp.Count; i++)
        {
            msg += temp[i] + "->";
        }
        Debug.Log(msg);
    }

    public void AddState(State<T> state)
    {
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
        List<State<T>> path = CurrentState.PathToRoot();
        path.Reverse();
        foreach (var s in path) s.Update(deltaTime);
    }
 
    public void FixedUpdate(float fixedDeltaTime)
    {        
        List<State<T>> path = CurrentState.PathToRoot();
        path.Reverse();
        foreach (var s in path) s.FixedUpdate(fixedDeltaTime);
    }
}