using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SoilDetailGenerator : MonoBehaviour
{
    public Tilemap soiltilemap;
    public TileBase soilRuletile;
    public Tilemap groundTilemap;

    public void GenerateDetails(Tilemap targetTilemap, BoundsInt bounds, TileBase Tile)
    {
        for (int x = bounds.xMin; x < bounds.xMax ; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                targetTilemap.SetTile(tilePos, Tile);
            }
        }
    }

}
