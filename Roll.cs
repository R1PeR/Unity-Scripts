using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour {
    public float rotateX = 0;
    public float rotateY = 0;
    public float rotateZ = 0;
    // Update is called once per frame
    void Update () {
        transform.Rotate(rotateX * Time.deltaTime, rotateY * Time.deltaTime, rotateZ * Time.deltaTime);
	}
}
