using System.Collections;
using UnityEngine;
using Newtonsoft.Json; // Import JSON.NET namespace

public class MapAPiConnector : MonoBehaviour
{
    private APIConnector apiConnector;
    public ObjectPlacer objectPlacer;
    public GameObject[] buildingPrefab;

    void Start()
    {
        apiConnector = GetComponent<APIConnector>();
        if (apiConnector == null)
        {
            Debug.LogError("APIConnector component not found.");
            return;
        }
        StartCoroutine(LoadMapData());
    }

    private IEnumerator LoadMapData()
    {
        yield return apiConnector.SendRequestAsync("map/6609f611c37d6b088c3dc6c1/buildings", APIConnector.HttpMethod.GET, OnSuccess, OnFailure);
    }

    private void OnFailure(string error)
    {
        Debug.LogError("Failed to load map data: " + error);
    }

    private void OnSuccess(string response)
    {
        Debug.Log("Map data: " + response);
       

        // Deserialize JSON array into an array of myBuildingBack objects using JSON.NET
        myBuildingBack[] buildings = JsonConvert.DeserializeObject<myBuildingBack[]>(response);

        foreach (myBuildingBack building in buildings)
        {
            Debug.Log(building.name.ToString());
            // Place each building
            objectPlacer.PlaceObject(buildingPrefab[0], new Vector3(building.position[0], building.position[0],0));
            objectPlacer.PlaceObject(buildingPrefab[1], new Vector3(building.position[0], building.position[1]+2,0));
        }
    }
}

[System.Serializable]
public class myBuildingBack
{
    public string name;
    public string id;
    public int[] position;
    public int level;
}
