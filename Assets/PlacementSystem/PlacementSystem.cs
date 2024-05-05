using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private GameObject gridVisualization;
    private int currentBuildingID;
    [SerializeField] private AudioClip correctPlacementClip, wrongPlacementClip;
    [SerializeField] private AudioSource source;
    private bool isObjectPlaced = false;
    private GridData floorData, furnitureData;

    [SerializeField] private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField] private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    [SerializeField] private SoundFeedback soundFeedback;
    private Quaternion rotation = Quaternion.Euler(0, 90, 0);

    private void Start()
    {
        gridVisualization.SetActive(false);
        floorData = new();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        currentBuildingID = ID; // Store the current building ID
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           furnitureData,
                                           objectPlacer,
                                           soundFeedback);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnRotate += RotateStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer, soundFeedback);
        inputManager.OnClicked += RemoveStructure; // Changed the event handler
        inputManager.OnExit += StopPlacement;
    }
    private void RotateStructure()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        rotation *= Quaternion.Euler(0, 90, 0);
        buildingState.Rotate();
        Debug.Log(rotation);
        //PlacementState.RotateBuilding(gridPosition);
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        if (isObjectPlaced) // Check if an object has already been placed
        {
            return; // If yes, return without placing another object
        }

        // Check if the player can afford the building
        /* if (!ResourceManager.Instance.CanAffordBuilding(currentBuildingID))
         {
             Debug.LogWarning("Not enough resources to place this building.");
             return; // Abort placement if resources are insufficient
         }*/

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // Proceed with placement
        buildingState.OnAction(gridPosition, rotation);
        isObjectPlaced = true; // Set flag to true after placing an object
    }

    private void RemoveStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // Proceed with removal
        buildingState.OnAction(gridPosition, rotation);

        // Reset the isObjectPlaced flag after removing the building
        isObjectPlaced = false;
    }

    private void StopPlacement()
    {
        soundFeedback.PlaySound(SoundType.Click);
        if (buildingState == null)
            return;
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure; // Unsubscribe from PlaceStructure
        inputManager.OnClicked -= RemoveStructure; // Unsubscribe from RemoveStructure
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
        isObjectPlaced = false;
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }
}