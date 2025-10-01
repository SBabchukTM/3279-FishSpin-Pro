using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace AssetsProvider
{
    public class ConfigsProvider
    {
        private readonly AssetProvider _assetProvider;
        
        private readonly Dictionary<Type, BaseConfig> _configsLoaded = new ();

        public ConfigsProvider(AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask Initialize()
        {
            var persistentConfigs = await _assetProvider.LoadByLabel(new List<string>{ConstLabels.ConfigLabel});

            foreach (var config in persistentConfigs) 
                Set(config);
        }

        public T Get<T>() where T : BaseConfig
        {
            if (_configsLoaded.ContainsKey(typeof(T)))
            {
                var config = _configsLoaded[typeof(T)];
                return config as T;
            }

            throw new Exception("No config found");
        }

        private void Set(Object config)
        {
            if (_configsLoaded.ContainsKey(config.GetType()))
                return;

            _configsLoaded.Add(config.GetType(), config as BaseConfig);
        }
    }
}