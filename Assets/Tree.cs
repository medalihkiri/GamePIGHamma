using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            Debug.Log("Collided with an object tagged as 'Building'. Self-destructing...");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Collided with " + other.name + ", which is not tagged as 'Building'.");
        }
    }
}
