using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GenerateDetailTilemap : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap detailTilemap;

    protected Dictionary<string, TileBase> tileRules;

    protected int detailX;
    protected int detailY;
    protected int overlapGroundX;
    protected int overlapGroundY;
    protected int offsetX;
    protected int offsetY;
    protected BoundsInt groundBounds;
    protected TileBase[] overlapTiles;
    protected TileBase overlapTileCurrent;
    protected TileBase overlapTileRight;
    protected TileBase overlapTileTop;
    protected TileBase overlapTileTopRight;

    void Start()
    {
        GenerateDetailMap();
    }


    protected virtual void GenerateDetailMap()
    {
        if (groundTilemap == null || detailTilemap == null)
        {
            Debug.LogError("Tilemaps not assigned!");
            return;
        }

        groundBounds = groundTilemap.cellBounds;

        for (int groundX = groundBounds.xMin; groundX < groundBounds.xMax; groundX++)
        {
            for (int groundY = groundBounds.yMin; groundY < groundBounds.yMax; groundY++)
            {
                for (offsetX = 0; offsetX < 2; offsetX++)
                {
                    for (offsetY = 0; offsetY < 2; offsetY++)
                    {
                        detailX = (groundX - groundBounds.xMin) * 2 + offsetX + groundBounds.xMin * 2;
                        detailY = (groundY - groundBounds.yMin) * 2 + offsetY + groundBounds.yMin * 2;

                        overlapGroundX = groundX;
                        overlapGroundY = groundY;

                        if (offsetX == 1) overlapGroundX++;
                        if (offsetY == 1) overlapGroundY++;

                        overlapGroundX = Mathf.Clamp(overlapGroundX, groundBounds.xMin, groundBounds.xMax);
                        overlapGroundY = Mathf.Clamp(overlapGroundY, groundBounds.yMin, groundBounds.yMax);

                        overlapTileCurrent = groundTilemap.GetTile(new Vector3Int(groundX, groundY, 0));
                        overlapTileRight = groundTilemap.GetTile(new Vector3Int(groundX + 1, groundY, 0));
                        overlapTileTop = groundTilemap.GetTile(new Vector3Int(groundX, groundY + 1, 0));
                        overlapTileTopRight = groundTilemap.GetTile(new Vector3Int(groundX + 1, groundY + 1, 0));

                        overlapTiles = new TileBase[4];
                        overlapTiles[0] = overlapTileCurrent;
                        overlapTiles[1] = overlapTileRight;
                        overlapTiles[2] = overlapTileTop;
                        overlapTiles[3] = overlapTileTopRight;

                        TileBase tileForDetail = GetTileForDetail(overlapTiles, offsetX, offsetY);
                        if (tileForDetail != null)
                        {
                            detailTilemap.SetTile(new Vector3Int(detailX, detailY, 0), tileForDetail);
                        }
                    }
                }
            }
        }
    }

    protected virtual TileBase GetTileForDetail(TileBase[] neighbors, int offsetX, int offsetY)
    {
        return null; 
    }
}