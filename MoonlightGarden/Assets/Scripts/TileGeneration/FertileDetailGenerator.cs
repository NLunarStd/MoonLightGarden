using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FertileDetailGenerator : TileGenerator
{
    public TileBase fertileRuleTile;
    public TileBase fertileTile;

    public override void GenerateDetailMap()
    {
        if (groundTilemap == null || targetTilemap == null)
        {
            Debug.LogError("Tilemaps not assigned!");
            return;
        }

        groundBounds = groundTilemap.cellBounds;

        for (int groundX = groundBounds.xMin; groundX < groundBounds.xMax; groundX++)
        {
            for (int groundY = groundBounds.yMin; groundY < groundBounds.yMax; groundY++)
            {
                GenerateDetail(groundX, groundY);
            }
        }
    }

    protected override void GenerateTileDetails(int detailX, int detailY, TileBase underlineTile)
    {
        if (underlineTile == fertileTile)
        {
            targetTilemap.SetTile(new Vector3Int(detailX, detailY, 0), fertileRuleTile);
            targetTilemap.SetTile(new Vector3Int(detailX + 1, detailY, 0), fertileRuleTile);
            targetTilemap.SetTile(new Vector3Int(detailX, detailY + 1, 0), fertileRuleTile);
            targetTilemap.SetTile(new Vector3Int(detailX + 1, detailY + 1, 0), fertileRuleTile);
        }
    }
}
