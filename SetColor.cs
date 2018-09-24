using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour {
    public Color color = Color.white;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Renderer>().material.color = color;
    }
}
