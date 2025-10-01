using AssetsProvider;
using Services;
using Zenject;

public class ProjectBootstrapController : IInitializable
{
    private readonly AssetProvider _assetProvider;
    private readonly ConfigsProvider _configsProvider;
    private readonly UserDataManager _userDataManager;
        
    public ProjectBootstrapController(
        AssetProvider assetProvider,
        ConfigsProvider configsProvider,
        UserDataManager userDataManager)
    {
        _assetProvider = assetProvider;
        _configsProvider = configsProvider;
        _userDataManager = userDataManager;
    }

    public async void Initialize()
    {
        await _assetProvider.Initialize();
        await _configsProvider.Initialize();
        _userDataManager.Load();
    }
}
