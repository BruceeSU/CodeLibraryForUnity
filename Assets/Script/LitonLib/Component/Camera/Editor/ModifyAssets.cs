using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ModifyMethod
{
    Replace,
    Find,
}

public struct RepalceStruce
{
    public string FindText;
    public string ReplaceTo;
    /// <summary>
    /// 替换结果
    /// </summary>
    public bool ReplaceSucced;
}
public class ModifyAssets :  EditorWindow
{

    ModifyMethod _method;
    RepalceStruce _repalceInfo = new RepalceStruce();

    [MenuItem("LitonTool/ 批量修改资源")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow<ModifyAssets>(true, "批量修改资源");
    }

    void OnGUI()
    {
        _method = (ModifyMethod)EditorGUILayout.EnumPopup(new GUIContent("功能"),_method, new GUILayoutOption[0]);
        if (_method == ModifyMethod.Replace)
        {
            DrawReplaceWindow();
        }
        else if (_method == ModifyMethod.Find)
        {
            DrawFindWindow();
        }
    }

    /// <summary>
    /// 显示替换窗口
    /// </summary>
    void DrawReplaceWindow()
    {
        EditorGUILayout.LabelField(new GUIContent(string.Format("选择了{0}个资源",Selection.objects.Length)));
        GUILayout.Space(20f);
        _repalceInfo.FindText = EditorGUILayout.TextField(new GUIContent("要替换的字符串： "),_repalceInfo.FindText, new GUILayoutOption[0]);
        _repalceInfo.ReplaceTo = EditorGUILayout.TextField(new GUIContent("该字符串替换为： "),_repalceInfo.ReplaceTo, new GUILayoutOption[0]);
        if (GUILayout.Button("开始替换"))
        {
            _repalceInfo.ReplaceSucced = false;
            Object[] selectedAssets = Selection.objects;
            foreach (Object asset in selectedAssets)
            {
                if (asset.name.Contains(_repalceInfo.FindText))
                {
                    string oldName = AssetDatabase.GetAssetPath(asset);                    
                    string newName = asset.name.Replace(_repalceInfo.FindText, _repalceInfo.ReplaceTo); 
                    AssetDatabase.RenameAsset(oldName, newName);
                }
                else
                {
                    Debug.LogErrorFormat("{0}名字不包含该字符串",asset.name);
                }
            }
            AssetDatabase.Refresh();
        }

    }

    /// <summary>
    /// 显示搜索窗口
    /// </summary>
    void DrawFindWindow()
    { }

    [MenuItem("LitonTool/Clear")]
    static void ClearProgressBar()
    {
        UnityEditor.EditorUtility.ClearProgressBar();
    }
}
