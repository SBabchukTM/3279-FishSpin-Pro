using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace AssetsProvider
{
    public class AssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache =
            new Dictionary<string, AsyncOperationHandle>();

        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles =
            new Dictionary<string, List<AsyncOperationHandle>>();

        public async UniTask Initialize()
        {
            await Addressables.InitializeAsync();
        }

        public async UniTask<List<Object>> LoadByLabel(List<string> labels, Addressables.MergeMode mergeMode = Addressables.MergeMode.Union)
        {
            var loadedAssets = new List<Object>();

            var locations = await Addressables.LoadResourceLocationsAsync(
                labels, mergeMode);
            
            try
            {
                await UniTask.WhenAll(locations.Select(async location =>
                {
                    var asset = await Load<Object>(location.PrimaryKey);
                    loadedAssets.Add(asset);
                }));
            }
            catch
            {
                throw new Exception("Error occured while loading assets");
            }

            return loadedAssets;
        }
        
        public async UniTask<T> Load<T>(string address) where T : class
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCacheOnComplete(Addressables.LoadAssetAsync<T>(address), cacheKey: address);
        }
        
        private async UniTask<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completeHandle => { _completedCache[cacheKey] = completeHandle; };

            AddHandle<T>(cacheKey, handle);

            return await handle.Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }
    }
}