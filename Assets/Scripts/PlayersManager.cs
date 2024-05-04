using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Dali.Core.Singletons;

public class PlayersManager : Dali.Core.Singletons.Singleton<PlayersManager>
{

    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();
    // Start is called before the first frame update
    public int PlayersInGame
    {
        get
        {
            return playersInGame.Value;
        }

    }
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                
                playersInGame.Value++;
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                playersInGame.Value--;
            }
        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
