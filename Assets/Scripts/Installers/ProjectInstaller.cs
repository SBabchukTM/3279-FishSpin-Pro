using AssetsProvider;
using Services;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectBootstrapController>().AsSingle();
            BindAddressables();
            BindSaveSystem();
        }

        private void BindAddressables()
        {
            Container.Bind<AssetProvider>().AsSingle();
            Container.Bind<ConfigsProvider>().AsSingle();
        }

        private void BindSaveSystem()
        {
            Container.Bind<UserDataManager>().AsSingle();
        }
    }
}