using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandleScript_new : MonoBehaviour {
    public GameObject hand;
    public GameObject doorToOpen;
    private bool used = false;
    private DoorScript_new otherScript;
    public int whichStateToChange = 0;
    // Update is called once per frame
    void Start()
    {
        otherScript = doorToOpen.GetComponent<DoorScript_new>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == hand.name && !used)
        {
            otherScript.ChangeDoorState(whichStateToChange, true);
        }
    }
}
