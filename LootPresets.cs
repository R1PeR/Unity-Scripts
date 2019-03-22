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
    public List<ObligatoryItemPreset> itemPresetsObligatory;
}
[System.Serializable]
public class SingleItemPreset
{
    public GameObject gameobject;
    public int percentage;
}
[System.Serializable]
public class ObligatoryItemPreset
{
    public GameObject gameobject;
}