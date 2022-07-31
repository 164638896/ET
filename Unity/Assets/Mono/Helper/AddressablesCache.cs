using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace ET.Module
{
    public static class AddressablesCache
    {
        private static readonly Dictionary<string, UnityEngine.Object> prefabCache = new Dictionary<string, UnityEngine.Object>();

        public static T Instantiate<T>(string addressableKey) where T : UnityEngine.Object
        {
            var obj = LoadAsset<T>(addressableKey);
            if (obj)
            {
                return UnityEngine.Object.Instantiate(obj);
            }

            return null;
        }

        public static T LoadAsset<T>(string addressableKey) where T : UnityEngine.Object
        {
            if (!prefabCache.TryGetValue(addressableKey, out var prefab))
            {
                prefab = Addressables.LoadAssetAsync<T>(addressableKey).WaitForCompletion();
                if (prefab != null)
                {
                    prefabCache.Add(addressableKey, prefab);
                }
                else
                {
                    return null;
                }
            }

            return prefab as T;
        }
        
        public static async ETTask<T> LoadAssetAsync<T>(string addressableKey) where T : UnityEngine.Object
        {
             var tcs = ETTask<T>.Create(true);

            if (!prefabCache.TryGetValue(addressableKey, out var prefab))
            {
                Addressables.LoadAssetAsync<T>(addressableKey).Completed += handle =>
                {
                    tcs.SetResult(handle.Result);
                };
            }
            else
            {
                tcs.SetResult(prefab as T);
            }

            return await tcs;
        }
        
        public static ETTask<SceneInstance> LoadSceneAsync(string scenePath, UnityEngine.SceneManagement.LoadSceneMode loadMode = UnityEngine.SceneManagement.LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
        {
            ETTask<SceneInstance> tcs = ETTask<SceneInstance>.Create();
            var sceneInstanceHandle = Addressables.LoadSceneAsync(scenePath, loadMode, activateOnLoad, priority);
            sceneInstanceHandle.Completed += (handle) =>
            {
                SceneInstance sceneInstance = handle.Result;
                tcs.SetResult(sceneInstance);
            };
            return tcs.GetAwaiter();
        }

        // /// <summary>
        // /// 激活加载的场景
        // /// </summary>
        // public static ETTask ActivateLoadScene(SceneInstance sceneInstance)
        // {
        //     ETTask tcs = ETTask.Create();
        //     var asyncOperation = sceneInstance.ActivateAsync();
        //     asyncOperation.completed += (operation) =>
        //     {
        //         tcs.SetResult();
        //     };
        //     return tcs.GetAwaiter();
        // }
        
        /// <summary>
        /// 通过场景实例化数据卸载场景
        /// </summary>
        // public static ETTask UnLoadSceneAsync(SceneInstance sceneInstance)
        // {
        //     ETTask tcs = ETTask.Create();
        //     Addressables.UnloadSceneAsync(sceneInstance).Completed += (handle) =>
        //     {
        //         tcs.SetResult();
        //     };
        //     return tcs.GetAwaiter();
        // }
        
    }
}