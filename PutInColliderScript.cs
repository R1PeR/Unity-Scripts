using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutInColliderScript : MonoBehaviour {
    PouchScripts ps;
    public List<string> tagsToSave;
    private void OnTriggerEnter(Collider other)
    {
        if (IsOneOfTag(other.gameObject.tag, tagsToSave))
        {
            ps = transform.parent.GetComponent<PouchScripts>();
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
