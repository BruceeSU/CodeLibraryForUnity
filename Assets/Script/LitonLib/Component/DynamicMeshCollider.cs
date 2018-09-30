using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动态MeshCollider，随着SkinedMesh变化
/// </summary>
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class DynamicMeshCollider : MonoBehaviour
{
    private MeshCollider _collider;
    private SkinnedMeshRenderer _renderer;

	void Start ()
    {
        InitSetting();
	}

	void Update ()
    {
        Mesh bakedMesh = CreateMeshFromSkinnedMeshRenderer();
        _collider.sharedMesh = bakedMesh;
	}

    private Mesh CreateMeshFromSkinnedMeshRenderer()
    {
        Mesh mesh = new Mesh();
        _renderer.BakeMesh(mesh);
        return mesh;

    }


    private void InitSetting()
    {
        _collider = GetComponent<MeshCollider>();
        _renderer = GetComponent<SkinnedMeshRenderer>();
    }
}
