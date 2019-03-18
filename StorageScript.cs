using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/* Jak używać:
 * Dodaj na obiekt, który będzie reprezentował sakwę(jakiś plane czy coś takiego)
 * BakePouch() - Zeruje i generuje nową tablice Gameobjectów "places" o wymiarach rwos i columns
 * AddItem(GameObject item) - Sprawdza czy jest wolne miejsce, jak znajdzie to dodaje item przekazany w argumencie
 * ClearNotParent() - Przelatuje przez tablice i sprawdza czy któryś parent jest inny od pouch, jak tak zeruje dane pole
 * AddItems(List<Item> listOfItems) - Dodaje całą liste przedmiotów, jeżeli większa niż tablica to przestaje dodawać
 * IsEmptyPlace() - Sprawdza i zwraca pierwsze wolne miejsce w tablicy, gdy nie ma wolnych wtedy null; Format int[] = {x,y};
 * DebugPrint() - Drukuje całą tablice przedmiotów, [] - puste pole [x] - pełne pole
 */


#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(StorageScript))]
public class PouchScriptsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        StorageScript myTarget = (StorageScript)target;
        if (GUILayout.Button("BakePouch"))
        {
            myTarget.BakePouch();
        }
        if (GUILayout.Button("AddItem DEBUG"))
        {
            myTarget.AddItem(myTarget.prefabToTest);
        }
        if (GUILayout.Button("Debug Array"))
        {
            myTarget.DebugPrint();
        }
    }
}
#endif
#endregion

public class StorageScript : MonoBehaviour {
    public GameObject[,] places;
    public int rows, columns;
    public float distanceBetween;
    public GameObject prefabToTest;
    public void BakePouch(){
        if (places != null)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if ((places[i, j] != null))
                    {
                        DestroyImmediate(places[i, j]);
                    }
                }
            }
        }
        places = new GameObject[rows, columns];
    }
    public void AddItem(GameObject item)
    {  
        ClearNotParent();
        int[] pos;
        if ((pos = IsEmptyPlace()) != null)
        {
            places[pos[0],pos[1]] = Instantiate(item, transform);
            places[pos[0],pos[1]].transform.parent = this.transform;
            places[pos[0],pos[1]].transform.localPosition = new Vector3((pos[0] * distanceBetween) - (((rows - 1) * distanceBetween) / 2.0f), 0, (pos[1] * distanceBetween) - (((columns - 1) * distanceBetween) / 2.0f));
        }
        else
        {
            Debug.Log("No space");
        }
    }
    public void AddItems(List<Item> listOfItems)
    {
        BakePouch();
        int k = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                places[i,j] = null;
                if (k < listOfItems.Count && listOfItems[k].pathToPrefab != null)
                {
                    places[i,j] = Instantiate(Resources.Load<GameObject>(listOfItems[k].pathToPrefab.Replace("Assets/DungeonCrawler/assets/resources/", "").Replace(".prefab", "")), listOfItems[k].position, listOfItems[k].rotation, this.transform);               
                    places[i,j].transform.parent = this.transform;
                    places[i,j].transform.localPosition = new Vector3((i * distanceBetween) - (((rows - 1) * distanceBetween) / 2.0f), 0, (j * distanceBetween) - (((columns - 1) * distanceBetween) / 2.0f));                
                }
                else
                {
                    Debug.Log("Nie można załadować obiektu");
                }
                k++;
            }
        }
    }
    public void ClearNotParent()
    {
        if (places != null)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if((places[i,j] != null) && (places[i,j].transform.parent != transform))
                    {
                        places[i, j] = null;
                    }
                }
            }
        }
    }
    int[] IsEmptyPlace()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (places[i,j] == null)
                {
                    return new int[]{i , j};
                }
            }
        }
        return null;
    }
    public void DebugPrint()
    {
        ClearNotParent();
        if (places != null)
        {           
            string s = "";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (places[i,j] == null)
                    {
                        s += "[]";
                    }
                    else
                    {
                        s += "[x]";
                    }
                }
                s += "\n";
            }
            Debug.Log(s);
        }      
    }
}
