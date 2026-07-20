using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBuilder<T>
{
    private StateMachine<T> stateMachine;
    private RootState<T> _rootState;
    private T info;
    public StateMachineBuilder(StateMachine<T> sM, T info)
    {
        stateMachine = sM;
        this.info = info;
    }

    // Called on list of top level states, inserts Root State automatically
    public void BuildTree(StateNode<T>[] children)
    {
        StateNode<T> rootNode = new StateNode<T>(typeof(RootState<T>), children);
        CreateStateFromNode(rootNode, null );
    }

    public void CreateStateFromNode(StateNode<T> n, State<T> parent)
    {
        State<T> currentState;
        currentState = (State<T>)Activator.CreateInstance(n.nodeType,stateMachine,info,parent);
        foreach (StateNode<T> childNode in n.children)
        {
            CreateStateFromNode(childNode, currentState);
        }
    }
}

// Used to create a tree
public class StateNode<T>
{
    public Type nodeType { get; private set; }
    public List<StateNode<T>> children { get; private set; }= new List<StateNode<T>>();

    public StateNode(Type nodeType, params StateNode<T>[] children )
    {
        this.nodeType = nodeType; 
        this.children = new List<StateNode<T>>(children);
    }
}

public static class Tree<T>
{
    public static StateNode<T> Node<TState>(params StateNode<T>[] children)
        where TState : State<T> => new(typeof(TState), children);
}