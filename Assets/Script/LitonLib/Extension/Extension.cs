using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Liton.Unity.Extension
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class EnumCommentAttribute : System.Attribute
    {
        private string _commont;
        public string Commont
        {
            get { return _commont; }
        }

        public EnumCommentAttribute(string commont)
        {
            _commont = commont;
            if (_commont == null || _commont == string.Empty) _commont = "Has No Commont";
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
    /// 对UnityEngine.UI空间下的UI类的扩展
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