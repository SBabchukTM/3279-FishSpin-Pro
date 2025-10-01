using Cysharp.Threading.Tasks;
using Services;
using UI;
using UI.Screens;

namespace States.GameStates
{
    public class OnboardingScreenStateController : StateController
    {
        private readonly UIPrefabFactory _uiPrefabFactory;
        private readonly UserDataManager _userDataManager;
        
        private OnboardingScreen _screen;
        
        public OnboardingScreenStateController(UIPrefabFactory uiPrefabFactory, UserDataManager userDataManager)
        {
            _uiPrefabFactory = uiPrefabFactory;
            _userDataManager = userDataManager;
        }
        
        public override async UniTask EnterState()
        {
            _screen = await _uiPrefabFactory.CreateScreen<OnboardingScreen>();
            _screen.OnStartButtonPressed += SaveTutorialDone;
        }

        private async void SaveTutorialDone()
        {
            _userDataManager.GetData().UserFinishedTutorialData.Value = true;
            await StateMachine.ChangeState<MainScreenStateController>();
        }

        public override async UniTask ExitState()
        {
            await _uiPrefabFactory.HideScreen<OnboardingScreen>();
        }   
    }
}