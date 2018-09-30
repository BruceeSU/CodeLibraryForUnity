using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LitonLib.Utils
{
    public static class GameObjectUtils
    {
        /// <summary>
        /// 深层遍历查找子物体
        /// </summary>
        /// <param name="self">被查找的父物体</param>
        /// <param name="childName">要查找的名字</param>
        /// <returns>查找到的子物体</returns>
        public static Transform FindChild(Transform self, string childName)
        {
            Transform result = null;
            if (self.childCount == 0) return null;
            foreach (Transform child in self)
            {
                if (child.name.Equals(childName))
                {
                    result = child;
                    break;
                }
                if (child.childCount > 0)
                {
                    result = FindChild(child, childName);
                    if (result != null) break;
                }
            }
            return result;
        }


        /// <summary>
        /// 根据路径找子物体，路径是物体相对父物体的路径
        /// </summary>
        /// <param name="self"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Transform FindTransformByPath(Transform self, string path, char separator = '/')
        {
            return self.FindChildInPath(path, separator);
        }


        /// <summary>
        /// 获取指定路径下的物体
        /// </summary>
        /// <param name="path">物体在场景里的路径</param>
        /// <param name="separator">路径层级分隔符，如：object/child/text，‘/’ 就是分隔符</param>
        /// <returns></returns>
        public static GameObject FindGameObjectInPath(string path, char separator = '/')
        {
            if (path == null || path == string.Empty) return null;

            string[] names = path.Split(separator);
            GameObject rootObj = GameObject.Find(names[0]);
            if (rootObj == null) return null;
            if (names.Length == 1) return rootObj;
            string newPath = path.Replace(names[0] + separator, "");
            return rootObj.transform.FindChildInPath(newPath).gameObject;
        }


        /// <summary>
        /// 扩展Transform的FindChild方法
        /// 获取指定路径下的子物体
        /// </summary>
        /// <param name="self"></param>
        /// <param name="path">物体相对父物体的路径</param>
        /// <param name="separator">路径层级分隔符，如：object/child/text，‘/’ 就是分隔符</param>
        /// <returns></returns>
        public static Transform FindChildInPath(this Transform trans, string path, char separator = '/')
        {
            if (path == null || path == string.Empty) return null;
            string[] names = path.Split(separator);
            Transform currentTrans = trans;
            for (int i = 0; i < names.Length; ++i)
            {
                currentTrans = currentTrans.Find(names[i]);
                if (currentTrans == null) return null;
            }
            return currentTrans;
        }


        /// <summary>
        /// 获取指定名称的子物体上的组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="self">父物体</param>
        /// <param name="childName">子物体名</param>
        /// <returns></returns>
        public static T GetComponentOnChildByName<T>(GameObject self, string childName) where T : Component
        {
            Transform obj = FindChild(self.transform, childName);
            if (obj == null) return null;
            T comp = obj.GetComponent<T>();
            return comp;
        }


        /// <summary>
        /// 获取指定路径下的子物体上的组件
        /// </summary>      
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="self"></param>
        /// <param name="childPath"></param>
        /// <returns></returns>
        public static T GetComponentOnChildByPath<T>(GameObject self, string childPath) where T : Component
        {
            Transform obj = self.transform.FindChildInPath(childPath);
            if (obj == null) return null;
            T comp = obj.GetComponent<T>();
            return comp;
        }


        /// <summary>
        /// 获取指定名称的子物体上的组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="self">父物体</param>
        /// <param name="childName">子物体名</param>
        /// <returns></returns>
        public static T GetComponentOnChildByName_E<T>(this GameObject self, string childName) where T : Component
        {
            Transform obj = FindChild(self.transform, childName);
            if (obj == null) return null;
            T comp = obj.GetComponent<T>();
            return comp;
        }


        /// <summary>
        /// 获取指定路径下的子物体上的组件
        /// </summary>      
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="self"></param>
        /// <param name="childPath"></param>
        /// <returns></returns>
        public static T GetComponentOnChildByPath_E<T>(this GameObject self, string childPath) where T : Component
        {
            Transform obj = self.transform.FindChildInPath(childPath);
            if (obj == null) return null;
            T comp = obj.GetComponent<T>();
            return comp;
        }

    }
}
