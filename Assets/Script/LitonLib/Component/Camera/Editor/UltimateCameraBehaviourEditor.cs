﻿using System.Collections;
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

        EditorGUILayout.HelpBox("Select a camera mode and set properties", MessageType.Info);
        GUILayout.Space(10);
        _target.cameraType = (UltimateCameraBehaviour.CameraType)EditorGUILayout.EnumPopup(new GUIContent("Camera Mode"), _target.cameraType, EditorStyles.toolbarButton, GUILayout.Height(20f));

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

    /// <summary>
    /// 绘制SpaceCraft模式的检视面板
    /// </summary>
    private void DrawSpaceCraftModeInspector()
    {
        _setting.foldOutSpaceCraftModePropertyPanel = EditorGUILayout.Foldout(_setting.foldOutSpaceCraftModePropertyPanel, new GUIContent("Base Property"));
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
        base.OnInspectorGUI();
    }
}


