using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{

    [Serializable]
    public class Achievement
    {
        public string name;
        public List<ResourceRequirement> resourceRequirements; // List of required resources
        public int experienceReward;
        [HideInInspector] public bool unlocked;
        public GameObject achievementObject; // Reference to the GameObject containing the UI Image
    }




    [Serializable]
    public class ResourceRequirement
    {
        public ResourceType resourceType;
        public int amountRequired;
    }

    public ResourceManager resourceManager; // Reference to the resource manager
    public PlayerLevelManager playerLevelManager; // Reference to the player level manager
    public List<Achievement> achievements = new List<Achievement>(); // List of achievements

    public TMP_Text achievementTextPrefab; // Prefab for displaying achievement text
    public Transform achievementParent; // Parent transform for organizing achievement texts
    public float displayDuration = 3f; // Duration to display each achievement

    private void Start()
    {
        // Subscribe to the event in ResourceManager that notifies when a resource is updated
        resourceManager.OnResourceUpdated += CheckAchievements;
    }

    private void CheckAchievements(ResourceType resourceType, int amount)
    {
        foreach (Achievement achievement in achievements)
        {
            // Check if all resource requirements are met
            bool allRequirementsMet = true;
            foreach (var requirement in achievement.resourceRequirements)
            {
                int availableAmount = resourceManager.GetResourceAmount(requirement.resourceType);
                if (availableAmount < requirement.amountRequired)
                {
                    allRequirementsMet = false;
                    break; // No need to check further requirements
                }
            }

            if (!achievement.unlocked && allRequirementsMet)
            {
                // Unlock the achievement
                UnlockAchievement(achievement);

                // Set the flag to prevent unlocking the achievement again
                achievement.unlocked = true;
            }
        }
    }

    private void UnlockAchievement(Achievement achievement)
    {
        // Notify the player level manager to gain experience
        playerLevelManager.GainExperience(achievement.experienceReward);

        // Display a message or perform any other actions related to unlocking the achievement
        Debug.Log("Achievement Unlocked: " + achievement.name);

        // Create a new text object for the achievement
        TMP_Text achievementText = Instantiate(achievementTextPrefab, achievementParent);
        
        achievementText.text = "Achievement Unlocked: " + achievement.name;

        // Check if the achievement has an associated UI Image GameObject
        if (achievement.achievementObject != null)
        {
            // Activate the entire GameObject containing the UI Image
            achievement.achievementObject.SetActive(true);
        }

        // Hide the achievement text and UI Image after a certain duration
        StartCoroutine(HideAchievementObjects(achievementText.gameObject, achievement.achievementObject));
    }


    IEnumerator HideAchievementObjects(GameObject textObject, GameObject imageObject)
    {
        yield return new WaitForSeconds(displayDuration);
        Destroy(textObject);
        Destroy(imageObject);
    }



}
