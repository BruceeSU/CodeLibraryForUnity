using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class MassGravity : MonoBehaviour 
{

    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody
    {
        get { return _rigidbody; }
    }

    private void Awake()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null) _rigidbody = gameObject.AddComponent<Rigidbody>();
            PointGravitySystem.AddToSystem(_rigidbody);
        }
    }

    void OnDestroy()
    {
        PointGravitySystem.RemoveMassObj(_rigidbody);
    }
}
