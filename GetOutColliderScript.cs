using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Jak używać:
 * Jeżeli łapanie kontrolerem to zmienianie parenta to powinno działać bez skryptu, dodać na itemy.
 * Sprawdza czy kolizja równa "Hand" i jak tak to zmienia parent na collider
 */
public class GetOutColliderScript : MonoBehaviour {
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Hand")
        {
            transform.parent = collision.gameObject.transform;
        }
    }
}
