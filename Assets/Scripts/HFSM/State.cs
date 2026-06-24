using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using HSM;

namespace HSM
{
    public abstract class State
    {
        public readonly StateMachine Machine;

        public readonly State Parent;

        public State ActiveChild;

        public State(StateMachine machine, State parent)
        {
            Machine = machine;
            Parent = parent;
        }

        protected virtual State GetInitialState() => null; // Null = leaf
        protected virtual State GetTransition() => null; // Null = staying in current state
        
        
        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }

        protected virtual void OnUpdate(float deltaTime) { }

        protected virtual void OnFixedUpdate(float fixedDeltaTime) { }
        

        internal void Enter()
        {
            if(Parent != null) Parent.ActiveChild = this;
            OnEnter();
            State init = GetInitialState();
            if(init != null) init.Enter();
        }

        internal void Exit()
        {
            if(ActiveChild != null) ActiveChild.Exit();
            ActiveChild = null;
            OnExit();
            
        }

        internal void Update(float deltaTime)
        {
            State t = GetTransition();
            if (t != null)
            {
                Machine.Sequencer.RequestTransition(this, t);
                return;
            }
            if(ActiveChild != null) ActiveChild.Update(deltaTime);
            OnUpdate(deltaTime);
        }

        internal void FixedUpdate(float fixedDeltaTime)
        {
            State t = GetTransition();
            if (t != null)
            {
                Machine.Sequencer.RequestTransition(this, t);
                return;
            }
            if(ActiveChild != null) ActiveChild.FixedUpdate(fixedDeltaTime);
            OnFixedUpdate(fixedDeltaTime);
        }

        public State Leaf()
        {
            State s = this;
            while(s != null) s = s.ActiveChild;
            return s;
        }

        public IEnumerable<State> PathToRoot()
        {
            for (State s= this; s != null; s = s.Parent) yield return s;
        }
    }
}