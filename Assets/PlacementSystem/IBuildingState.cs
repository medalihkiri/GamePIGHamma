using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnAction(Vector3Int position, Quaternion rotation);
    void OnAction(Vector3Int position, Quaternion rotation, GameObject prefab);
    void UpdateState(Vector3Int gridPosition);
    void Rotate();
}