using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SerializableCollections;
/* Jak używać:
 * Dodaj na obiekt, który jest w każdej scenie, testowane na kamerze
 * Save() - Automatycznie zapisuje każdy przedmiot z tagiem tagToSave
 * Load() - Automatycznie ładuje przedmioty zapisane w jsonie
 * 
 * Jak zrobić żeby działało:
 * 1. Dodaj ten skrypt na kamere, albo jakiś obiekt, który będzie w każdej scenie
 * 2. Aby skrzynka, plecak i sakwa działały dodaj na w/w skrypt StorageScript.
 * 3. Aby można było dodawać do plecaka/skrzynki/sakwy dodaj objekt z skryptem PutInColliderScript.
 * 
 * Przed zapisaniem użyć BuildGameobjectDatabase() (w inspektorze masz przycisk) by zbudować baze danych prefabów do ładowania, inaczej wszystko usunie i dupa :/
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
    [SerializeField] private ItemList chest;
    [SerializeField] private GameObjectWithPath gameObjectPath = new GameObjectWithPath();
    public List<string> tagsToSave;
    public Transform transformControllerLeft;
    public Transform transformControllerRight;
    public Transform transformWaistLeft;
    public Transform transformWaistBack;
    public Transform transformWaistRight;
    public Transform transformLegLeft;
    public Transform transformLegRight;
    public Transform transformPouch;
    public Transform transformBackpack;
    public string tagChest;
    public bool saveOnlyOnGround = false;
    private List<GameObject> inScene;
    private List<GameObject> tempScene;
    private StorageScript ps;
    private StorageScript ch;
    private GameObject chestGameObject;
    private readonly string FILETYPE = ".json";
    private readonly string FILE_NAME_PLAYER = "playeritems";
    private readonly string FILE_NAME_GROUND = "levelitems";
    private readonly string FILE_NAME_POUCH = "pouchitems";
    private readonly string FILE_NAME_BACKPACK = "backpackitems";
    private readonly string FILE_NAME_CHEST = "chestitems";
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
            try
            {
                gameObjectPath.Add(g.name, path.Replace("Assets/Resources/", "").Replace(".prefab", ""));
            }
            catch (System.ArgumentException e)
            {
                Debug.Log(g.name +"| |"+ e.Message);
            }
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
        if((transformPouch == null || transformBackpack == null) && !saveOnlyOnGround)
        {
            throw new System.NullReferenceException("Weź no przypisz te transformy :'(");
        }
#endif
#if UNITY_EDITOR
        if (tagsToSave == null)
        {
            throw new System.NullReferenceException("Weź no przypisz tag do zapisywania :'(");
        }
#endif
        if (!saveOnlyOnGround)
        {
            ps = transformPouch.GetComponent<StorageScript>();
            ps.ClearNotParent();
        }
        chestGameObject = GameObject.FindGameObjectWithTag(tagChest);
        if (chestGameObject != null)
        {
            ch = chestGameObject.transform.GetComponent<StorageScript>();
            ch.ClearNotParent();
        }
        //sw.Reset();
        //sw.Start();
        idOfItem = 0;
        GetItems();
        if (!saveOnlyOnGround)
        {
            AddItems(player.controllerLeft, transformControllerLeft);
            AddItems(player.controllerRight, transformControllerRight);
            AddItems(player.waistRight, transformWaistRight);
            AddItems(player.waistBack, transformWaistBack);
            AddItems(player.waistLeft, transformWaistLeft);
            AddItems(player.legRight, transformLegRight);
            AddItems(player.legLeft, transformLegLeft);
            AddItems(pouch.items, transformPouch);
            AddItems(backpack.items, transformBackpack.transform);
            if (chestGameObject.transform != null)
            {
                AddItems(chest.items, chestGameObject.transform);
            }
        }
        AddItems(ground.items);
        if (!saveOnlyOnGround)
        {
            WriteToFile(JsonUtility.ToJson(pouch), FILE_NAME_POUCH + FILETYPE);
            WriteToFile(JsonUtility.ToJson(backpack), FILE_NAME_BACKPACK + FILETYPE);
            WriteToFile(JsonUtility.ToJson(player), FILE_NAME_PLAYER + FILETYPE);
        }
        if(chestGameObject.transform != null)
        {
            WriteToFile(JsonUtility.ToJson(chest), FILE_NAME_CHEST + FILETYPE);
        }
        WriteToFile(JsonUtility.ToJson(ground), FILE_NAME_GROUND + "_" +SceneManager.GetActiveScene() + FILETYPE);
        //sw.Stop();
        //textOutput.text = "Save time: " + sw.Elapsed.Milliseconds + "ms";
    }
    public void Load()
    {
        //sw.Reset();
        //sw.Start();
        GetItems();
        if (!saveOnlyOnGround)
        {
            player = JsonUtility.FromJson<Player>(ReadFromFile(FILE_NAME_PLAYER + FILETYPE));
            pouch = JsonUtility.FromJson<ItemList>(ReadFromFile(FILE_NAME_POUCH + FILETYPE));
            backpack = JsonUtility.FromJson<ItemList>(ReadFromFile(FILE_NAME_BACKPACK + FILETYPE));
            chest = JsonUtility.FromJson<ItemList>(ReadFromFile(FILE_NAME_CHEST + FILETYPE));
        }
        ground = JsonUtility.FromJson<ItemList>(ReadFromFile(FILE_NAME_GROUND + "_" + SceneManager.GetActiveScene()+FILETYPE));
        RespawnItems();
        //sw.Stop();
        //textOutput.text = "Load time: " + sw.Elapsed.Milliseconds + "ms";
    }
    /// <summary>
    /// Respawnuje jeden konkretny item i, funkcja po prostu by zastąpić powtarzanie kodu
    /// </summary>
    /// <param name="i"></param>
    /// <returns>Item i który zostanie zrespawnowany</returns>
    bool RespawnItem(Item i)
    {
        if (i.pathToPrefab != null)
        {
            if (i.parent == "null")
            {
                //Debug.Log(i.pathToPrefab.Replace("Assets/DungeonCrawler/assets/resources/", ""));
                return Instantiate(Resources.Load<GameObject>(i.pathToPrefab.Replace("Assets/DungeonCrawler/assets/resources/", "").Replace(".prefab", "")), i.position, i.rotation);
            }
            else
            {
                return Instantiate(Resources.Load<GameObject>(i.pathToPrefab.Replace("Assets/DungeonClawler/assets/resources/", "").Replace(".prefab", "")), i.position, i.rotation, GameObject.Find(i.parent).transform);
            }

        }
        else
        {
            //Debug.Log("Nie można załadować: " + i.name);
            return false;
        }
    }
    /// <summary>
    /// Respawnuje wszystkie itemy z iterowanych arrayów
    /// </summary>
    void RespawnItems() {
        tempScene = new List<GameObject>();
        foreach (string s in tagsToSave)
        {
            tempScene.AddRange(GameObject.FindGameObjectsWithTag(s));
        }
        foreach (GameObject g in tempScene)
        {
            try {
                DestroyImmediate(g);
            }
            catch
            {
                Debug.Log("Nie można usunąć obiektu, może już nie istnieć");
            }        
        }
        foreach (Item i in ground.items)
        {
            RespawnItem(i);
        }
        
        if (!saveOnlyOnGround)
        {
            //
            foreach (Item i in player.controllerLeft)
            {
                RespawnItem(i);
            }
            foreach (Item i in player.controllerRight)
            {
                RespawnItem(i);
            }
            foreach (Item i in player.legLeft)
            {
                RespawnItem(i);
            }
            foreach (Item i in player.legRight)
            {
                RespawnItem(i);
            }
            foreach (Item i in player.waistBack)
            {
                RespawnItem(i);
            }
            foreach (Item i in player.waistLeft)
            {
                RespawnItem(i);
            }
            foreach (Item i in player.waistRight)
            {
                RespawnItem(i);
            }
            //
            ps = transformPouch.GetComponent<StorageScript>();
            ps.BakePouch();
            ps.AddItems(pouch.items);
            foreach (Item i in backpack.items)
            {
                RespawnItem(i);
            }
            chestGameObject = GameObject.FindGameObjectWithTag(tagChest);
            if (chestGameObject != null)
            {
                ch = chestGameObject.transform.GetComponent<StorageScript>();
                ch.BakePouch();
                ch.AddItems(chest.items);
            }
        }
    }
    public void AddHealthAndMana(float health, float mana) {
        player.health = health;
        player.mana = mana;
    }
    void GetItems() {
        inScene = new List<GameObject>();
        foreach (string s in tagsToSave)
        {
            inScene.AddRange(GameObject.FindGameObjectsWithTag(s));
        }
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
public class Item
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