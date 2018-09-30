using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGUI : Behaviour
{
    [SerializeField]
    Texture tex;
	void Start ()
    {
        
	}

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(10f, 10f, 200f, 200f),tex);
        GUI.Button(new Rect(20f, 10f, 150, 10f), "Click");
    }
}
