using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace States
{
    public class StateMachine
    {
        private readonly Dictionary<Type, StateController> _states = new ();

        private StateController currentStateController;
    
        public StateMachine(IEnumerable<StateController> states)
        {
            foreach (StateController state in states)
            {
                _states.Add(state.GetType(), state);
                state.SetMachine(this);
            }
        }

        public async UniTask ChangeState<T>() where T : StateController
        {
            if(currentStateController != null) 
                await currentStateController.ExitState();
        
            var state = _states[typeof(T)];
            currentStateController = state;
            await state.EnterState();
        }
    }
}
