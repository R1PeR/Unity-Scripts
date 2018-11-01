using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Custom;
using Random = UnityEngine.Random;
public class WeaponBob : MonoBehaviour {
    [SerializeField] private Recoil recoil;
    [SerializeField] private Recoil reload;
    public bool reloading;
    // Use this for initialization
    // Update is called once per frame
    void Start()
    {
        recoil.NewTarget(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, Random.Range(-0.1f, -0.05f)));
    }
    void Update()
    {       
        if (!reloading)
        {
           recoil.RunRoutine();
        }
        else
        {
            reload.RunRoutine();
        }
        
    }
    public void RestartRecoil()
    {
        recoil.NewTarget(new Vector3(0f, 0f, 0f), new Vector3(Random.Range(-0.01f, 0f) , Random.Range(-0.01f, 0f), Random.Range(-0.1f, -0.05f)));
    }
    public void Reload()
    {
        reload.NewTarget(new Vector3(0f, 0f, 0f), new Vector3(0,-0.1f,0));
    }
}
