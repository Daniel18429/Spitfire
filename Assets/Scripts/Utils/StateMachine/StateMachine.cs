using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Controller of the states
public class StateMachine<T>
{
    [SerializeField] public State<T> CurrentState { get; private set; } // The current state (current state may have active children)
    private State<T> Root { get; set; } // Empty state for controlling transitions and updates
    
    // Dictionary to easily swap to states from typing. Contains all states under state machine instance
    private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();
    
    // Called upon creation of state machine
    // BUG FIX: ENTER ALL STATES CORRECTLY
    public void Initialize(State<T> initialState)
    {
        List<State<T>> path = lcaToNextState(Root, initialState);
        for (int i = 1; i < path.Count - 1; i++)
        {
            path[i].Enter();
        }
        initialState.RecursiveEnter();
        CurrentState = initialState.Leaf();
        List<State<T>> temp = CurrentState.PathToRoot();
        string msg = "";
        for (int i = 0; i < temp.Count; i++)
        {
            msg += temp[i] + "->";
        }
        Debug.Log(msg);
    }

    public State<T> GetStateFromType<TState>() where TState : State<T> // Returns state instance from sm dictionary
    {
        if (!states.TryGetValue(typeof(TState), out State<T> nextState))
        {
            throw new System.Exception("State not found");
            return null;
        }
        return nextState;
    }
    
    // BUG FIX: TEST EXIT TRANSITIONS. IDLE EXITING TO GROUNDED WOULD CAUSE ERRORS (I THINK)
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
        if (states.Count == 0)
        {
            Root = state;
        }
        states.Add(state.GetType(), state);
    }

    // List of states with lca at 0 and next state at final position
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

    // Finds least common ancestor between two states (State is garunteed, worst case is Root)
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
        Root.Update(deltaTime);
    }
 
    public void FixedUpdate(float fixedDeltaTime)
    {
        State<T> nextState = Root.CallTransition();
        while (nextState != null && nextState != CurrentState)
        {
            TransitionState(nextState);
            nextState = Root.CallTransition();
        }        
        Root.FixedUpdate(fixedDeltaTime);
    }
}