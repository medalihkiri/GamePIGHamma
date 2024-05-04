using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;
}

[Serializable]
public class ObjectData
{
    [SerializeField] private string name;
    [SerializeField] private int id;
    [SerializeField] private Vector2Int size = Vector2Int.one;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int goldCost;
    [SerializeField] private Dictionary<ResourceType, int> resourceEffects;

    public string Name { get { return name; } }
    public int ID { get { return id; } }
    public Vector2Int Size { get { return size; } }
    public GameObject Prefab { get { return prefab; } }
    public int GoldCost { get { return goldCost; } }
    public Dictionary<ResourceType, int> ResourceEffects { get { return resourceEffects; } }

   /* public ObjectData(string name, int id, GameObject prefab, int goldCost, Dictionary<ResourceType, int> resourceEffects)
    {
        this.name = name;
        this.id = id;
        this.prefab = prefab;
        this.goldCost = goldCost;
        this.resourceEffects = resourceEffects;
    }*/
}

