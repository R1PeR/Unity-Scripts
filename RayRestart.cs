using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayRestart : MonoBehaviour {
    public GameObject o;
    TargetSpawner ts;
    // Use this for initialization
    void Start () {
        o.GetComponent<TargetSpawner>();
	}
	
	// Update is called once per frame
	void OnMouseClick () {
        ts.Restart();
	}
}
