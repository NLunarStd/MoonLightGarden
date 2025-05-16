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
            // �� buildPositionTransform.position �繵��˹��ҧ
            Instantiate(buildingPrefab, buildPositionTransform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("BuildingItem: buildPositionTransform ���� buildingPrefab �� null");
        }
    }

    public void SetGridAndPosition(Tilemap grid, Vector3Int position, Transform buildPositionTransform)
    {
        this.grid = grid;
        this.buildPosition = position;
        this.buildPositionTransform = buildPositionTransform;
    }
}