using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RigInspector : EditorWindow
{
    private class Consts
    {
        public const string title = "Rig Shower";
    }
    private Transform _currentSelectedMesh;
    private bool _showRigTree;
    private bool _lockSelected;
    //    private Editor drawHelper;
    [MenuItem("LitonTool/RigInspector/Open Editor")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow<RigInspector>(true, Consts.title, true).Show();
    }


    void OnEnable()
    {
        _showRigTree = false;
        _lockSelected = false;

        //  drawHelper = Editor.CreateEditor(_currentSelectedMesh);
    }
    void Update()
    {
        if (!_lockSelected)
        {
            if (Selection.gameObjects.Length > 0) _currentSelectedMesh = Selection.gameObjects[0].transform;
            else _currentSelectedMesh = null;
        }
        if (_currentSelectedMesh != null && _showRigTree)
        {
            DrawRigGizmo(_currentSelectedMesh);
        }

    }

    void OnGUI()
    {
        OnDrawToolBar();

        EditorGUILayout.ObjectField("根节点", _currentSelectedMesh, typeof(Transform));
        if (GUILayout.Button("AddComponent"))
        {
            System.Type windowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.AddComponentWindow");
            EditorWindow window = EditorWindow.GetWindow(windowType);
            System.Reflection.FieldInfo fieldInfo = windowType.GetField("m_GameObjects", System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(window, new GameObject[] { _currentSelectedMesh.gameObject });
            window.Show();
            /*
            System.Reflection.MethodInfo medthod = windowType.GetMethod("Show", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            Rect showPos = new Rect(this.position.x,this.position.y +50f,this.position.width,this.position.height - 50f);
            medthod.Invoke(window,new object[] { showPos, new GameObject[] { _currentSelectedMesh.gameObject } });
            */
        }
    }


    void OnDrawToolBar()
    {
        GUILayout.Space(2f);
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        float usedWidth = 0f;
        _lockSelected = GUILayout.Toggle(_lockSelected, "锁定选择", EditorStyles.toolbarButton, GUILayout.Width(100f)); usedWidth += 100f;
        _showRigTree = GUILayout.Toggle(_showRigTree, "显示骨骼", EditorStyles.toolbarButton, GUILayout.Width(100f)); usedWidth += 100f;
        GUILayout.Space(position.width - usedWidth);
        GUILayout.EndHorizontal();
    }

    void DrawRigGizmo(Transform rig)
    {
        //Handles.DrawWireCube(Vector3.zero, Vector3.one);
        //Gizmos.DrawCube(rig.position, Vector3.one);
        //    Handles.DrawWireCube(rig.position, Vector3.one);
        // return;
        if (rig.childCount > 0)
        {
            for (int i = 0, count = rig.childCount; i < count; ++i)
            {
                Transform child = rig.GetChild(i);
                HandlesUtils.DrawPyramid(rig.position, child.position, 1f);
                DrawRigGizmo(child);
            }
        }

    }

}
/*
[CustomEditor(typeof(Transform))]
public class TransformEditorExtension : Editor
{
    private class Consts
    {
        public const string EditorPrefsName_ShowRig = "ShowRig";
    }
    private Transform _target;
    private static bool _autoShowRigTree = true;

    [MenuItem("LitonTool/RigInspector/Disable Auto Show")]
    static void DisableAutoShowRigTree()
    {
        _autoShowRigTree = false;
        SaveToEditorPrefs();
    }
    [MenuItem("LitonTool/RigInspector/Disable Auto Show", true)]
    static bool DisableRig()
    {
        return _autoShowRigTree;
    }
    [MenuItem("LitonTool/RigInspector/Enable Auto Show", false)]
    static void EnableAutoShowRigTree()
    {
        _autoShowRigTree = true;
        SaveToEditorPrefs();
    }
    [MenuItem("LitonTool/RigInspector/Enable Auto Show", true)]
    static bool EnableRig()
    {
        return !_autoShowRigTree;
    }
    static void SaveToEditorPrefs()
    {
        EditorPrefs.SetBool("ShowRig", _autoShowRigTree);
    }

    void OnEnable()
    {
        _target = (Transform)target;
        if (EditorPrefs.HasKey(Consts.EditorPrefsName_ShowRig))
            _autoShowRigTree = EditorPrefs.GetBool(Consts.EditorPrefsName_ShowRig);
    }

    void OnSceneGUI()
    {
        DrawRigGizmo(_target);
    }

    void DrawRigGizmo(Transform rig)
    {
        if (!_autoShowRigTree) return;
        if (rig.childCount > 0)
        {
            for (int i = 0; i < rig.childCount; ++i)
            {
                Transform child = rig.GetChild(i);
                HandlesUtils.DrawPyramid(rig.position, child.position, 0.02f);
                //  HandlesUtils.DrawSphere(rig, 0.01f);
                DrawRigGizmo(child);
            }
        }

    }
}
*/