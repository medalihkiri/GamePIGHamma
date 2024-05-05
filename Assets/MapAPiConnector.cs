using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;


// Import JSON.NET namespace

public class MapAPiConnector : MonoBehaviour
{
    private APIConnector apiConnector;
    public ObjectPlacer objectPlacer;
    public PlacementState placementState;
    public List<GameObject> buildingPrefab;
    public Grid grid;

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
    private IEnumerator SaveMapData()
    {
        yield return apiConnector.SendRequestAsync("map/6609f611c37d6b088c3dc6c1/buildings", APIConnector.HttpMethod.POST, OnSuccess, OnFailure);
    }

    private void OnFailure(string error)
    {
        Debug.LogError("Failed to load map data: " + error);
    }

    private void OnSuccess(string response)
    {
        myBuildingBack[] buildings = JsonConvert.DeserializeObject<myBuildingBack[]>(response);
        Debug.Log("Map data: " + response);
        Dictionary<string, GameObject> map = new Dictionary<string, GameObject>();
        foreach (myBuildingBack building in buildings)
        {

            map.Add(building.name, buildingPrefab.Find((x) =>
            {
                return
                x.name == building.name;
            }));


        };


        // Deserialize JSON array into an array of myBuildingBack objects using JSON.NET

        foreach (myBuildingBack building in buildings)
        {
            Debug.Log(building.name.ToString());

            // Place each building
            //objectPlacer.PlaceObject(buildingPrefab[0], new Vector3(building.position[0], building.position[0],0));
            //objectPlacer.PlaceObject(buildingPrefab[1], new Vector3(building.position[0], building.position[1]+2,0));
            Vector3Int x =
                new Vector3Int(building.position[0], 0, building.position[1]);
            Vector3 y =
                grid.CellToWorld(
                new Vector3Int(building.position[0], 0, building.position[1]));
            Vector3Int z = Vector3Int.RoundToInt(y);
            Debug.Log(x.ToString() + "//" + y.ToString());
            placementState.OnAction(x, Quaternion.identity, map.First((x) =>
            {
                return x.Key == building.name;

            }).Value);

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
