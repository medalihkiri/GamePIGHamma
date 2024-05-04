using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

 
public class Player : NetworkBehaviour
{
    public NetworkVariable<int> playerScore = new NetworkVariable<int>();


    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
        {   
            enabled = false;
            return; 
        }
        playerScore.OnValueChanged += FindObjectOfType<PlayerUI>().UpdateScoreUI;
    }
    void Start()
    {
      
    }
    
    [ServerRpc]
    public void RequestSpawnServerRpc()
    {

    }
    [ClientRpc]
    public void RequestSpawnClientRpc()
    {
        RequestSpawnServerRpc();
    }

    private bool canAttack = true;
    
    private void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            //movementSpeedMultiplier = 0.5f;
            //animator.SetFloat("Attack", 1);
            
            if (canAttack)
            {
                AttackServerRPC();

                StartCoroutine(AttackCooldown());
            }
        }

    }
    
    [ServerRpc]
    private void AttackServerRPC()
    {
        //RaycastHit[] hits = Physics.SphereCastAll(transform.position, 5, currentMoveDirection, 0, 1 << 6);

        /*if (hits.Length > 0)
        {
            hits[0].transform.GetComponent<HealthSystem>().OnDamageDealt(50);
            if (hits[0].transform.GetComponent<HealthSystem>().health < 0)
            {
                playerScore.Value++;
                Debug.Log("touched");
            }
        }*/
        playerScore.Value++;
        Debug.Log("1");
    }
    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(1);
        canAttack = true;
    }
    // Start is called before the first frame update


}
