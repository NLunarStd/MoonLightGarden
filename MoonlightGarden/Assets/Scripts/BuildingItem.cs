using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingItem : Item
{
    public GameObject buildingPrefab;
    public Transform buildPositionTransform;
    public Tilemap grid;
    public Vector3Int buildPosition;

    public override void UseItem()
    {
        if (buildPositionTransform != null && buildingPrefab != null)
        {
            // ใช้ buildPositionTransform.position เป็นตำแหน่งวาง
            Instantiate(buildingPrefab, buildPositionTransform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("BuildingItem: buildPositionTransform หรือ buildingPrefab เป็น null");
        }
    }

    public void SetGridAndPosition(Tilemap grid, Vector3Int position, Transform buildPositionTransform)
    {
        this.grid = grid;
        this.buildPosition = position;
        this.buildPositionTransform = buildPositionTransform;
    }
}