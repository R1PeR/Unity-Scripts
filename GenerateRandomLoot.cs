using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(GenerateRandomLoot))]
public class GenerateRandomLootEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GenerateRandomLoot myTarget = (GenerateRandomLoot)target;
        if (GUILayout.Button("Generate Loot"))
        {
            myTarget.Loot();
        }
    }
}
#endif
#endregion
public class GenerateRandomLoot : MonoBehaviour
{
    public Transform lootPresetTransform;
    private LootPresets lootPresets;
    public int listIndex;
    private ItemPreset list;
    public List<GameObject> generated;
    public int amountToGenerate;
    public bool addObligatoryItems;
    public void Start()
    {
        lootPresets = lootPresetTransform.GetComponent<LootPresets>();
        list = lootPresets.listItemPresets[listIndex];
        Loot();
    }
    public void Loot()
    {
        generated = GenerateLoot(amountToGenerate, addObligatoryItems);
        StorageScript sg = GetComponent<StorageScript>();
        sg.BakePouch();
        foreach(GameObject g in generated)
        {
            sg.AddItem(g);
        }
    }
    public List<GameObject> GenerateLoot(int amount, bool addObligatory)
    {
        List<GameObject> listReturn = new List<GameObject>();
        for(int i = 0; i < amount; i++)
        {
            listReturn.Add(GenerateItem());
        }
        if (addObligatory)
        {
            foreach (ObligatoryItemPreset preset in list.itemPresetsObligatory)
            {
                listReturn.Add(preset.gameobject);
            }
        }
        return listReturn;
    }
    public GameObject GenerateItem()
    {
        int range = 0;
        foreach(SingleItemPreset preset in list.itemPresets)
        {
            range += preset.percentage; 
        }
        int rand = Random.Range(0, range);
        int top = 0;
        foreach(SingleItemPreset preset in list.itemPresets)
        {
            top += preset.percentage;
            if(rand < top)
            {
                return preset.gameobject;
            }
        }
        return null;
    }
}