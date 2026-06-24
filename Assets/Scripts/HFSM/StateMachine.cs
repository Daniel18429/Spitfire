using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM
{
    public class StateMachine
    {
        public readonly State Root;

        public readonly TransitionSequencer Sequencer;
        private bool started;

        public StateMachine(State root)
        {
            Sequencer = new TransitionSequencer(this);
            root = Root;
        }

        public void Start()
        {
            if (started) return;
            started = true;
            Root.Enter();
        }

        public void Update(float deltaTime)
        {
            if(!started) Start();
            InternalUpdate(deltaTime);
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            if(!started) Start();
            InternalFixedUpdate(fixedDeltaTime);
        }
        internal void InternalUpdate(float deltaTime) => Root.Update(deltaTime);
        internal void InternalFixedUpdate(float deltaTime) => Root.FixedUpdate(deltaTime);
        public void ChangeState(State from, State to)
        {
            if (from == to || from == null || to == null) return;

            State lca = TransitionSequencer.Lca(from, to);

            for (State s = from; s != lca; s = s.Parent) s.Exit();
            
            var stack = new Stack<State>();
            for(State s = to; s != lca; s = s.Parent) stack.Push(s);
            while(stack.Count > 0) stack.Pop().Enter();
        }
    }
}