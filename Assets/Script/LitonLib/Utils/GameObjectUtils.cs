using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LitonLib.Utils
{
    public class GameObjectUtils
    {
        /// <summary>
        /// 深层遍历查找子物体
        /// </summary>
        /// <param name="self">被查找的父物体</param>
        /// <param name="childName">要查找的名字</param>
        /// <returns>查找到的子物体</returns>
        public static Transform FindChild(Transform self, string childName)
        {
            Transform result= null;
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
        /// 根据路径找子物体
        /// </summary>
        /// <param name="self"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Transform FindChildByPath(Transform self,string path)
        {
            string[] names = path.Split('/');
            Transform currentTrans = self;
            foreach (string name in names)
            {
                currentTrans = FindChild(currentTrans, name);
                if (currentTrans == null)
                    return null;
            }
            return currentTrans;
        }
        public static T GetComponentInChild<T>(GameObject self, string childName) where T : Component
        {
            Transform obj = FindChild(self.transform, childName);
            if (obj == null) return null;
            T comp = obj.GetComponent<T>();
            return comp;
        }
        public static T GetComponentInChildByPath<T>(GameObject self, string childPath) where T : Component
        {
            Transform obj = FindChildByPath(self.transform, childPath);
            if (obj == null) return null;
            T comp = obj.GetComponent<T>();
            return comp;
        }

        public static GameObject FindGameObjectInPath(string path)
        {
            if (path == null || path == string.Empty) return null;

            string[] names = path.Split('/');
            GameObject rootObj = GameObject.Find(names[0]);
            if (rootObj == null) return null;
            if (names.Length == 1) return rootObj;
            string newPath = path.Replace(names[0] + "/", "");
            return FindChildByPath(rootObj.transform, newPath).gameObject;
        }
    }
}
