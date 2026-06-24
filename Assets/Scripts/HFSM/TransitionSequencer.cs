using System.Collections.Generic;

namespace HSM
{
    public class TransitionSequencer
    {
        public readonly StateMachine Machine;

        public TransitionSequencer(StateMachine machine)
        {
            Machine = machine;
        }

        public void RequestTransition(State from, State to)
        {
            Machine.ChangeState(from, to);
        }

        public static State Lca(State a, State b)
        {
            var aSet = new HashSet<State>();
            for(var s = a; s != null; s = s.Parent) aSet.Add(s);
            
            for(var s = b; s != null; s = s.Parent)
                if (aSet.Contains(s))
                    return s;
            return null;
        }
    }
}