using UnityEngine;

public class PandemicManager : MonoBehaviour
{
    public int HealthThreshold = 100;
    public int HealthRecoveryThreshold = 200;
    public int PandemicDurationDays = 7;

    private int consecutiveLowHealthDays = 0;
    private bool pandemicActive = false;
    private ResourceManager resourceManager;
    private TimeController timeController;
    private bool pandemicStarted = false;
    private int pandemicStartDay = 0;

    private void Start()
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
        CheckPandemicStatus();
    }

    private void CheckPandemicStatus()
    {
        int currentHealth = resourceManager.GetResourceAmount(ResourceType.Health);

        // Check if health is below threshold
        if (currentHealth < HealthThreshold)
        {
            if (!pandemicStarted)
            {
                pandemicStartDay = timeController.currentDay; // Record the day when pandemic starts
                pandemicStarted = true;
            }

            // Increment consecutive low health days if the pandemic is not already active
            if (!pandemicActive)
            {
                consecutiveLowHealthDays = timeController.currentDay - pandemicStartDay; // Calculate the days passed since the pandemic started

                // Check if consecutive low health days reached pandemic threshold
                if (consecutiveLowHealthDays >= PandemicDurationDays)
                {
                    StartPandemic();
                }
            }
        }
        else
        {
            // Reset consecutive low health days when health goes above threshold
            consecutiveLowHealthDays = 0;
            pandemicStarted = false;
            // Check if pandemic is active and health is recovered
            if (pandemicActive && currentHealth >= HealthRecoveryThreshold)
            {
                EndPandemic();
            }
        }
    }

    public void StartPandemic()
    {
        pandemicActive = true;
        // Implement pandemic start actions here
        // Example: Stop resource generation in buildings affected by pandemic
        Building[] buildings = FindObjectsOfType<Building>();
        foreach (Building building in buildings)
        {
            if (building.affectedByPandemic) // Check if the building is affected by the pandemic
            {
                building.StopGeneratingResources();
            }
        }
    }

    public void EndPandemic()
    {
        pandemicActive = false;
        // Implement pandemic end actions here
        // Example: Resume resource generation in buildings affected by pandemic
        Building[] buildings = FindObjectsOfType<Building>();
        foreach (Building building in buildings)
        {
            if (building.affectedByPandemic) // Check if the building is affected by the pandemic
            {
                building.ResumeGeneratingResources();
            }
        }
    }
}