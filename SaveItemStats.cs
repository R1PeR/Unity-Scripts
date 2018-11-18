using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using SerializableCollections;

/* Jak używać:
 * Dodaj na obiekt, który jest w każdej scenie, testowane na kamerze
 * Save() - Automatycznie zapisuje każdy przedmiot z tagiem tagToSave
 * Load() - Automatycznie ładuje przedmioty zapisane w jsonie
 * Przed zapisaniem użyć BuildGameobjectDatabase() by zbudować baze danych prefabów do ładowania
 * AddHealthAndMana(float health, float mana) - Dodaj zycie i mana do zapisania, weź to podepnij bo mi się nie chce :)
 * Wszystko zakomentowane to mierzenie czasu zapisu i odczytu
 */
#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(SaveItemStats))]
public class SaveItemsStatsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SaveItemStats myTarget = (SaveItemStats)target;
        if (GUILayout.Button("Save Items"))
        {
            myTarget.Save();
        }
        if (GUILayout.Button("Load Items"))
        {
            myTarget.Load();
        }
        if (GUILayout.Button("Bake Database"))
        {
            myTarget.BuildGameobjectDatabase();
        }
        if (GUILayout.Button("Delete Database"))
        {
            myTarget.DeleteDatabase();
        }
    }
}

[UnityEditor.CustomPropertyDrawer(typeof(GameObjectWithPath))]
public class ExtendedSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer
{

}
#endif
#endregion
public class SaveItemStats : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ItemList ground;
    [SerializeField] private ItemList pouch;
    [SerializeField] private ItemList backpack;
    [SerializeField] private GameObjectWithPath gameObjectPath = new GameObjectWithPath();
    public string tagToSave = "Item";
    public Transform transformControllerLeft;
    public Transform transformControllerRight;
    public Transform transformWaistLeft;
    public Transform transformWaistBack;
    public Transform transformWaistRight;
    public Transform transformLegLeft;
    public Transform transformLegRight;
    public Transform transformPouch;
    public Transform transformBackpack;
    private GameObject[] inScene;  
    private readonly string FILE_NAME_PLAYER = "playeritems.json";
    private readonly string FILE_NAME_GROUND = "levelitems.json";
    private readonly string FILE_NAME_POUCH = "pouchitems.json";
    private readonly string FILE_NAME_BACKPACK = "backpackitems.json";
    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
    private int idOfItem = 0;
    private float time;
    //public Text textOutput;
    private string text;

#region Editor
#if UNITY_EDITOR
    public void BuildGameobjectDatabase()
    {
        DeleteDatabase();
        GameObject[] go = Resources.LoadAll<GameObject>("/");
        foreach (GameObject g in go)
        {
            Object parentObject = PrefabUtility.FindPrefabRoot(g);
            string path = AssetDatabase.GetAssetPath(parentObject);
            gameObjectPath.Add(g.name, path.Replace("Assets/Resources/", "").Replace(".prefab", ""));
        }
    }
    public void DeleteDatabase()
    {
        gameObjectPath.Clear();
    }
#endif
#endregion
    public void Save() {
#if UNITY_EDITOR
        if(transformWaistLeft == null || transformPouch == null || transformBackpack == null)
        {
            throw new System.NullReferenceException("Weź no przypisz te transformy :'(");
        }
#endif
#if UNITY_EDITOR
        if (tagToSave == null || tagToSave == "")
        {
            throw new System.NullReferenceException("Weź no przypisz tag do zapisywania :'(");
        }
#endif
        //sw.Reset();
        //sw.Start();
        idOfItem = 0;
        GetItems();
        AddItems(player.controllerLeft,transformControllerLeft);
        AddItems(player.controllerRight, transformControllerRight);
        AddItems(player.waistRight, transformWaistRight);
        AddItems(player.waistBack, transformWaistBack);
        AddItems(player.waistLeft, transformWaistLeft);
        AddItems(player.legRight, transformLegRight);
        AddItems(player.legLeft, transformLegLeft);
        AddItems(ground.items);
        AddItems(pouch.items, transformPouch);
        AddItems(backpack.items, transformBackpack);
        WriteToFile(JsonUtility.ToJson(player), FILE_NAME_PLAYER);
        WriteToFile(JsonUtility.ToJson(ground), FILE_NAME_GROUND);
        WriteToFile(JsonUtility.ToJson(pouch), FILE_NAME_POUCH);
        WriteToFile(JsonUtility.ToJson(backpack), FILE_NAME_BACKPACK);
        //sw.Stop();
        //textOutput.text = "Save time: " + sw.Elapsed.Milliseconds + "ms";
    }
    public void Load()
    {
        //sw.Reset();
        //sw.Start();
        GetItems();
        player = JsonUtility.FromJson<Player>(ReadFromFile(FILE_NAME_PLAYER));
        ground = JsonUtility.FromJson<ItemList>(ReadFromFile(FILE_NAME_GROUND));
        pouch = JsonUtility.FromJson<ItemList>(ReadFromFile(FILE_NAME_POUCH));
        backpack = JsonUtility.FromJson<ItemList>(ReadFromFile(FILE_NAME_BACKPACK));
        RespawnItems();
        //sw.Stop();
        //textOutput.text = "Load time: " + sw.Elapsed.Milliseconds + "ms";
    }
    void RespawnItems() {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag(tagToSave))
        {
            try {
                Destroy(g);
            }
            catch
            {
                Debug.Log("Nie można usunąć obiektu, może już nie istnieć");
            }        
        }
        foreach (Item i in ground.items)
        {
            if(i.pathToPrefab != null)
            {
                if(i.parent == "null")
                {
                    Instantiate(Resources.Load<GameObject>(i.pathToPrefab.Replace("Assets/Resources/", "").Replace(".prefab","")), i.position, i.rotation);
                }
                else
                {
                    Instantiate(Resources.Load<GameObject>(i.pathToPrefab.Replace("Assets/Resources/", "").Replace(".prefab", "")), i.position, i.rotation, GameObject.Find(i.parent).transform);
                }
            }
            else
            {
                Debug.Log("Nie można załadować: " + i.name);
            }
        }
        foreach (Item i in pouch.items)
        {
            if (i.pathToPrefab != null)
            {
                if (i.parent == "null")
                {
                    Instantiate(Resources.Load<GameObject>(i.pathToPrefab.Replace("Assets/Resources/", "").Replace(".prefab", "")), i.position, i.rotation);
                }
                else
                {
                    Instantiate(Resources.Load<GameObject>(i.pathToPrefab.Replace("Assets/Resources/", "").Replace(".prefab", "")), i.position, i.rotation, GameObject.Find(i.parent).transform);
                }
            }
            else
            {
                Debug.Log("Nie można załadować: " + i.name);
            }
        }
        foreach (Item i in backpack.items)
        {
            if (i.pathToPrefab != null)
            {
                if (i.parent == "null")
                {
                    Instantiate(Resources.Load<GameObject>(i.pathToPrefab.Replace("Assets/Resources/", "").Replace(".prefab", "")), i.position, i.rotation);
                }
                else
                {
                    Instantiate(Resources.Load<GameObject>(i.pathToPrefab.Replace("Assets/Resources/", "").Replace(".prefab", "")), i.position, i.rotation, GameObject.Find(i.parent).transform);
                }
            }
            else
            {
                Debug.Log("Nie można załadować: " + i.name);
            }
        }
    }
    public void AddHealthAndMana(float health, float mana) {
        player.health = health;
        player.mana = mana;
    }
    void GetItems() {
        inScene = GameObject.FindGameObjectsWithTag(tagToSave);
    }
    void AddItems (List<Item> list, Transform parent = null) {
#if UNITY_EDITOR
        if(gameObjectPath.Count == 0 || gameObjectPath == null)
        {
            throw new System.NullReferenceException("Weź no zbuduj tą baze danych :'(");
        }
#endif
        list.Clear();
        string path = "none";
        int index;
        foreach (GameObject g in inScene)
        {
            if (g.transform.parent == parent)
            {
                index = g.name.IndexOf("(");
                if(index > 0)
                {
                    gameObjectPath.TryGetValue(g.name.Remove(index).Replace(" ",""), out path);
                }
                else
                {
                    gameObjectPath.TryGetValue(g.name.Replace(" ", ""), out path);
                }                
                if (parent == null)
                {
                    list.Add(new Item(g.name, idOfItem++, g.transform.position, g.transform.rotation, "null", path));
                }
                else
                {
                    list.Add(new Item(g.name, idOfItem++, g.transform.position, g.transform.rotation, g.transform.parent.name, path));
                }
            }
        }
    }
    void WriteToFile(string json, string fileName)
    {
        string filename = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filename))
        {
            File.Delete(filename);
        }
        File.WriteAllText(filename, json);
    }
    string ReadFromFile(string fileName) {
        string filename = Path.Combine(Application.persistentDataPath, fileName);
        return File.ReadAllText(filename);
    }
}
[System.Serializable]
class Player
{
    [SerializeField] public float health;
    [SerializeField] public float mana;
    [SerializeField] public List<Item> controllerLeft = new List<Item>();
    [SerializeField] public List<Item> controllerRight = new List<Item>();
    [SerializeField] public List<Item> waistLeft = new List<Item>();
    [SerializeField] public List<Item> waistBack = new List<Item>();
    [SerializeField] public List<Item> waistRight = new List<Item>();
    [SerializeField] public List<Item> legRight = new List<Item>();
    [SerializeField] public List<Item> legLeft = new List<Item>();
}
[System.Serializable]
class ItemList
{
    [SerializeField] public List<Item> items = new List<Item>();
}
[System.Serializable]
class Item
{
    [SerializeField] public string name;
    [SerializeField] public int id;
    [SerializeField] public Vector3 position;
    [SerializeField] public Quaternion rotation;
    [SerializeField] public string parent;
    [SerializeField] public string pathToPrefab;
    public Item(string name, int id, Vector3 position, Quaternion rotation, string parent, string pathToPrefab)
    {
        this.name = name;
        this.id = id;
        this.position = position;
        this.rotation = rotation;
        this.parent = parent;
        this.pathToPrefab = pathToPrefab;
    }
}
[System.Serializable]
class GameObjectWithPath : SerializableDictionary<string, string>
{
}