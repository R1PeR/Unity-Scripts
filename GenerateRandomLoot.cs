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
    public List<GameObject> generated;
    public int amountToGenerate;
    public void Start()
    {
        lootPresets = lootPresetTransform.GetComponent<LootPresets>();
    }
    public void Loot()
    {
        generated = GenerateLoot(amountToGenerate);
    }
    public List<GameObject> GenerateLoot(int amount)
    {
        List<GameObject> list = new List<GameObject>();
        for(int i = 0; i < amount; i++)
        {
            list.Add(GenerateItem(listIndex));
        }
        return list;
    }
    public GameObject GenerateItem(int index)
    {
        ItemPreset list = lootPresets.listItemPresets[index];
        int range = 0;
        foreach(SingleItemPreset preset in list.itemPresets)
        {
            range += preset.percentage;
        }
        int rand = Random.Range(0, 100);
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