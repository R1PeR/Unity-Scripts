using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouchWaistScript : MonoBehaviour
{
    public GameObject whichHasSaveItemScript;
    public Transform parentToSaveAs;
    public List<string> tagsToSave;
    SaveItemStats script;
    // Start is called before the first frame update
    void Start()
    {
        script = whichHasSaveItemScript.transform.GetComponent<SaveItemStats>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (IsOneOfTag(other.gameObject.tag, tagsToSave))
        {
            script.AddItemToPouch(other.gameObject, parentToSaveAs);
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
