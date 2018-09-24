using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour
{

    public float DestroyTime = 3;

    private bool canUpdateState;
    // Use this for initialization
    void OnEnable()
    {
        canUpdateState = true;
    }

    private void Update()
    {
        if (canUpdateState)
        {
            canUpdateState = false;
            Invoke("DestroyThis", DestroyTime);
        }
    }

    // Update is called once per frame
    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
