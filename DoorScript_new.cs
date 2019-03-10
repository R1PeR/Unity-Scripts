using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript_new : MonoBehaviour {
    public bool[] locks;
    private bool isOpen = false;
    private BoxCollider doorCollider;
    private BoxCollider handleCollider;
    private Animator doorAnimator;
    // Use this for initialization
    // Update is called once per frame
    void Start() {
        doorCollider = this.transform.Find("Door").GetComponent<BoxCollider>();
        try
        {
            handleCollider = this.transform.Find("DoorLock").GetComponent<BoxCollider>();
        }catch(NullReferenceException e){
            Debug.Log("Handle collider not found, they won't be disabled on door open" + "-" + e);
        }
        doorAnimator = GetComponent<Animator>();
        for(int i = 0; i<locks.Length; i++)
        {
            locks[i] = false;
        }
    }
    public IEnumerator OpenDoor() {
        if (!doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor"))
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
    public void ChangeDoorState(int lockIndex, bool state) {
        locks[lockIndex] = state;
        if (AreAllTrue(locks) && isOpen == false)
        {
            StartCoroutine(OpenDoor());
        }
        if (!AreAllTrue(locks) && isOpen == true)
        {
            StartCoroutine(CloseDoor());
        }
    }
    public void ChangeCollidersEnabled(bool isEnabled) {
        doorCollider.enabled = isEnabled;
        if (handleCollider != null)
        {
            handleCollider.enabled = isEnabled;
        }
    }
    public static bool AreAllTrue(bool[] array)
    {
        foreach (bool b in array) if (!b) return false;
        return true;
    }
}
