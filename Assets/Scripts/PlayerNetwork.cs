using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    public float movementSpeed = 5f;

    private Vector3 movementInput;
    public int playerScore;
    private void Start()
    {
        if (IsLocalPlayer)
        {
            enabled = true;
        }
    }

    private void Update()
    {
        if (!IsLocalPlayer) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        movementInput = new Vector3(horizontalInput, 0f, verticalInput).normalized;
    }

    private void FixedUpdate()
    {
        if (!IsLocalPlayer) return;

        Vector3 newPosition = transform.position + movementInput * movementSpeed * Time.deltaTime;

        // Request server to move the player
        RequestMoveServerRpc(newPosition);
    }

    
    [ServerRpc]
    private void RequestMoveServerRpc(Vector3 newPosition)
    {
        // Update position on all clients
        UpdatePositionClientRpc(newPosition);
    }

    [ClientRpc]
    private void UpdatePositionClientRpc(Vector3 newPosition)
    {
        // Update position on all clients
        transform.position = newPosition;
    }
}
