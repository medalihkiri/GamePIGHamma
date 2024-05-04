using UnityEngine;
using TMPro;
using System;

public class PlayerLevelManager : MonoBehaviour
{
    [Header("Player Level")]
    [SerializeField] private int startingLevel = 1;
    [SerializeField] private int startingExperience = 0;
    [SerializeField] private int experienceToLevelUp = 100;

    private int currentLevel;
    private int experiencePoints;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text experienceText;

    // Event to be invoked when the player levels up
    public event Action<int> OnLevelUp;

    private void Start()
    {
        currentLevel = startingLevel;
        experiencePoints = startingExperience;

        UpdateUI();
    }

    public void GainExperience(int amount)
    {
        experiencePoints += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (experiencePoints >= experienceToLevelUp)
        {
            currentLevel++;
            experiencePoints -= experienceToLevelUp;
            experienceToLevelUp += 100; // Increase experience threshold for next level
            Debug.Log("Level Up! Current Level: " + currentLevel);
            OnLevelUp?.Invoke(currentLevel); // Invoke the event with the new level
            CheckLevelUp(); // Check for additional level ups if enough experience is gained
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (levelText != null)
            levelText.text = "Level: " + currentLevel;

        if (experienceText != null)
            experienceText.text = "XP: " + experiencePoints + "/" + experienceToLevelUp;
    }

    #region Properties
    public int CurrentLevel => currentLevel;
    public int ExperiencePoints => experiencePoints;
    public int ExperienceToLevelUp => experienceToLevelUp;
    #endregion
}
