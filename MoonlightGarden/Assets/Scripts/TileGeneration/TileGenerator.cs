using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileGenerator : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap targetTilemap;

    protected BoundsInt groundBounds;
    protected int offsetX;
    protected int offsetY;
    protected int detailX;
    protected int detailY;
    protected TileBase underlineTile;

    protected virtual void GenerateDetail(int groundX, int groundY)
    {
       
        for (offsetX = 0; offsetX < 2; offsetX++)
        {
            for (offsetY = 0; offsetY < 2; offsetY++)
            {
                detailX = (groundX - groundBounds.xMin) * 2 + offsetX + groundBounds.xMin * 2;
                detailY = (groundY - groundBounds.yMin) * 2 + offsetY + groundBounds.yMin * 2;
                underlineTile = groundTilemap.GetTile(new Vector3Int(groundX, groundY, 0));

                if (underlineTile != null)
                {
                    GenerateTileDetails(detailX, detailY, underlineTile);
                }
            }
        }
    }

    protected abstract void GenerateTileDetails(int detailX, int detailY, TileBase underlineTile);

    public abstract void GenerateDetailMap();
}
