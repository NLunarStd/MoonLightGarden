using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(FertileDetailGenerator))]
[RequireComponent(typeof(MudDetailGenerator))]
[RequireComponent(typeof(WaterDetailGenerator))]
[RequireComponent(typeof(DeepWaterDetailGenerator))]
[RequireComponent(typeof(SoilDetailGenerator))]
[RequireComponent(typeof(GenerateTilemapWithPerlinNoise))]
public class GroundGeneration : MonoBehaviour
{
    SoilDetailGenerator soilDetailGenerator;
    FertileDetailGenerator fertileDetailGenerator;
    MudDetailGenerator mudDetailGenerator;
    WaterDetailGenerator waterDetailGenerator;
    DeepWaterDetailGenerator deepWaterDetailGenerator;
    GenerateTilemapWithPerlinNoise generateTilemapWithPerlin;


    private void Start()
    {
        SetUp();
    }

    void SetUp()
    {
        soilDetailGenerator = GetComponent<SoilDetailGenerator>();
        fertileDetailGenerator = GetComponent<FertileDetailGenerator>();
        mudDetailGenerator = GetComponent<MudDetailGenerator>();
        waterDetailGenerator = GetComponent<WaterDetailGenerator>();
        deepWaterDetailGenerator = GetComponent<DeepWaterDetailGenerator>();
        generateTilemapWithPerlin = GetComponent<GenerateTilemapWithPerlinNoise>();

        soilRuletile = soilDetailGenerator.soilRuletile;
        soiltilemap = soilDetailGenerator.soiltilemap;
        StartCoroutine(GenerateCoroutine()); 
    }

    public Tilemap soiltilemap;
    public TileBase soilRuletile;
    public Tilemap groundTilemap;
    System.Collections.IEnumerator GenerateCoroutine()
    {
        generateTilemapWithPerlin.GenerateTilemap(); 
        yield return null; 
        BoundsInt bounds = groundTilemap.cellBounds;
        bounds.xMin = bounds.xMin * 2;
        bounds.yMin = bounds.yMin * 2;
        bounds.xMax = bounds.xMax * 2;
        bounds.yMax = bounds.yMax * 2;
        soilDetailGenerator.GenerateDetails(soiltilemap, bounds, soilRuletile);
        fertileDetailGenerator.GenerateDetailMap();
        mudDetailGenerator.GenerateDetailMap();
        waterDetailGenerator.GenerateDetailMap();
        deepWaterDetailGenerator.GenerateDetailMap();
        yield return null;
        AstarPath.active.Scan();
        yield return null;
        GameManager.instance.SetUpComplete();
    }
}