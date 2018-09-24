using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour {
    private float _health = 100;
    private bool _isDead = false;
    public GameObject screenToActivate;
    // Use this for initialization
    // Update is called once per frame
    private void Start()
    {
        screenToActivate.SetActive(false);
    }
    void Update () {
		if(GetHealth()<=0)
        {
            SetDead(true);
        }
        if (GetDead()) {
            screenToActivate.SetActive(true);
        }
        //debug
        ApplyDamage(Time.deltaTime*10);
        //Debug.Log(GetHealth());
	}

    public float GetHealth() {
        return _health;
    }
    public void SetHealth(float health)
    {
        _health = health;
    }
    public void ApplyDamage(float damage) {
        if ((_health -= damage) < 0)
        {
            _health = 0.0f;
        }
        else {
            _health -= damage;
        }
        
    }
    public bool GetDead(){
        return _isDead;
    }
    public void SetDead(bool dead)
    {
        _isDead = dead;
    }
}
