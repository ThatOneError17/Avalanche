using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;

public class TileLevelGen : MonoBehaviour
{
    [Header("Speed Settings")]
    public float scrollSpeed = 5f;

    [Header("Tile Settings")]
    public GameObject chunkPrefab;
    public float chunkWidth = 10f;
    public TileBase groundTile;
    public TileBase obstacleTile;

    [Range(0, 1)]
    public float obstacleProbability = 0.2f;

    [Header("Object Pool Settings")]
    public int initialPoolSize = 10;
    public int maxPoolSize = 20;

    private ObjectPool<GameObject> tilePool;
    private List<GameObject> activeTiles = new List<GameObject>();

    private Camera mainCamera;
    private float screenLeftEdge;
    private float screenRightEdge;
    private float nextSpawnX = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        if (obstacleTile == null)
        {
            Debug.LogError("Obstacle Tile is not assigned in the inspector. Tile Generation will not work");
            return;
        }

        if (groundTile == null)
        {
            Debug.LogError("Ground Tile is not assigned in the inspector. Tile Generation will not work");
            return;
        }

        if (chunkPrefab == null)
        {
            Debug.LogError("Chunk Prefab is not assigned in the inspector. Tile Generation will not work");
            return;
        }

        tilePool = new ObjectPool<GameObject>(
            createFunc: CreateNewChunk,
            actionOnGet: PositionAndActivate,
            actionOnRelease: DeactivateChunk,
            actionOnDestroy: DestroyChunk,
            collectionCheck: false,
            defaultCapacity: initialPoolSize,
            maxSize: maxPoolSize
        );

        CalculateScreenBounds();

        while (nextSpawnX < screenRightEdge + chunkWidth)
        {
            tilePool.Get();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (obstacleTile == null || groundTile == null)
        {
            Debug.LogError("Tiles are not properly assigned in the inspector. Tile Generation will not work");
            return;
        }

        float deltaX = scrollSpeed * Time.deltaTime;

        for (int i = activeTiles.Count - 1; i >= 0; i--)
        {
            GameObject chunk = activeTiles[i];
            chunk.transform.position += Vector3.left * deltaX;
        }

        nextSpawnX -= deltaX;

        //check if leftmost chunk is offscreen
        if (activeTiles.Count > 0 && (activeTiles[0].transform.position.x + chunkWidth < screenLeftEdge))
        {
            GameObject chunkToRelease = activeTiles[0];
            activeTiles.RemoveAt(0);
            tilePool.Release(chunkToRelease);
        }

        GameObject rightMostChunk = (activeTiles.Count > 0) ? activeTiles[activeTiles.Count - 1] : null;
        float rightMostEdgeX = (rightMostChunk != null) ? rightMostChunk.transform.position.x + chunkWidth : nextSpawnX;

        //spawn new chunk if needed
        if (rightMostEdgeX < screenRightEdge + chunkWidth)
        {
            tilePool.Get();
        }
    }

    private GameObject CreateNewChunk()
    {
        GameObject chunk = Instantiate(chunkPrefab);
        chunk.SetActive(false);

        if (chunk.GetComponentInChildren<Tilemap>() == null)
        {
            Debug.LogError("Chunk Prefab does not have a Tilemap component. Tile Generation will not work");
            Destroy(chunk);
            return null;
        }

        return chunk;
    }

    private void PositionAndActivate(GameObject chunk)
    {
        chunk.SetActive(true);
        chunk.transform.position = new Vector3(nextSpawnX, 0, 0);
        Tilemap tilemap = chunk.GetComponentInChildren<Tilemap>();
        nextSpawnX += chunkWidth;


        tilemap.ClearAllTiles();

        activeTiles.Add(chunk);

        // Generate tiles for the chunk
        // For simplicity, we fill the bottom row with ground tiles and randomly place obstacles on the row above
        // You can expand this logic to create more complex patterns
        // Assuming the chunk height is at least 5
        for (int x = 0; x < chunkWidth; x++)
        {
            Vector3Int tilePos = new Vector3Int(x, -5, 0);
            tilemap.SetTile(tilePos, groundTile);

            if (Random.value < obstacleProbability)
            {
                Vector3Int obstaclePos = new Vector3Int(x, -4, 0);
                tilemap.SetTile(obstaclePos, obstacleTile);
            }
        }
    }
    private void DeactivateChunk(GameObject chunk)
    {
        chunk.SetActive(false);
        activeTiles.Remove(chunk);
    }

    private void DestroyChunk(GameObject chunk)
    {
        Destroy(chunk);
    }

    private void CalculateScreenBounds()
    {
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;
        screenLeftEdge = mainCamera.transform.position.x - (screenWidth / 2);
        screenRightEdge = mainCamera.transform.position.x + (screenWidth / 2);
    }
}
