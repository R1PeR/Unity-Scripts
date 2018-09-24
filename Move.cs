using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    public float moveX = 0;
    public float moveY = 0;
    public float moveZ = 0;
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(transform.position.x + (moveX * Time.deltaTime), transform.position.y + (moveY * Time.deltaTime), transform.position.z + (moveZ * Time.deltaTime));
    }
}
