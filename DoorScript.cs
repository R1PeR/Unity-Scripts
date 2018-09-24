using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
    public bool isLocked;
    public int howManyHandles = 1;
    int n = 0;
    private bool isOpen = false;
    private BoxCollider doorCollider;
    private BoxCollider handleCollider;
    private Animator doorAnimator;
    // Use this for initialization
    // Update is called once per frame
    void Start() {
        doorCollider = transform.Find("Door").GetComponent<BoxCollider>();
        try
        {
            handleCollider = transform.Find("Door").Find("DoorLock").GetComponent<BoxCollider>();
        }catch(NullReferenceException e){
            Debug.Log("Handle collider not found, they won't be disabled on door open");
        }
        doorAnimator = GetComponent<Animator>();
    }
    public IEnumerator OpenDoor() {
        if (!doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor") && !isLocked)
        {
            doorAnimator.Play("OpenDoor");
            ChangeCollidersEnabled(false);
            yield return new WaitForSeconds(doorAnimator.GetCurrentAnimatorStateInfo(0).length);
            ChangeCollidersEnabled(true);
            isOpen = true;
        }       
    }
    public IEnumerator CloseDoor() {
        if (!doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("CloseDoor"))
        {
            doorAnimator.Play("CloseDoor");
            ChangeCollidersEnabled(false);
            yield return new WaitForSeconds(doorAnimator.GetCurrentAnimatorStateInfo(0).length);
            ChangeCollidersEnabled(true);
            isOpen = false;
        }
    }
    public void ChangeDoorState() {
        n++;
        if (isOpen && (n >= howManyHandles))
        {
            StartCoroutine(CloseDoor());
        }
        else if(!isOpen && (n >= howManyHandles))
        {
            StartCoroutine(OpenDoor());
        }
    }
    //Used by door lock script to unlock doors
    public void ChangeLocked(bool isLocked)
    {
        this.isLocked = isLocked;
    }
    public void ChangeCollidersEnabled(bool isEnabled) {
        doorCollider.enabled = isEnabled;
        if (handleCollider != null)
        {
            handleCollider.enabled = isEnabled;
        }
    }
}
