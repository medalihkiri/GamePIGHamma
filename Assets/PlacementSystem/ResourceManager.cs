using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;


public enum ResourceType
{
    Gold,
    AnimalStock,
    Vegetables,
    Water,
    Homelessness,
    Electricity,
    Coal,
    Oil,
    Education,
    Security,
    Health,
    InfrastructureQuality,
    EnvironmentalSustainability,

    Unemployment,

    PeopleFood, // New resource: Seeds for planting crops
    AnimalFood, // New resource: Animal feed for livestock
    Nutrients, // New resource: Nutrients for hydroponic farming
    ConstructionMaterials, // New resource: Construction materials for building projects
    Fertilizers, // New resource: Fertilizers for farming projects

    //Tools, // New resource: Tools for maintaining farms and gardens
    // Compost, // New resource: Compost for organic farming
    ChemicalFertilizers, // New resource: Chemical fertilizers for intensive farming
    Pesticides, // New resource: Pesticides for pest control in farming
    Trees // New resource: Trees for agroforestry projects
}



[System.Serializable]
public class ResourceAllocationData
{
    public ResourceType resourceType;
    public TMP_Text text; // Reference to the associated TMP_Text component
    public int allocation;
}

public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    private ObjectsDatabaseSO objectsDatabase;

    public static ResourceManager Instance;

    public List<ResourceAllocationData> resourceAllocations = new List<ResourceAllocationData>();

    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    // Define event for resource update
    public event Action<ResourceType, int> OnResourceUpdated;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializeResources();
    }

    private void Start()
    {
        // Update UI initially
        UpdateResourceUI();
    }

    private void InitializeResources()
    {
        foreach (var allocation in resourceAllocations)
        {
            resources[allocation.resourceType] = allocation.allocation;
        }
    }

    public int GetResourceAmount(ResourceType type)
    {
        if (resources.ContainsKey(type))
            return resources[type];
        else
            return 0;
    }

    public void SetResourceAmount(ResourceType type, int amount)
    {
        resources[type] = amount;
        UpdateResourceUI(); // Update UI when resource amount changes

        // Notify subscribers about resource update
        OnResourceUpdated?.Invoke(type, amount);
    }

    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;
        UpdateResourceUI(); // Update UI when resource amount changes

        // Notify subscribers about resource update
        OnResourceUpdated?.Invoke(type, resources[type]);
    }

    public void SubtractResource(ResourceType type, int amount)
    {
        resources[type] -= amount;
        if (resources[type] < 0)
            resources[type] = 0;
        UpdateResourceUI(); // Update UI when resource amount changes

        // Notify subscribers about resource update
        OnResourceUpdated?.Invoke(type, resources[type]);
    }

    public void UpdateResourceUI()
    {
        foreach (var allocation in resourceAllocations)
        {
            if (allocation.text != null)
            {
                allocation.text.text = $"{allocation.resourceType}: {resources[allocation.resourceType]}";
            }
        }
    }
}
#if UNITY_EDITOR // => Ignore from here to next endif if not in editor
[CustomEditor(typeof(ResourceManager))]
public class ResourceManagerEditor : Editor
{
    private SerializedProperty resourceAllocations;

    private void OnEnable()
    {
        resourceAllocations = serializedObject.FindProperty("resourceAllocations");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(resourceAllocations, true);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif