using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightningStrike : MonoBehaviour {
    System.Random rng = new System.Random();
    List<GameObject> enemies;
    public GameObject target;
    GameObject lastEnemy;
    public float maxDistance = 100;
    public float speed = 10f;
    public float maxBounces = 3;
    public GameObject toSpawn;
    float DestroyTime = 3.0f;
    int n = 0;
    RaycastHit hit;
    // Update is called once per frame
    void Start()
    {
        //Enemies loading
        lastEnemy = gameObject;
        enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy").ToList());
        if (target == null)
        {
            target = FindRandomEnemieInDistance(maxDistance);
        }
    }
    void Update()
    {
        if (target != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }
        if(target.transform.position == gameObject.transform.position)
        {
            Invoke("DestroyThis", DestroyTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (n < maxBounces)
        {
            try
            {
                target = FindRandomEnemieInDistance(maxDistance);
            }
            catch{
                Debug.Log("Not Found Target");
            }
        }
        else {
            Invoke("DestroyThis", DestroyTime);

        }
        Instantiate<GameObject>(toSpawn, gameObject.transform.position, new Quaternion(0, 0, 0, 0), gameObject.transform.parent);
        n++;
    }
    void DestroyThis() {
        Destroy(gameObject);
    }
    GameObject FindRandomEnemieInDistance(float distance) {
        foreach (GameObject obj in enemies) {
            if (Vector3.Distance(transform.position, obj.transform.position) < distance && obj != lastEnemy)
            {
                Physics.Raycast(transform.position, obj.transform.position - transform.position, out hit);
                if(hit.collider.gameObject == obj.gameObject)
                {
                    lastEnemy = obj;
                    Shuffle<GameObject>(enemies);
                    return obj;
                }
            }
        }
        return null;
    }
    void Shuffle<GameObject>(IList<GameObject> list) {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
