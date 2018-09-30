using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitonLib.CustomComponent.Cameras;

[CustomEditor(typeof(UltimateCameraBehaviour))]
public class UltimateCameraBehaviourEditor : Editor
{

    private class ShowingSetting
    {
        public bool foldOutSpaceCraftModePropertyPanel;
    }
    private UltimateCameraBehaviour _target;
    private static ShowingSetting _setting;
    private void OnEnable()
    {
        _target = target as UltimateCameraBehaviour;
        if (_setting == null) _setting = new ShowingSetting();
    }



    public override void OnInspectorGUI()
    {
        
        //EditorGUILayout.HelpBox("Select a camera mode and set properties", MessageType.Info);
        GUILayout.Space(10);
        DrawModeSelectBar();

        switch (_target.cameraType)
        {
            case UltimateCameraBehaviour.CameraType.SpaceCraft:
                DrawSpaceCraftModeInspector();
                break;
            case UltimateCameraBehaviour.CameraType.PlaneFirstPerson:
                DrawPlaneFirstPersonModeInspector();
                break;
            case UltimateCameraBehaviour.CameraType.PlaneThirdPerson:
                DrawPlaneThirdPersonModeInspector();
                break;
            case UltimateCameraBehaviour.CameraType.SphereFirstPerson:
                DrawSphereFirstPersonModeInspector();
                break;
            case UltimateCameraBehaviour.CameraType.SphereThirdPerson:
                DrawSphereThirdPersonModeInspector();
                break;
            default:
                break;

        }
        base.Repaint();
    }
    private static void DrawModeSelectBar()
    {
        //_setting.foldOutSpaceCraftModePropertyPanel = EditorGUILayout.Foldout(_setting.foldOutSpaceCraftModePropertyPanel, new GUIContent("Base Property"));
    }

    /// <summary>
    /// 绘制SpaceCraft模式的检视面板
    /// </summary>
    private void DrawSpaceCraftModeInspector()
    {
        DrawModeSelectBar();
        if (_setting.foldOutSpaceCraftModePropertyPanel)
        {
            _target.spaceCraftModeData.moveSpeed = EditorGUILayout.FloatField(new GUIContent("Move Speed"), _target.spaceCraftModeData.moveSpeed);
            _target.spaceCraftModeData.rotateSpeed = EditorGUILayout.FloatField(new GUIContent("Rotate Speed"), _target.spaceCraftModeData.rotateSpeed);
        }

    }



    private void DrawPlaneFirstPersonModeInspector()
    { }

    private void DrawPlaneThirdPersonModeInspector()
    { }

    private void DrawSphereFirstPersonModeInspector()
    { }

    private void DrawSphereThirdPersonModeInspector()
    {
        SphereThirdPersonModeData data = _target.sphere3rdMode;
        GUILayout.Space(20);
        data.ignoreTimeScale = GUILayout.Toggle( data.ignoreTimeScale, new GUIContent("Ignore Time Scale"), EditorStyles.toolbarButton);
        GUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        data.zoomMinDistance = EditorGUILayout.FloatField("Min", data.zoomMinDistance);
        data.zoomMaxDistance = EditorGUILayout.FloatField("Max", data.zoomMaxDistance);
        EditorGUILayout.EndHorizontal();
        data.zoomDistance = EditorGUILayout.Slider("Zoom Dis", data.zoomDistance, data.zoomMinDistance, data.zoomMaxDistance);
        data.zoomSpeed = EditorGUILayout.FloatField("Zoom Speed", data.zoomSpeed);

        GUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        data.minAngle = EditorGUILayout.FloatField("MinAngle", data.minAngle);
        data.maxAngle = EditorGUILayout.FloatField("MaxAngle", data.maxAngle);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20);

        data.moveSpeed = EditorGUILayout.FloatField("Move Speed", data.moveSpeed);
        data.rotateSpeed = EditorGUILayout.FloatField("Rotate Speed", data.rotateSpeed);


        _target.sphere3rdMode = data;
    }
}


