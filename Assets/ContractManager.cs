using UnityEngine;
using UnityEngine.UI;

public class ContractsManager : MonoBehaviour
{
    [System.Serializable]
    public struct Contract
    {
        public int requiredLevel;
        public Button contractButton;
    }

    [SerializeField] private Contract[] contracts;

    private PlayerLevelManager playerLevelManager;

    private void Start()
    {
        playerLevelManager = FindObjectOfType<PlayerLevelManager>();
        if (playerLevelManager == null)
        {
            Debug.LogError("PlayerLevelManager not found in the scene.");
            return;
        }

        // Subscribe to level up event
        playerLevelManager.OnLevelUp += CheckContracts;

        // Initially check contracts based on player's level
        CheckContracts(playerLevelManager.CurrentLevel);
    }

    private void CheckContracts(int level)
    {
        foreach (var contract in contracts)
        {
            if (level >= contract.requiredLevel)
            {
                contract.contractButton.gameObject.SetActive(true);
            }
            else
            {
                contract.contractButton.gameObject.SetActive(false);
            }
        }
    }
}
