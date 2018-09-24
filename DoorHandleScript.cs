using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandleScript : MonoBehaviour {
    public GameObject hand;
    public GameObject doorToOpen;
    public bool workOnce = false;
    private bool used = false;
    private DoorScript otherScript;
    // Update is called once per frame
    void Start()
    {
        otherScript = doorToOpen.GetComponent<DoorScript>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == hand.name && !used)
        {
            otherScript.ChangeDoorState();
            if (workOnce)
            {
                used = true;
            }
        }
    }
}
