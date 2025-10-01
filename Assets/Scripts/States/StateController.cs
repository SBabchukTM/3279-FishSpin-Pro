using Cysharp.Threading.Tasks;

namespace States
{
    public class StateController
    {
        protected StateMachine StateMachine;
    
        public StateController()
        {
        
        }
    
        public void SetMachine(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
    
        public virtual async UniTask EnterState()
        {
        
        }

        public virtual async UniTask ExitState()
        {
        
        }
    }
}
