using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Custom;
using Random = UnityEngine.Random;
public class GunScript : MonoBehaviour {

    [SerializeField] private Gun[] guns;
    [SerializeField] private GameObject spawnPosition;
    [SerializeField] private GameObject gunShootPosition;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private WeaponBob wb;
    [SerializeField] private GameObject bulletHoleTex;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject roundText;
    [SerializeField] private GameObject targetSpawner;
    private AnimationCurve recoilPatternX, recoilPatternY;
    private GameObject lastSpawned;
    private Text text;
    private Text text2;
    LineRenderer line;
    Camera cam;
    RaycastHit hit;
    private int shootFired = 0;
    private int rounds;
    private bool waitingToShoot;
    private float timeOnFunction, rateOfFire,damage;
    private Recoil2 r;
    float rand;
    float timeToReset;
    bool resetTime = false;
    int score = 0;

    // Use this for initialization
    void Start()
    {
        text = scoreText.GetComponent<Text>();
        text2 = roundText.GetComponent<Text>();
        cam = GetComponentInParent<Camera>();
        r = new Recoil2(cam,player);
    }
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            ChangeGun(0);
        }
        if (Input.GetKeyDown("r") && !waitingToShoot)
        {
            StartCoroutine(Reload(3.0f));
        }
        if (Input.GetMouseButton(0) && lastSpawned != null && !waitingToShoot)
        {
            StartCoroutine(ShootGun(rateOfFire));
        }
        if (!waitingToShoot)
        {
            timeOnFunction = 0.0f;
        }
        r.RunRoutine();
    }
    // Update is called once per frame
    void ChangeGun(int n)
    {
        if (lastSpawned != null)
        {
            Destroy(lastSpawned);
        }
        lastSpawned = Instantiate<GameObject>(guns[n].gunModel,spawnPosition.transform.position,new Quaternion(0,0,0,0), spawnPosition.transform);
        recoilPatternX = guns[n].recoilPatternX;
        recoilPatternY = guns[n].recoilPatternY;
        rateOfFire = guns[n].fireRate;
        damage = guns[n].damage;
        rounds = guns[n].rounds;
        shootFired = 0;
        timeOnFunction = 0;
    }
    IEnumerator ShootGun(float waitTime)
    {
        if (shootFired < rounds) {
            waitingToShoot = true;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 100));
            if (Physics.Raycast(ray, out hit)) {
                Instantiate(bulletHoleTex, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                if(hit.transform.tag == "Target")
                {
                    Destroy(hit.transform.gameObject);
                    score++;
                    text.text = ("Score:" + score);
                }
                if (hit.transform.tag == "Random")
                {
                    targetSpawner.GetComponent<TargetSpawner>().Restart();
                    score = 0;
                }
            }
            ApplyRecoil();
            wb.RestartRecoil();
            DrawLine(gunShootPosition.transform.position, cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 100)), Color.gray, Color.grey, 0.02f, 0.02f);
            yield return new WaitForSeconds(waitTime);
            waitingToShoot = false;          
        }    
    }
    IEnumerator Reload(float waitTime)
    {
        waitingToShoot = true;
        wb.Reload();
        wb.reloading = true;
        yield return new WaitForSeconds(waitTime);
        shootFired = 0;
        wb.reloading = false;
        waitingToShoot = false;
        text2.text = (rounds - shootFired) + "";
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
    void ApplyRecoil() {     
        timeOnFunction += (1.0f / rounds);
        r.NewTarget(recoilPatternX.Evaluate(timeOnFunction), recoilPatternY.Evaluate(timeOnFunction));   
        shootFired++;
        text2.text  = (rounds - shootFired) + "";
    }
}
[System.Serializable]
public class Gun {
    public GameObject gunModel;
    public float damage;
    public float fireRate;
    public int rounds;
    public AnimationCurve recoilPatternX;
    public AnimationCurve recoilPatternY;
}
