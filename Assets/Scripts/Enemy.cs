using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HealthSystem))]
public class Enemy : NetworkBehaviour
{
    [SerializeField] float attackRange = 2;
    [SerializeField] int damage = 3;
    public EnemySpawner enemySpawner;
    private NavMeshAgent agent;
    private Transform player;

    /*void Start()
    {

    }*/
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
            return;
        }
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisconnected;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    private void ClientDisconnected(ulong u) 
    {
        player = null;
    }
    
    private void Update()
    {
        foreach (GameObject obj in enemySpawner.players)
        {
            if (player == null || 
                Vector3.Distance(obj.transform.position, transform.position) < Vector3.Distance(player.position, transform.position))
            {
                player = obj.transform;
            }
        }
        
        Move();
    }
    private void OnEnable()
    {
          GetComponent<HealthSystem>().OnDied += OnDied;
    }
    private void OnDisable()
    {
         GetComponent<HealthSystem>().OnDied -= OnDied;
    }

    private void OnDied()
    {
        enemySpawner.enemies.Remove(gameObject);
        Destroy(gameObject);
    }



    private bool canAttack = true;
    private void Move()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) > attackRange) // Changed to Vector3 and added null check for player
        {
            agent.destination = player.position;
        }
        else if (canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;

        // player.GetComponent<HealthSystem>().OnDamageDealt(damage);

        yield return new WaitForSeconds(2);
        canAttack = true;
    }
}
