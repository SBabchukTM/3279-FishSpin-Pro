using Cysharp.Threading.Tasks;
using UI;
using UI.Screens;

namespace States.GameStates
{
    public class GuessGameScreenStateController : StateController
    {
        private readonly UIPrefabFactory _uiPrefabFactory;
        
        private GuessGameScreen _screen;
        
        public GuessGameScreenStateController(UIPrefabFactory uiPrefabFactory)
        {
            _uiPrefabFactory = uiPrefabFactory;
        }
        
        public override async UniTask EnterState()
        {
            _screen = await _uiPrefabFactory.CreateScreen<GuessGameScreen>();
            _screen.OnBackPressed += async () => await StateMachine.ChangeState<MainScreenStateController>();
        }
        
        public override async UniTask ExitState()
        {
            await _uiPrefabFactory.HideScreen<GuessGameScreen>();
        }   
    }
}