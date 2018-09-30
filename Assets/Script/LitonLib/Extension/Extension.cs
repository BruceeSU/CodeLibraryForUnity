using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Liton.Unity.Extension
{
    /// <summary>
    /// 给枚举类型成员添加注释的特性
    /// 
    /// @秉承着代码里最好不要出现中文的习惯 ( 除了注释 )，枚举类型及其成员都使用英文字母命名
    /// @由于UI界面使用时可能要中文，于是将枚举成员的中文注释保存在特性中，写入元数据
    /// @通过反射机制来获取枚举成员的中文注释
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class EnumCommentAttribute : System.Attribute
    {
        private string _comment;

        /// <summary>
        /// 枚举成员的注释
        /// </summary>
        public string Comment
        {
            get { return _comment; }
        }

        public EnumCommentAttribute(string comment)
        {
            _comment = comment;
            if (_comment == null || _comment == string.Empty) _comment = "Has No Commont";
        }
    }




    /// <summary>
    /// 枚举扩展类
    /// </summary>

    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举类的特性标记
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="mananer">特性实例</param>
        /// <returns></returns>
        public static T GetFieldAttribute<T>(this System.Enum mananer) where T : System.Attribute
        {
            System.Attribute[] atts = mananer.GetType().GetField(mananer.ToString()).GetCustomAttributes(typeof(T), false) as System.Attribute[];
            if (atts == null || atts.Length == 0) return null;
            return atts[0] as T;
        }
    }

    /// <summary>
    /// 对UnityEngine.UI空间下的UI类的扩展，一次调用完成多个属性的设置
    /// </summary>
    public static class UIExtension
    {

        public static T Setup<T>(this UnityEngine.UI.Selectable ui, Transform parent, Vector3 pos, float scale) where T : UnityEngine.UI.Selectable
        {
            Transform trans = ui.transform;
            trans.SetParent(parent);
            trans.localPosition = pos;
            trans.localScale = Vector3.one * scale;
            return ui as T;
        }

        /// <summary>
        /// 对Button的扩展
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="content"></param>
        /// <param name="onClick"></param>
        /// <returns></returns>
        public static UnityEngine.UI.Button SetupButton(this UnityEngine.UI.Button btn, Transform parent, Vector3 pos, float scale, string content, UnityEngine.Events.UnityAction onClick, string labelName = "Text")
        {
            Transform trans = btn.transform;
            trans.SetParent(parent);
            trans.localPosition = pos;
            trans.localScale = Vector3.one * scale;
            trans.Find(labelName).GetComponent<UnityEngine.UI.Text>().text = content;
            btn.onClick.AddListener(onClick);
            return btn;
        }

        /// <summary>
        /// 扩展Toggle组件
        /// </summary>
        /// <param name="toggle"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="content"></param>
        /// <param name="onSwitch"></param>
        /// <returns></returns>
        public static UnityEngine.UI.Toggle SetupToggle(this UnityEngine.UI.Toggle toggle, Transform parent, Vector3 pos, float scale, string content, UnityEngine.Events.UnityAction<bool> onSwitch, string labelName = "Label")
        {
            Transform trans = toggle.transform;
            trans.SetParent(parent);
            trans.localPosition = pos;
            trans.localScale = Vector3.one * scale;
            trans.Find(labelName).GetComponent<UnityEngine.UI.Text>().text = content;
            toggle.onValueChanged.AddListener(onSwitch);
            return toggle;
        }

        public static UnityEngine.UI.Slider SetupSlider(this UnityEngine.UI.Slider slider, Transform parent, Vector3 pos, float scale, UnityEngine.Events.UnityAction<float> onValueChange)
        {
            Transform trans = slider.transform;
            trans.SetParent(parent);
            trans.localPosition = pos;
            trans.localScale = Vector3.one * scale;
            slider.onValueChanged.AddListener(onValueChange);
            return slider;
        }
    }
}