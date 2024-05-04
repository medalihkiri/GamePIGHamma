using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using System;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI playerName;

    
    public void UpdateScoreUI(int newValue, int oldValue)
    {
        scoreText.text = "Score: " + oldValue ; 
    }  
    public void UpdateNameUI(string name)
    {
        playerName.text = "Player: " + name ; 
    }

    internal NetworkVariable<string> UpdateNameUI()
    {
        throw new NotImplementedException();
    }
}