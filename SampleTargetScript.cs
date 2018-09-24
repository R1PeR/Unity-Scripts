using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleTargetScript : MonoBehaviour {
    public GameObject target;
    public float speed = 10f;
    float DestroyTime = 3.0f;
    // Use this for initialization
    void Start () {
		
	}
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }
        if (target.transform.position == gameObject.transform.position)
        {
            Invoke("DestroyThis", DestroyTime);
        }
    }
}
