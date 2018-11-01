using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using SerializableCollections;

/* Jak używać:
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
    [SerializeField] private Ground ground;
    [SerializeField] private Storage storage;
    [SerializeField] private GameObjectWithPath gameObjectPath = new GameObjectWithPath();
    public string tagToSave;
    public Transform transformPlayer;
    public Transform transformStorage;
    private GameObject[] inScene;  
    private readonly string FILE_NAME_PLAYER = "playeritems.json";
    private readonly string FILE_NAME_GROUND = "levelitems.json";
    private readonly string FILE_NAME_STORAGE = "storageitems.json";
    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
    private int idOfItem = 0;
    private float time;
    //public Text textOutput;
    private string text;

#region Editor
#if UNITY_EDITOR
    public void BuildGameobjectDatabase()
    {
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
        //sw.Reset();
        //sw.Start();
        idOfItem = 0;
        GetItems();
        AddItems(player.items,transformPlayer);
        AddItems(ground.items);
        AddItems(storage.items, transformPlayer);
        WriteToFile(JsonUtility.ToJson(player), FILE_NAME_PLAYER);
        WriteToFile(JsonUtility.ToJson(ground), FILE_NAME_GROUND);
        WriteToFile(JsonUtility.ToJson(storage), FILE_NAME_STORAGE);
        //sw.Stop();
        //textOutput.text = "Save time: " + sw.Elapsed.Milliseconds + "ms";
    }
    public void Load()
    {
        //sw.Reset();
        //sw.Start();
        GetItems();
        player = JsonUtility.FromJson<Player>(ReadFromFile(FILE_NAME_PLAYER));
        ground = JsonUtility.FromJson<Ground>(ReadFromFile(FILE_NAME_GROUND));
        storage = JsonUtility.FromJson<Storage>(ReadFromFile(FILE_NAME_STORAGE));
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
        foreach (Item i in storage.items)
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
    [SerializeField] public List<Item> items = new List<Item>();
}
[System.Serializable]
class Ground
{
    [SerializeField] public List<Item> items = new List<Item>();
}
[System.Serializable]
class Storage
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