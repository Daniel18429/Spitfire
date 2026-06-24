using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBuilder<T>
{
    private StateMachine<T> stateMachine;
    private UltraState<T> ultraState;
    private T info;
    public StateMachineBuilder(StateMachine<T> sM, T info)
    {
        stateMachine = sM;
        this.info = info;
    }

    public void BuildTree(StateNode<T> root)
    {
        CreateStateFromNode(root, null );
        
    }

    public void CreateStateFromNode(StateNode<T> n, State<T> parent)
    {
        State<T> currentState;
        if(parent != null)  currentState = (State<T>)Activator.CreateInstance(n.nodeType,stateMachine,info,parent);
        else
        { 
            currentState = (State<T>)Activator.CreateInstance(n.nodeType,stateMachine,info);
        }
        foreach (StateNode<T> childNode in n.children)
        {
            CreateStateFromNode(childNode, currentState);
        }
    }
}


public class StateNode<T>
{
    public Type nodeType { get; private set; }
    public List<StateNode<T>> children { get; private set; }= new List<StateNode<T>>();

    public StateNode(Type nodeType, params StateNode<T>[] children)
    {
        this.nodeType = nodeType; 
        this.children = new List<StateNode<T>>(children);
    }
}