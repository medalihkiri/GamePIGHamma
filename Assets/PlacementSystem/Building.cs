using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject carPrefab;
    [SerializeField]
    public GameObject pivotCenter;
    private bool carSpawned = false;
    private bool isPlaced = false;
    private bool costPaid = false;
    private bool generating = false;
    private bool consuming = false;
    private PandemicManager pandemicManager;
    public Vector3Int positionInGrid = Vector3Int.zero;
    public bool isPandemicCritical = false;
    [System.Serializable]
    public class ResourceGeneration
    {
        public ResourceType resourceType;
        public int amount;
    }

    [System.Serializable]
    public class ResourceConsumption
    {
        public ResourceType resourceType;
        public int amount;
    }

    [Header("Resource Generation")]
    [SerializeField] private List<ResourceGeneration> resourceGenerations = new List<ResourceGeneration>();
    [SerializeField] public bool affectedByPandemic ;

    [Header("Resource Consumption")]
    [SerializeField] private List<ResourceConsumption> resourceConsumptions = new List<ResourceConsumption>();

    [Header("Building Cost")]
    [SerializeField] private List<ResourceConsumption> cost = new List<ResourceConsumption>();

    [Header("Collision Tags")]
    [SerializeField] private string roadTag = "Road";

    private ResourceManager resourceManager;
    private TimeController timeController;
    private int lastUpdatedDay = -1;

    void Start()
    {   
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        if (collider != null)
        {
            Vector3 hamma = collider.size;

            Vector3 center = new Vector3 (hamma.x/2, hamma.y/2, hamma.z/2);
            pivotCenter.gameObject.transform.localPosition = center;   
        }
        resourceManager = FindObjectOfType<ResourceManager>();
        if (resourceManager == null)
        {
            Debug.LogError("ResourceManager not found in the scene!");
        }

        timeController = FindObjectOfType<TimeController>();
        if (timeController == null)
        {
            Debug.LogError("TimeController not found in the scene!");
        }
        pandemicManager = FindObjectOfType<PandemicManager>(); // Find the PandemicManager in the scene
        if (pandemicManager == null)
        {
            Debug.LogError("PandemicManager not found in the scene!");
        }

    }

    private void Update()
    {
        if (isPlaced)
        {
            int currentDay = timeController.currentDay;

            // Check if the day has changed
            if (currentDay != lastUpdatedDay)
            {
                lastUpdatedDay = currentDay;
                if (generating)
                {
                    GenerateResources();
                }
                if (consuming)
                {
                    ConsumeResources();
                }
            }

            if (!carSpawned && carPrefab != null)
            {
                SpawnCar();
            }

            if (!costPaid)
            {
                DeductCost();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(roadTag))
        {
            Debug.Log("Building collided with road.");
            generating = true;
            consuming = true;
        }
      /* if (affectedByPandemic && pandemicManager.pandemicActive)
        {
            StopGeneratingResources();
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(roadTag))
        {
            Debug.Log("Building stopped colliding with road.");
            generating = false;
            consuming = false;

        }
    }

    // Mark the building as placed and handle tree destruction
    public void MarkAsPlaced()
    {
        isPlaced = true;
        SpawnCar();
        DestroyNearbyTrees(); // Destroy trees near the building
    }

    // Generate resources once per day
    private void GenerateResources()
    {
        Debug.Log("Generating resources...");
        foreach (ResourceGeneration generation in resourceGenerations)
        {
            resourceManager.AddResource(generation.resourceType, generation.amount);
            Debug.Log(gameObject.name + " generated " + generation.amount + " " + generation.resourceType.ToString());
        }
    }

    // Consume resources once per day
    private void ConsumeResources()
    {
        Debug.Log("Consuming resources...");
        foreach (ResourceConsumption consumption in resourceConsumptions)
        {
            resourceManager.SubtractResource(consumption.resourceType, consumption.amount);
            Debug.Log(gameObject.name + " consumed " + consumption.amount + " " + consumption.resourceType.ToString());
        }
    }

    // Spawn a car object
    private void SpawnCar()
    {
        Instantiate(carPrefab, transform.position, Quaternion.identity);
        carSpawned = true;
    }

    // Deduct building cost from resources
    private void DeductCost()
    {
        foreach (ResourceConsumption resourceCost in cost)
        {
            resourceManager.SubtractResource(resourceCost.resourceType, resourceCost.amount);
            Debug.Log("Building cost deducted: " + resourceCost.amount + " " + resourceCost.resourceType.ToString());
            costPaid = true;
        }
    }
    public void ResumeGeneratingResources()
    {
        generating = true;
        consuming = true;
    }
    public void StopGeneratingResources()
    {
        generating = false;
        consuming = false;
    }
    // Destroy trees near the building upon placement
    private void DestroyNearbyTrees()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f); // Adjust the radius as needed
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("tree"))
            {
                Destroy(col.gameObject); // Destroy the tree object
            }
        }
    }
}
