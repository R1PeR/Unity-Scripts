using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPresets : MonoBehaviour
{
    public List<ItemPreset> listItemPresets;
}
[System.Serializable]
public class ItemPreset{
    public List<SingleItemPreset> itemPresets;
}
[System.Serializable]
public class SingleItemPreset
{
    public GameObject gameobject;
    public int percentage;
}