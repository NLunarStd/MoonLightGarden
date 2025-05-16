using UnityEngine;

public class GenerateCloudEdge : MonoBehaviour
{
    public GroundGeneration groundGeneration; 
    public GameObject cloudEdgeParticle; 
    public Collider2D collider2D; 
    public Transform cloudEdgeParent; 
    public int edgeLengthParticleCount = 5; 
    public int edgeWidthParticleCount = 3; 
    public float cloudSpacing = 1f; 
    public float rotationSpeed = 10f; 

    void Start()
    {
        GenerateCornerParticles(); 
        GenerateCloudEdgeParticles(); 
        GenerateEdgeCollider(); 
    }

    void GenerateCornerParticles()
    {
        GenerateTilemapWithPerlinNoise groundGenerator = groundGeneration.GetComponent<GenerateTilemapWithPerlinNoise>();
        int mapWidth = groundGenerator.mapWidth;
        int mapHeight = groundGenerator.mapHeight;

        // คำนวณตำแหน่งมุม
        Vector3 topLeft = new Vector3(-mapWidth * cloudSpacing / 2f, mapHeight * cloudSpacing / 2f, 0f);
        Vector3 topRight = new Vector3(mapWidth * cloudSpacing / 2f, mapHeight * cloudSpacing / 2f, 0f);
        Vector3 bottomLeft = new Vector3(-mapWidth * cloudSpacing / 2f, -mapHeight * cloudSpacing / 2f, 0f);
        Vector3 bottomRight = new Vector3(mapWidth * cloudSpacing / 2f, -mapHeight * cloudSpacing / 2f, 0f);

        // วาง Particle ที่มุม
        InstantiateCloudParticle(cloudEdgeParticle, topLeft, new Vector3(1f, -1f, 0f), true);
        InstantiateCloudParticle(cloudEdgeParticle, topRight, new Vector3(-1f, -1f, 0f), true);
        InstantiateCloudParticle(cloudEdgeParticle, bottomLeft, new Vector3(1f, 1f, 0f), false);
        InstantiateCloudParticle(cloudEdgeParticle, bottomRight, new Vector3(-1f, 1f, 0f), false);
    }

    void GenerateCloudEdgeParticles()
    {
        GenerateTilemapWithPerlinNoise groundGenerator = groundGeneration.GetComponent<GenerateTilemapWithPerlinNoise>();
        int mapWidth = groundGenerator.mapWidth;
        int mapHeight = groundGenerator.mapHeight;

        // ขอบด้านบนและด้านล่าง
        for (int x = 0; x < edgeLengthParticleCount ; x++)
        {
            float posX = (x / (float)(edgeLengthParticleCount)) * mapWidth * cloudSpacing - mapWidth * cloudSpacing / 2f;
            Vector3 topPosition = new Vector3(posX, mapHeight * cloudSpacing / 2f, 0f);
            InstantiateCloudParticle(cloudEdgeParticle, topPosition, new Vector3(1f, -1f, 0f), true);

            Vector3 bottomPosition = new Vector3(posX, -mapHeight * cloudSpacing / 2f, 0f);
            InstantiateCloudParticle(cloudEdgeParticle, bottomPosition, new Vector3(1f, 1f, 0f), false);
        }

        // ขอบด้านซ้ายและด้านขวา
        for (int y = 0; y < edgeWidthParticleCount ; y++)
        {
            float posY = (y / (float)(edgeWidthParticleCount )) * mapHeight * cloudSpacing - mapHeight * cloudSpacing / 2f;
            Vector3 leftPosition = new Vector3(-mapWidth * cloudSpacing / 2f, posY, 0f);
            InstantiateCloudParticle(cloudEdgeParticle, leftPosition, new Vector3(1f, 1f, 0f), false);

            Vector3 rightPosition = new Vector3(mapWidth * cloudSpacing / 2f, posY, 0f);
            InstantiateCloudParticle(cloudEdgeParticle, rightPosition, new Vector3(-1f, 1f, 0f), false);
        }
    }

    void InstantiateCloudParticle(GameObject particle, Vector3 position, Vector3 direction, bool flipY)
    {
        GameObject cloud = Instantiate(particle, position, Quaternion.Euler(0f, 0f, Random.Range(-rotationSpeed, rotationSpeed)), cloudEdgeParent);
        ParticleSystem ps = cloud.GetComponent<ParticleSystem>();
        var velocityModule = ps.velocityOverLifetime;
        velocityModule.x = direction.x;
        velocityModule.y = direction.y;

        if (flipY)
        {
            cloud.transform.localScale = new Vector3(cloud.transform.localScale.x, -cloud.transform.localScale.y, cloud.transform.localScale.z);
        }

        if (position.x == -groundGeneration.GetComponent<GenerateTilemapWithPerlinNoise>().mapWidth * cloudSpacing / 2f) // ขอบด้านซ้าย
        {
            cloud.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (position.x == groundGeneration.GetComponent<GenerateTilemapWithPerlinNoise>().mapWidth * cloudSpacing / 2f) // ขอบด้านขวา
        {
            cloud.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (position.y == groundGeneration.GetComponent<GenerateTilemapWithPerlinNoise>().mapHeight * cloudSpacing / 2f) // ขอบด้านบน
        {
            cloud.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (position.y == -groundGeneration.GetComponent<GenerateTilemapWithPerlinNoise>().mapHeight * cloudSpacing / 2f) // ขอบด้านล่าง
        {
            cloud.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    void GenerateEdgeCollider()
    {
        GenerateTilemapWithPerlinNoise groundGenerator = groundGeneration.GetComponent<GenerateTilemapWithPerlinNoise>();
        int mapWidth = groundGenerator.mapWidth;
        int mapHeight = groundGenerator.mapHeight;

    }
}