using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockScript : MonoBehaviour {

    public GameObject key;
    public GameObject doorToUnlock;
    private DoorScript otherScript;
    // Update is called once per frame
    void Start()
    {
        otherScript = doorToUnlock.GetComponent<DoorScript>();
    }
    void OnCollisionEnter(Collision collision)
    {    
        if (collision.gameObject.name == key.name)
        {
            otherScript.ChangeLocked(false);
            gameObject.SetActive(false);
        }
    }
}
