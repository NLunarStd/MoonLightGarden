using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;
    public Tilemap grid;

    public Transform buildingParent;

    private void Awake()
    {
        instance = this;
    }

    public void PlaceBuilding(GameObject buildingPrefab, Transform interactorTransform) 
    {
        if (grid != null && interactorTransform != null) 
        {
            Debug.Log("interactorTransform.position: " + interactorTransform.position);
            Vector3Int cellPosition = grid.WorldToCell(interactorTransform.position);
            Debug.Log("cellPosition: " + cellPosition); 
            GameObject buildingInstance = Instantiate(buildingPrefab, interactorTransform.position, Quaternion.identity); 
            buildingInstance.transform.SetParent(buildingParent);
        }
        else
        {
            Debug.LogError("BuildingManager: grid หรือ interactorTransform เป็น null");
        }
    }
}