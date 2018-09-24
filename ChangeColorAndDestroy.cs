using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorAndDestroy : MonoBehaviour {
    LineRenderer lr;
    float time;
    // Use this for initialization
    void Start () {
        lr = GetComponent<LineRenderer>();
    }
	// Update is called once per frame
	void Update () {
        ChangeLinesColor(new Color(0, 0, 0, 0), time+=(Time.deltaTime/2));
	}
    void ChangeLinesColor(Color color, float time)
    {
        lr.startColor = Color.Lerp(lr.startColor, color, time);
        lr.endColor = Color.Lerp(lr.endColor, color, time);
        if(lr.startColor == color)
        {
            Destroy(gameObject);
        }
    }
}
