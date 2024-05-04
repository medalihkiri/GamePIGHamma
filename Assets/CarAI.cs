using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarController : MonoBehaviour
{
    public string targetTag = "Target"; // Tag of the target points
    private Transform[] targets; // Array to store all target points
    private int currentTargetIndex = 0; // Index of the current target point
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        // Find all GameObjects tagged as "Target" and store their Transforms
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(targetTag);
        targets = new Transform[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++)
        {
            targets[i] = targetObjects[i].transform;
        }

        // Get the NavMeshAgent component attached to this GameObject
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Start moving towards the first target point
        SetDestinationToNextTarget();
    }

    void Update()
    {
        // If the car has reached its destination (current target point), move to the next target point
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
        {
            SetDestinationToNextTarget();
        }
    }

    // Sets the destination of the NavMeshAgent to the next target point
    void SetDestinationToNextTarget()
    {
        // If there are no more target points, stop moving
        if (currentTargetIndex >= targets.Length)
        {
            navMeshAgent.isStopped = true;
            return;
        }

        // Set the destination of the NavMeshAgent to the next target point
        navMeshAgent.SetDestination(targets[currentTargetIndex].position);

        // Move to the next target point in the array
        currentTargetIndex++;
    }
}
