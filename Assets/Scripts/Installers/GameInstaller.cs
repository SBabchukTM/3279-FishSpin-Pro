using Services;
using States;
using States.GameStates;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [FormerlySerializedAs("_gameUIContainer")] [SerializeField] private UIPrefabFactory _uiPrefabFactory;
        [SerializeField] private MusicPlayer _musicPlayer;
        [SerializeField] private SoundPlayer _soundPlayer;
        
        public override void InstallBindings()
        {
            Container.Bind<DIFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameBootstrapController>().AsSingle();
            Container.Bind<UIPrefabFactory>().FromComponentInNewPrefab(_uiPrefabFactory).AsSingle();
            Container.Bind<MusicPlayer>().FromComponentInNewPrefab(_musicPlayer).AsSingle();
            Container.Bind<SoundPlayer>().FromComponentInNewPrefab(_soundPlayer).AsSingle();
            BindStates();
        }

        private void BindStates()
        {
            Container.Bind<StateMachine>().AsSingle();
            Container.Bind<StateController>().To<LoadingScreenStateController>().AsSingle();
            Container.Bind<StateController>().To<GuessGameScreenStateController>().AsSingle();
            Container.Bind<StateController>().To<MainScreenStateController>().AsSingle();
            Container.Bind<StateController>().To<OnboardingScreenStateController>().AsSingle();
        }
    }
}
