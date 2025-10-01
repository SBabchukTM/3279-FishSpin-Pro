using Cysharp.Threading.Tasks;
using Services;
using UI;
using UI.Screens;

namespace States.GameStates
{
    public class LoadingScreenStateController : StateController
    {
        private readonly UIPrefabFactory _uiPrefabFactory;
        private readonly UserDataManager _userDataManager;
        
        private LoadingScreen _screen;
        
        public LoadingScreenStateController(UIPrefabFactory uiPrefabFactory, UserDataManager userDataManager)
        {
            _uiPrefabFactory = uiPrefabFactory;
            _userDataManager = userDataManager;
        }
        
        public override async UniTask EnterState()
        {
            _screen = await _uiPrefabFactory.CreateScreen<LoadingScreen>();
            await _screen.Load();
            
            if(_userDataManager.GetData().UserFinishedTutorialData.Value)
                await StateMachine.ChangeState<MainScreenStateController>();
            else
                await StateMachine.ChangeState<OnboardingScreenStateController>();
        }
        
        public override async UniTask ExitState()
        {
            await _uiPrefabFactory.HideScreen<LoadingScreen>();
        }   
    }
}