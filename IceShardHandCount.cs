using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShardHandCount : MonoBehaviour {
    public GameObject toSpawn;
    public GameObject spawnPositionGameObject;
    Vector3 spawnPosition1;
    Vector3 spawnPosition2;
    Vector3 spawnPosition3;
    GameObject[] spawned;
    public float offset;
    int maxCharges = 3;

    void Start() {
        spawned = new GameObject[3];
        spawnPosition1 = new Vector3(0, offset, 0);
        spawnPosition2 = new Vector3(0, 2 * offset, 0);
        spawned[0] = Instantiate<GameObject>(toSpawn, Vector3.zero, new Quaternion(0, 0, 0, 0), spawnPositionGameObject.transform);
        spawned[1] = Instantiate<GameObject>(toSpawn, spawnPosition1, new Quaternion(0, 0, 0, 0), spawnPositionGameObject.transform);
        spawned[2] = Instantiate<GameObject>(toSpawn, spawnPosition2, new Quaternion(0, 0, 0, 0), spawnPositionGameObject.transform);
    }

    bool DestroyShard() {
        if (maxCharges > 0)
        {
            Destroy(spawned[maxCharges - 1]);
            maxCharges--;
            return true;
        }
        else
        {
            return false;
        }
    }
}
