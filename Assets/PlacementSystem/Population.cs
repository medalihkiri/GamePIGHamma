using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    //public GameObject carPrefab;
    //public GameObject treePrefab;
    //private bool carSpawned = false;
   // private bool isPlaced = false;
    //private bool costPaid = false;
    private bool generating = true;
    private bool consuming = true;

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
   // [SerializeField] private bool affectedByPandemic = true;

    [Header("Resource Consumption")]
    [SerializeField] private List<ResourceConsumption> resourceConsumptions = new List<ResourceConsumption>();

    [Header("Building Cost")]
    [SerializeField] private List<ResourceConsumption> cost = new List<ResourceConsumption>();

    [Header("Collision Tags")]
  //  [SerializeField] private string roadTag = "Road";

    private ResourceManager resourceManager;
    private TimeController timeController;
    private int lastUpdatedDay = -1;

    void Start()
    {
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
    }

    private void Update()
    {
        if (true)
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


           
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(roadTag))
        {
            Debug.Log("Building collided with road.");
            generating = true;
            consuming = true;
        }
    }*/

   /* private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(roadTag))
        {
            Debug.Log("Building stopped colliding with road.");
            generating = false;
            consuming = false;
        }
    }*/

    // Mark the building as placed and handle tree destruction
/*public void MarkAsPlaced()
    {
        isPlaced = true;
        SpawnCar();
        DestroyNearbyTrees(); // Destroy trees near the building
    }*/

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
   /* private void SpawnCar()
    {
        Instantiate(carPrefab, transform.position, Quaternion.identity);
        carSpawned = true;
    }*/

    // Deduct building cost from resources
   /* private void DeductCost()
    {
        foreach (ResourceConsumption resourceCost in cost)
        {
            resourceManager.SubtractResource(resourceCost.resourceType, resourceCost.amount);
            Debug.Log("Building cost deducted: " + resourceCost.amount + " " + resourceCost.resourceType.ToString());
            costPaid = true;
        }
    }*/

    // Destroy trees near the building upon placement
  /*  private void DestroyNearbyTrees()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f); // Adjust the radius as needed
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("tree"))
            {
                Destroy(col.gameObject); // Destroy the tree object
            }
        }
    }*/
}
