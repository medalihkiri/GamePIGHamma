using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIHandler : MonoBehaviour
{
    private APIConnector apiConnector;

    void Start()
    {
        apiConnector = GetComponent<APIConnector>(); // Assuming APIConnector is attached to the same GameObject
        if (apiConnector == null)
        {
            Debug.LogError("APIConnector component not found.");
            return;
        }

        StartCoroutine(LoadMapData());
    }

    private IEnumerator LoadMapData()
    {
        apiConnector.SendRequestAsync("map", APIConnector.HttpMethod.GET, OnSuccess, OnFailure);
        yield return null; // Wait for the request to complete
    }

    private void OnSuccess(string response)
    {
        Debug.Log("Map data: " + response);
    }

    private void OnFailure(string error)
    {
        Debug.LogError("Failed to load map data: " + error);
    }
}


