using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegeneration : MonoBehaviour {
    public float healthToAdd = 10.0f;
    public GameObject player;
    HealthController healthController;
	// Use this for initialization
	void Start () {
        healthController = player.GetComponent<HealthController>();
	}
	
	// Update is called once per frame
	void Update () {
        healthController.AddHealth(healthToAdd * Time.deltaTime);
	}
}
