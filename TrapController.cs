using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour {
public GameObject trap;
public string animationName;
public float timeToWait = 0.5f;
public bool OnButton = false;
float timer = 0;
bool timerReached = true;
// Use this for initialization
// Update is called once per frame
void Start(){
	if (!OnButton) {
		timerReached = false;
	}
}
void Update () {
	if (!timerReached && !trap.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName (animationName)) {
		timer += Time.deltaTime;
	}
	if (timer > timeToWait)
	{
		timer = 0;
		trap.GetComponent<Animator> ().Play (animationName);
		if (OnButton) {
			timerReached = true;
		}
	}
}
void OnTriggerEnter(){
	if(OnButton){
		timer = 0;
		timerReached = false;
		Debug.Log("Collided");
	}
}
}
