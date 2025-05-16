using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTilemapWithPerlinNoise : MonoBehaviour
{
    public Tilemap groundMap;
    public Tilemap paintMap;

    [Header("Perlin Noise Parameters")]
    public float scale = 5f;
    public float offsetX = 0f;
    public float offsetY = 0f;

    [Header("Tile Thresholds")]
    public float soilThreshold = 0.2f;
    public float fertileThreshold = 0.5f;
    public float mudThreshold = 0.75f;
    public float waterThreshold = 0.8f;

    [Header("Tiles")]
    public Tile fertileTile;
    public Tile mudTile;
    public Tile waterTile;
    public Tile deepWaterTile;

    public int mapWidth = 50;
    public int mapHeight = 50;

    public void GenerateTilemap()
    {
        if (groundMap == null)
        {
            Debug.LogError("Ground Tilemap is not assigned!");
            return;
        }

        groundMap.ClearAllTiles(); // ล้าง Tilemap ก่อนเริ่มใหม่

        float[,] noiseMap = GeneratePerlinNoiseMap(mapWidth, mapHeight, scale, offsetX, offsetY);

        StartCoroutine(Generate(noiseMap));
    }
    IEnumerator Generate(float[,] noiseMap)
    {
        ApplyNoiseToTilemap(noiseMap);
        yield return null;
        MergePaintToGround();
    }

    private float[,] GeneratePerlinNoiseMap(int width, int height, float scale, float offsetX, float offsetY)
    {
        float[,] noiseMap = new float[width, height];
        float startX = -width / 2f;
        float startY = -height / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // ปรับ Offset
                float xCoord = (float)(x + startX) / width * scale + offsetX;
                float yCoord = (float)(y + startY) / height * scale + offsetY;

                noiseMap[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }

        return noiseMap;
    }

    private void ApplyNoiseToTilemap(float[,] noiseMap)
    {
        // คำนวณตำแหน่งเริ่มต้น
        int startX = -mapWidth / 2;
        int startY = -mapHeight / 2;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int position = new Vector3Int(x + startX, y + startY, 0); // ปรับตำแหน่ง Tile
                float noiseValue = noiseMap[x, y];

                if (noiseValue < soilThreshold)
                {
                    groundMap.SetTile(position, null);
                }
                else if (noiseValue < fertileThreshold)
                {
                    groundMap.SetTile(position, fertileTile);
                }
                else if (noiseValue < mudThreshold)
                {
                    groundMap.SetTile(position, mudTile);
                }
                else if (noiseValue < waterThreshold)
                {
                    groundMap.SetTile(position, waterTile);
                }
                else
                {
                    groundMap.SetTile(position, deepWaterTile);
                }
            }
        }
    }

    public void MergePaintToGround()
    {
        if (paintMap == null)
        {
            Debug.LogError("Paint Tilemap is not assigned!");
            return;
        }

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int position = new Vector3Int(x - mapWidth / 2, y - mapHeight / 2, 0); // ปรับตำแหน่ง Tile
                TileBase paintTile = paintMap.GetTile(position);

                if (paintTile != null)
                {
                    groundMap.SetTile(position, paintTile);
                }
            }
        }
    }
}