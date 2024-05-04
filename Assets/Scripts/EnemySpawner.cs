using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
using UnityEngine.AI;
using JetBrains.Annotations;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField] private int maxEnemyCount = 10;
    [SerializeField] private float spawnCooldown = 2;
    [SerializeField] private GameObject enemyPrefab; // Changed to GameObject
    [SerializeField] private Vector3 minSpawnPos; // Changed to Vector3
    [SerializeField] private Vector3 maxSpawnPos; // Changed to Vector3

    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> players;// Changed to GameObject

    private void Start()
    {
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
            //GetComponent<NavMeshAgent>().enabled = false;
            return;
        }

        StartCoroutine(SpawnEnemies());

        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisConnected;
    }

    public void ClientConnected(ulong u)
    {
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
    }
    public async void ClientDisConnected(ulong u)
    {
        await Task.Yield();
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (enemies.Count < maxEnemyCount)
            {
                Vector3 spawnPos = new Vector3(Random.Range(minSpawnPos.x, maxSpawnPos.x),
                                               Random.Range(minSpawnPos.y, maxSpawnPos.y),
                                               Random.Range(minSpawnPos.z, maxSpawnPos.z)); // Changed to Vector3
                GameObject enemyTransform = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform); // Changed to GameObject
                enemies.Add(enemyTransform);
                //Transform enemyTransform = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform); // Changed to GameObject
                enemyTransform.GetComponent<Enemy>().enemySpawner = this;
                enemyTransform.GetComponent<NetworkObject>().Spawn(true);
                enemies.Add(enemyTransform);

                yield return new WaitForSeconds(spawnCooldown);
            }

            yield return null;
        }
    }
}
