using Cysharp.Threading.Tasks;
using UI;
using UI.Popups;
using UI.Screens;

namespace States.GameStates
{
    public class MainScreenStateController : StateController
    {
        private readonly UIPrefabFactory _uiPrefabFactory;
        
        private MainScreen _screen;
        
        public MainScreenStateController(UIPrefabFactory uiPrefabFactory)
        {
            _uiPrefabFactory = uiPrefabFactory;
        }
        
        public override async UniTask EnterState()
        {
            _screen = await _uiPrefabFactory.CreateScreen<MainScreen>();
            _screen.OnSettingsPressed += () => _uiPrefabFactory.CreatePopup<SettingsPopup>();
            _screen.OnProfilePressed += () => _uiPrefabFactory.CreatePopup<ProfilePopup>();
            _screen.OnQuizPressed += async () => await StateMachine.ChangeState<GuessGameScreenStateController>();
        }
        
        public override async UniTask ExitState()
        {
            await _uiPrefabFactory.HideScreen<MainScreen>();
        }   
    }
}