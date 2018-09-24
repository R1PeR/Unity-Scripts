using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RockScript : MonoBehaviour {
    public void applyForce(float x, float y, float z) {
        GetComponent<Rigidbody>().AddForce(new Vector3(x,y,z));
    }
}
