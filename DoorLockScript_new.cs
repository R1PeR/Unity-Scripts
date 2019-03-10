using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockScript_new : MonoBehaviour {

    public GameObject key;
    public GameObject doorToUnlock;
    public int whichStateToChange = 0;
    private DoorScript_new otherScript;
    // Update is called once per frame
    void Start()
    {
        otherScript = doorToUnlock.GetComponent<DoorScript_new>();
    }
    void OnCollisionEnter(Collision collision)
    {    
        if (collision.gameObject.name == key.name)
        {
            otherScript.ChangeDoorState(whichStateToChange, true);
            gameObject.SetActive(false);
        }
    }
}
