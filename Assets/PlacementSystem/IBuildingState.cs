using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnAction(Vector3Int position, Quaternion rotation);
    void UpdateState(Vector3Int gridPosition);
    void Rotate();
}