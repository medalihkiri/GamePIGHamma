using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public Grid grid;
    [SerializeField]
    public GridData gridData;
    Vector3 pivotOffset = new Vector3(0f, 10f, 0f);
    [SerializeField]

    public List<Building> placedBuildings = new List<Building>();
    public List<GameObject> list = new List<GameObject>();  
    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        Debug.Log(prefab.gameObject.name);
        Debug.Log(position.ToString());
        GameObject newObject = Instantiate(prefab, transform.position, Quaternion.identity);
        newObject.transform.position = position;

        //newObject.transform.position -= pivotOffset;
        //newObject.transform.Rotate(0,90,0);
        //newObject.transform.position += pivotOffset;
        Building buildingComponent = newObject.GetComponent<Building>();
        if (buildingComponent != null)
        {
            placedBuildings.Add(buildingComponent);
            buildingComponent.MarkAsPlaced();
        }

        return placedBuildings.Count - 1;
    }


    public void Start()
    {
        list.ForEach(building => {
            Building MyBuilding = building.GetComponent<Building>();
            if (MyBuilding != null)
            {
                PlaceObject(building,grid.CellToWorld(MyBuilding.positionInGrid));
                gridData.AddObjectAt(Vector3Int.RoundToInt(grid.CellToWorld(MyBuilding.positionInGrid)), new Vector2Int(2, 2), 1, 1);

                //gridData.AddObjectAt(grid.CellToWorld(MyBuilding.positionInGrid).ConvertTo<Vector3Int>(), new Vector2Int(2, 2), 1, 1);            
            }

        });
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (gameObjectIndex < 0 || gameObjectIndex >= placedBuildings.Count || placedBuildings[gameObjectIndex] == null)
            return;

        Destroy(placedBuildings[gameObjectIndex].gameObject);
        placedBuildings.RemoveAt(gameObjectIndex);
    }

    // Method to return the list of placed buildings
    public List<Building> GetPlacedBuildings()
    {
        return placedBuildings;
    }

}