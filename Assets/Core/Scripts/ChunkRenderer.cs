using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class ChunkRenderer : MonoBehaviour
{
    public const int ChunkWidth = 16;
    public const int ChunkHeight = 256;
    public const float BlockScale = 0.5f;

    public ChunkData ChunkData;
    public GameWorld ParentWorld;
    
    private List<Vector3> verticals = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private void Start()
    {
        Mesh chunkMesh = new Mesh();

        for (int y = 0; y < ChunkHeight; y++)
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    GenerateBlock(x, y, z);
                }
            }
        }

        chunkMesh.vertices = verticals.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        chunkMesh.Optimize();

        chunkMesh.RecalculateNormals();
        chunkMesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = chunkMesh;
        GetComponent<MeshCollider>().sharedMesh = chunkMesh;
    }
    
    private BlockTypes GetBlockAtPosition(Vector3Int blockPosition)
    {
        if (blockPosition.x >= 0 && blockPosition.x < ChunkWidth &&
            blockPosition.y >= 0 && blockPosition.y < ChunkHeight &&
            blockPosition.z >= 0 && blockPosition.z < ChunkWidth)
        {
            return ChunkData.Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
        }
        else
        {
            return BlockTypes.Air;
        }
    }
    
    private void GenerateBlock(int x, int y, int z)
    {
        Vector3Int blockPosition = new Vector3Int(x, y, z);

        if (GetBlockAtPosition(blockPosition) == 0) return;
            
        if (GetBlockAtPosition(blockPosition + Vector3Int.right) == 0) 
            GenerateRightSide(blockPosition);
            
        if (GetBlockAtPosition(blockPosition + Vector3Int.left) == 0)
            GenerateLeftSide(blockPosition);
            
        if (GetBlockAtPosition(blockPosition + Vector3Int.forward) == 0)
            GenerateFrontSide(new Vector3Int(x, y, z));
            
        if (GetBlockAtPosition(blockPosition + Vector3Int.back) == 0)
            GenerateBackSide(new Vector3Int(x, y, z));
            
        if (GetBlockAtPosition(blockPosition + Vector3Int.up) == 0)
            GenerateTopSide(new Vector3Int(x, y, z));
            
        if (GetBlockAtPosition(blockPosition + Vector3Int.down) == 0)
            GenerateBottomSide(new Vector3Int(x, y, z));
    }
    
    private void GenerateRightSide(Vector3Int blockPosition)
    {
        verticals.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);
        
        AddLastVerticalSquare();
    }

    private void GenerateLeftSide(Vector3Int blockPosition)
    {
        verticals.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);

        AddLastVerticalSquare();
    }
    
    private void GenerateFrontSide(Vector3Int blockPosition)
    {
        verticals.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

        AddLastVerticalSquare();
    }
    
    private void GenerateBackSide(Vector3Int blockPosition)
    {
        verticals.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);

        AddLastVerticalSquare();
    }
    
    private void GenerateTopSide(Vector3Int blockPosition)
    {
        verticals.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

        AddLastVerticalSquare();
    }
    
    private void GenerateBottomSide(Vector3Int blockPosition)
    {
        verticals.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticals.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);

        AddLastVerticalSquare();
    }

    private void AddLastVerticalSquare()
    {
        triangles.Add(verticals.Count - 4);
        triangles.Add(verticals.Count - 3);
        triangles.Add(verticals.Count - 2);

        triangles.Add(verticals.Count - 3);
        triangles.Add(verticals.Count - 1);
        triangles.Add(verticals.Count - 2);
    }
}
