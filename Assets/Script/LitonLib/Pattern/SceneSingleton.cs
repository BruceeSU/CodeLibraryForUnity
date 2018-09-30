using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LitonLib.Pattern
{
    /// <summary>
    /// 场景单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
    {
        protected static T _instance;
        public static T Instance
        {
            get { return _instance; }
        }

        protected virtual void Awake ()
        {
            if(_instance != null)
            {
                Debug.LogErrorFormat("{0} 有多个实例在场景中", typeof(T).Name);
                DestroyImmediate(gameObject);
                return;
            }
            _instance = (T)this;
        }
    } 
}
