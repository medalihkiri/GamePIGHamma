using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using Unity.VisualScripting;
using TMPro;

public class PlayerHud : NetworkBehaviour
{
    
    private NetworkVariable<NetworkString> playersName = new NetworkVariable<NetworkString>();
    private bool overlaySet = false;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            playersName.Value = $"Player {OwnerClientId}";

        }

    }

    public void SetOverlay()
    {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = playersName.Value;
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if(!overlaySet && !string.IsNullOrEmpty(playersName.Value)) { 
        
            SetOverlay();
            overlaySet = true;
        }
    }
}
 
