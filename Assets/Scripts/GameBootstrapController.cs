using Cysharp.Threading.Tasks;
using Services;
using States;
using States.GameStates;
using Zenject;

public class GameBootstrapController : IInitializable
{
    private readonly StateMachine _stateMachine;
    private readonly DiContainer _container;
    private readonly UserDataManager _userDataManager;
    private readonly MusicPlayer _musicPlayer;
    private readonly SoundPlayer _soundPlayer;

    public GameBootstrapController(StateMachine stateMachine, DiContainer container, 
        UserDataManager userDataManager, MusicPlayer musicPlayer, SoundPlayer soundPlayer)
    {
        _stateMachine = stateMachine;
        _container = container;
        _userDataManager = userDataManager;
        _musicPlayer = musicPlayer;
        _soundPlayer = soundPlayer;
    }
        
    public async void Initialize()
    {
        SetVolume();
        _container.InstantiateComponentOnNewGameObject<ApplicationStateListener>("ApplicationStateListener");
        await _stateMachine.ChangeState<LoadingScreenStateController>();
    }

    private async UniTaskVoid SetVolume()
    {
        do
        {
            await UniTask.NextFrame();
        } while (_userDataManager.GetData() == null);

        var data = _userDataManager.GetData().GameSettingsData;
        _musicPlayer.SetVolume(data.MusicVolume);
        _soundPlayer.SetVolume(data.SoundVolume);
    }
}