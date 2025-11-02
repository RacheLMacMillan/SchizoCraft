using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public Dictionary<Vector2Int, ChunkData> ChunkData = new Dictionary<Vector2Int, ChunkData>();

    public ChunkRenderer ChunkPrefab;
    
    private void Start()
    {
        for (int x = 0; x < 10; x++)
        {
            for ( int y = 0; y < 10; y++)
            {
                float xPos = x * ChunkRenderer.ChunkWidth * ChunkRenderer.BlockScale;
                float zPos = y * ChunkRenderer.ChunkWidth * ChunkRenderer.BlockScale;

                ChunkData chunkData = new ChunkData();

                chunkData.Blocks = TerrainGenerator.GenerateTerrain((int)xPos, (int)zPos);

                ChunkData.Add(new Vector2Int(x, y), chunkData);

                var chunk = Instantiate(ChunkPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
                
                chunk.ChunkData = chunkData;
                chunk.ParentWorld = this;
            }
        }
    }
}