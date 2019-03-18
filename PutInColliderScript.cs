using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Jak używać:
 * Dodaj na obiekt, który będzie reprezentował sakwę(jakiś plane czy coś takiego)
 * IsOneOfTag() - Sprawdza czy podany tag jest jednym z listy tagow, jesli tak zwrata true, jesli nie false
 * Na triggerze sprawdza IsOneOfTag() i jeśli tak dodaje taki sam objekt do sakwy przy jednoczesnym niszczeniu aktualnego.
 */
public class PutInColliderScript : MonoBehaviour {
    StorageScript ps;
    public List<string> tagsToSave;
    private void OnTriggerEnter(Collider other)
    {
        if (IsOneOfTag(other.gameObject.tag, tagsToSave))
        {
            if(other.GetComponentInParent<StorageScript>())
            {
                return;
            }

            if(other.transform.parent != null)
            {
                return;
            }
            ps = transform.GetComponent<StorageScript>();
            ps.AddItem(other.gameObject);
            Destroy(other.gameObject);
        }
    }
    bool IsOneOfTag(string tag, List<string> comparable)
    {
        foreach (string s in comparable)
        {
            if (tag == s) return true;
        }
        return false;
    }
}
