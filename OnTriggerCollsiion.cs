using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerCollsiion : MonoBehaviour {
    public GameObject toSpawn;
    public float SpawnTime = 3.0f;
    public float DestroyTime = 3.0f;
    void OnTriggerEnter() {
        Debug.Log("Hit");
        Invoke("SpawnFire", SpawnTime);
        Invoke("DestroyThis", DestroyTime);
    }
    void DestroyThis()
    {
        Destroy(gameObject);
    }
    void SpawnFire()
    {
        Instantiate<GameObject>(toSpawn, gameObject.transform.position, new Quaternion(0, 0, 0, 0), gameObject.transform.parent);
    }
}
