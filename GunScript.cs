using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour {

    [SerializeField]private GameObject[] guns;
    [SerializeField]private GameObject spawnPosition;
    [SerializeField]private GameObject gunShootPosition;
    [SerializeField]private GameObject linePrefab;
    private GameObject lastSpawned;
    LineRenderer line;
    Camera cam;
    RaycastHit hit;
    private bool waitingToShoot;
    // Use this for initialization
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            ChangeGun(0);
        }
        if (Input.GetKeyDown("2"))
        {
            ChangeGun(1);
        }
        if (Input.GetMouseButton(0) && lastSpawned != null && !waitingToShoot)
        {
            StartCoroutine(ShootGun(0.1f));
        }
    }
    // Update is called once per frame
    void ChangeGun(int n)
    {
        if (lastSpawned != null)
        {
            Destroy(lastSpawned);
        }
        lastSpawned = Instantiate<GameObject>(guns[n],spawnPosition.transform.position,new Quaternion(0,0,0,0), spawnPosition.transform);
    }
    IEnumerator ShootGun(float waitTime)
    {
        waitingToShoot = true;
        //Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        //Physics.Raycast(ray,100);
        //
        DrawLine(gunShootPosition.transform.position, cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 100)), Color.gray, Color.grey,0.02f, 0.02f);
        yield return new WaitForSeconds(waitTime);
        waitingToShoot = false;
    }
    LineRenderer DrawLine(Vector3 start, Vector3 end, Color startColor, Color endColor, float startWidth, float endWidth)
    {
        GameObject myLine = Instantiate<GameObject>(linePrefab);
        myLine.transform.position = start;
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.startWidth = startWidth;
        lr.endWidth = endWidth;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        return lr;
    }
    
}
