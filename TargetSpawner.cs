using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class TargetSpawner : MonoBehaviour {
    float maxValueX,maxValueY;
    bool ready = true;
    Vector3 desirePos;
    [SerializeField]
    GameObject objectToSpawn;
    int spawned = 0;
    // Use this for initialization
    void Start () {
        maxValueX = gameObject.transform.localScale.x/2;
        maxValueY = gameObject.transform.localScale.y/2;
    }
	
	// Update is called once per frame
	void Update () {
        if(ready && (spawned<30)) StartCoroutine(SpawnTarget(1.0f));
	}
    IEnumerator SpawnTarget(float waitTime) {
        ready = false;
        desirePos = gameObject.transform.localPosition;
        desirePos.x += Random.Range(-maxValueX, maxValueX);
        desirePos.y += Random.Range(-maxValueY, maxValueY);
        desirePos.z -= 0.3f;
        spawned++;
        Instantiate(objectToSpawn,desirePos , new Quaternion(0, 0, 0, 0), gameObject.transform.parent);
        yield return new WaitForSeconds(waitTime);
        ready = true;
    }
    public void Restart() {
        spawned = 0;
    }
}
