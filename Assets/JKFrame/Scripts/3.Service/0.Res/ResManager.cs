using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JKFrame
{
    public static class ResManager
    {
        // 需要缓存的类型
        private static Dictionary<Type, bool> wantCacheDic;

        static ResManager()
        {
            wantCacheDic = GameRoot.Instance.GameSetting.cacheDic;
        }

        /// <summary>
        /// 检查一个类型是否需要缓存
        /// </summary>
        private static bool CheckCacheDic(Type type)
        {
            return wantCacheDic.ContainsKey(type);
        }

        /// <summary>
        /// 加载Unity资源 如AudioClip Spirte
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// 获取实例-普通Class
        /// 如果类型需要缓存，会从对象池中获取
        /// </summary>
        public static T Load<T>() where T : class, new()
        {
            // 需要缓存
            if (CheckCacheDic(typeof(T)))
            {
                return PoolManager.Instance.GetObject<T>();
            }
            else
            {
                return new T();
            }
        }

        /// <summary>
        /// 获取实例--组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T Load<T>(string path, Transform parent = null) where T : Component
        {
            if (CheckCacheDic(typeof(T)))
            {
                return PoolManager.Instance.GetGameObject<T>(GetPrefab(path), parent);
            }
            else
            {
                return InstantiateForPrefab(path).GetComponent<T>();
            }
        }

        /// <summary>
        /// 异步加载游戏物体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callBack"></param>
        /// <param name="parent"></param>

        public static void LoadGameObjectAsync<T>(string path, Action<T> callBack = null, Transform parent = null) where T : UnityEngine.Object
        {
            // 对象池里面有
            if (CheckCacheDic(typeof(T)))
            {
                GameObject go = PoolManager.Instance.CheckCacheAndLoadGameObject(path, parent);
                // 对象有
                if (go != null)
                {
                    callBack?.Invoke(go.GetComponent<T>());
                }
                // 对象池没有
                else
                {
                    MonoManager.Instance.StartCoroutine(DoLoadGameObjectAsync<T>(path, callBack, parent));
                }
            }
            // 对象池没有
            else
            {
                MonoManager.Instance.StartCoroutine(DoLoadGameObjectAsync<T>(path, callBack, parent));
            }


        }
        static IEnumerator DoLoadGameObjectAsync<T>(string path, Action<T> callBack = null, Transform parent = null) where T : UnityEngine.Object
        {
            ResourceRequest request = Resources.LoadAsync<GameObject>(path);
            yield return request;
            GameObject go = InstantiateForPrefab(request.asset as GameObject, parent);
            callBack?.Invoke(go.GetComponent<T>());
        }

        /// <summary>
        /// 异步加载Unity资源 AudioClip Sprite GameObject(预制体)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callBack"></param>
        public static void LoadAssetAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
        {
            MonoManager.Instance.StartCoroutine(DoLoadAssetAsync<T>(path, callBack));
        }

        static IEnumerator DoLoadAssetAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            yield return request;
            callBack?.Invoke(request.asset as T);
        }


        /// <summary>
        /// 获取预制体
        /// </summary>
        public static GameObject GetPrefab(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                throw new Exception("JK:预制体路径有误，没有找到预制体");
            }
        }
        /// <summary>
        /// 基于预制体实例化
        /// </summary>
        public static GameObject InstantiateForPrefab(string path, Transform parent = null)
        {
            return InstantiateForPrefab(GetPrefab(path), parent);
        }
        /// <summary>
        /// 基于预制体实例化
        /// </summary>
        public static GameObject InstantiateForPrefab(GameObject prefab, Transform parent = null)
        {
            GameObject go = GameObject.Instantiate<GameObject>(prefab, parent);
            go.name = prefab.name;
            return go;
        }
    }
}