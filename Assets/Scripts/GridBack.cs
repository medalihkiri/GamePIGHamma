using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MyBuildingObjects
{
    public string id;
    public string name;
    public string description;
    public Vector2 position;
    public int level;
}

[System.Serializable]
public class MapData
{
    public string name;
    public string terrainType;
    public List<string> resources;
    public List<MyBuildingObjects> MyBuildingObjectss;
}

public class GridBack : MonoBehaviour
{
    // Grid size
    private const int GRID_SIZE = 2;

    // Map data
    private MapData mapData;

    // Prefabs for MyBuildingObjects visualization
    public GameObject MyBuildingObjectsPrefab;
    public GameObject resourcePrefab;

    // List to keep track of instantiated MyBuildingObjects objects
    private List<GameObject> MyBuildingObjectsObjects = new List<GameObject>();

    // Load map data from the API
    public void LoadMapData()
    {
        APIConnector.Instance.SendRequestAsync("map", APIConnector.HttpMethod.GET, OnLoadSuccess, OnLoadFailure);
    }

    private void OnLoadSuccess(string response)
    {
        mapData = JsonUtility.FromJson<MapData>(response);
        Debug.Log("Map data loaded successfully.");

        VisualizeMap();
    }

    private void OnLoadFailure(string error)
    {
        Debug.LogError("Failed to load map data: " + error);
    }

    // Save map data to the API
    public void SaveMapData()
    {
        string jsonData = JsonUtility.ToJson(mapData);
        APIConnector.Instance.SendRequestAsync("map", APIConnector.HttpMethod.POST, OnSaveSuccess, OnSaveFailure, jsonData);
    }

    private void OnSaveSuccess(string response)
    {
        Debug.Log("Map data saved successfully.");
    }

    private void OnSaveFailure(string error)
    {
        Debug.LogError("Failed to save map data: " + error);
    }

    // Example method to add a MyBuildingObjects to the grid
    public void AddMyBuildingObjects(MyBuildingObjects MyBuildingObjects)
    {
        if (mapData == null)
        {
            Debug.LogError("Map data not loaded.");
            return;
        }

        mapData.MyBuildingObjectss.Add(MyBuildingObjects);

        // Visualize the new MyBuildingObjects
        VisualizeMyBuildingObjects(MyBuildingObjects);
    }

    // Example method to remove a MyBuildingObjects from the grid
    public void RemoveMyBuildingObjects(MyBuildingObjects MyBuildingObjects)
    {
        if (mapData == null)
        {
            Debug.LogError("Map data not loaded.");
            return;
        }

        mapData.MyBuildingObjectss.Remove(MyBuildingObjects);

        // Remove the MyBuildingObjects visualization
        RemoveMyBuildingObjectsVisualization(MyBuildingObjects);
    }

    // Method to visualize the map
    private void VisualizeMap()
    {
        if (mapData == null)
        {
            Debug.LogError("Map data not loaded.");
            return;
        }

        // Visualize MyBuildingObjectss
        foreach (MyBuildingObjects MyBuildingObjects in mapData.MyBuildingObjectss)
        {
            VisualizeMyBuildingObjects(MyBuildingObjects);
        }

        // Visualize resources
        foreach (string resource in mapData.resources)
        {
            VisualizeResource(resource);
        }
    }

    // Method to visualize a MyBuildingObjects
    private void VisualizeMyBuildingObjects(MyBuildingObjects MyBuildingObjects)
    {
        GameObject MyBuildingObjectsObject = Instantiate(MyBuildingObjectsPrefab, new Vector3(MyBuildingObjects.position.x, 0, MyBuildingObjects.position.y), Quaternion.identity);
        MyBuildingObjectsObjects.Add(MyBuildingObjectsObject);
    }

    // Method to remove MyBuildingObjects visualization
    private void RemoveMyBuildingObjectsVisualization(MyBuildingObjects MyBuildingObjects)
    {
        foreach (GameObject MyBuildingObjectsObject in MyBuildingObjectsObjects)
        {
            if (MyBuildingObjectsObject.transform.position.x == MyBuildingObjects.position.x && MyBuildingObjectsObject.transform.position.z == MyBuildingObjects.position.y)
            {
                Destroy(MyBuildingObjectsObject);
                MyBuildingObjectsObjects.Remove(MyBuildingObjectsObject);
                break;
            }
        }
    }

    // Method to visualize a resource (for demonstration purposes)
    private void VisualizeResource(string resource)
    {
        // For demonstration purposes, let's create a sphere as a placeholder for resources
        Vector2 resourcePosition = new Vector2(UnityEngine.Random.Range(0, GRID_SIZE), UnityEngine.Random.Range(0, GRID_SIZE));
        GameObject resourceObject = Instantiate(resourcePrefab, new Vector3(resourcePosition.x, 0, resourcePosition.y), Quaternion.identity);
    }
}
