using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(PouchScripts))]
public class PouchScriptsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PouchScripts myTarget = (PouchScripts)target;
        if (GUILayout.Button("BakePouch"))
        {
            myTarget.BakePouch();
        }
        if (GUILayout.Button("AddItem DEBUG"))
        {
            myTarget.AddItem(myTarget.prefabToTest);
        }
        if (GUILayout.Button("RemoveItem DEBUG"))
        {
            myTarget.RemoveItem(1,2);
        }
    }
}
#endif
#endregion


public class PouchScripts : MonoBehaviour {
    public GameObject[][] places;
    public int rows, columns;
    public float distanceBetween;
    public GameObject prefabToTest;
	// Use this for initialization
	void Start () {

	}	
	// Update is called once per frame
	void Update () {
		
	}
    public void BakePouch(){
        if(places != null)
        {
            for (int i = 0; i < places.Length; i++)
            {
                for (int j = 0; j < places[0].Length; j++)
                {
                    DestroyImmediate(places[i][j]);
                }
            }
        }
        places = new GameObject[rows][];
        for (int i = 0; i < rows; i++)
        {
            places[i] = new GameObject[columns];
        }
    }
    public void AddItem(GameObject item)
    {
        int[] pos;
        if ((pos = IsEmptyPlace()) != null)
        {
            places[pos[0]][pos[1]] = Instantiate(item, transform);
            places[pos[0]][pos[1]].transform.parent = this.transform;
            places[pos[0]][pos[1]].transform.localPosition = new Vector3((pos[0] * distanceBetween) - (((rows - 1) * distanceBetween) / 2.0f), 0, (pos[1] * distanceBetween) - (((columns - 1) * distanceBetween) / 2.0f));
        }
    }
    public void RemoveItem(int i, int j)
    {
        DestroyImmediate(places[i][j]);
        places[i][j] = null;
    }
    public void ClearNotParent()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if(places[i][j].transform.parent != transform)
                {
                    places[i][j] = null;
                }
            }
        }    
    }
    public void AddItems(List<Item> listOfItems)
    {
        int k = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                places[i][j] = null;
                if (k < listOfItems.Count && listOfItems[k].pathToPrefab != null)
                {
                    places[i][j] = Instantiate(Resources.Load<GameObject>(listOfItems[k].pathToPrefab.Replace("Assets/Resources/", "").Replace(".prefab", "")), listOfItems[k].position, listOfItems[k].rotation, this.transform);               
                    places[i][j].transform.parent = this.transform;
                    places[i][j].transform.localPosition = new Vector3((i * distanceBetween) - (((rows - 1) * distanceBetween) / 2.0f), 0, (j * distanceBetween) - (((columns - 1) * distanceBetween) / 2.0f));                
                }
                else
                {
                    Debug.Log("Nie można załadować obiektu");
                }
                k++;
            }
        }
    }
    int[] IsEmptyPlace()
    {
        for (int i = 0; i < places.Length; i++)
        {
            for (int j = 0; j < places[0].Length; j++)
            {
                if (places[i][j] == null)
                {
                    return new int[]{i , j};
                }
            }
        }
        return null;
    }
}
