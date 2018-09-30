using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LitonLib.Pattern
{
    /// <summary>
    /// 整个游戏里的单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameSingleton<T> : MonoBehaviour where T : GameSingleton<T>
    {
        protected static T _instance;
        public static T Instance
        {
            get { return _instance; }
        }

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Debug.LogErrorFormat("{0} 有多个实例在场景中", typeof(T).Name);
                DestroyImmediate(gameObject);
                return;
            }
            _instance = (T)this;
            DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }
    }
}
