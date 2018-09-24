using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour {
    public float speed = 2f;
    public Color colorOne = Color.red;
    public Color colorTwo = Color.green;
    private float time;
    private float timeColor = 0.5f*Mathf.PI;
    private Scrollbar sb;
    private RockScript script;
    // Use this for initialization
    void Start () {
        sb = gameObject.GetComponent<Scrollbar>();
        script = GameObject.Find("Rock").GetComponent<RockScript>();
    }
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime * speed;
        timeColor += Time.deltaTime * speed * 2.0f;
        if (time > 2 * Mathf.PI) { time -= 2 * Mathf.PI; }
        sb.value = Sinus1to0(time);
        SetColor(Color.Lerp(colorOne, colorTwo, Sinus1to0(timeColor)));
    }
    void SetColor(Color color)
    {
        ColorBlock cb = sb.colors;
        cb.disabledColor = color;
        sb.colors = cb;
    }
    float Sinus1to0(float x)
    {
        return (Mathf.Sin(x) + 1.0f) / 2.0f;
    }
    void OnMouseDown()
    {
        Debug.Log("Click");
        script.applyForce(0, sb.value, sb.value*5);
    }
}
