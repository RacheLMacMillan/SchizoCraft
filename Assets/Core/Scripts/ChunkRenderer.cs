using System.CodeDom.Compiler;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class ChunkRenderer : MonoBehaviour
{
    private const int ChunkWidth = 16;
    private const int ChunkHeight = 256;

    public int[,,] Blocks = new int[ChunkWidth, ChunkHeight, ChunkWidth];
    
    private List<Vector3> verticals = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private void Start()
    {
        Mesh chunkMesh = new Mesh();
        
        Blocks[0,0,0] = 1;
        Blocks[0,0,1] = 1;
        Blocks[0,0,2] = 1;
        Blocks[5,1,1] = 1;
        Blocks[5,2,1] = 1;
        Blocks[5,0,1] = 1;
        Blocks[6,1,1] = 1;
        Blocks[4,1,1] = 1;
        Blocks[5,1,2] = 1;
        Blocks[5,1,0] = 1;

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

        chunkMesh.RecalculateNormals();
        chunkMesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = chunkMesh;
    }
    
    private int GetBlockAtPosition(Vector3Int blockPosition)
    {
        if (blockPosition.x >= 0 && blockPosition.x < ChunkWidth &&
            blockPosition.y >= 0 && blockPosition.y < ChunkHeight &&
            blockPosition.z >= 0 && blockPosition.z < ChunkWidth)
        {
            return Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
        }
        else
        {
            return 0;
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
        verticals.Add(new Vector3(1, 0, 0) + blockPosition);
        verticals.Add(new Vector3(1, 1, 0) + blockPosition);
        verticals.Add(new Vector3(1, 0, 1) + blockPosition);
        verticals.Add(new Vector3(1, 1, 1) + blockPosition);
        
        AddLastVerticalSquare();
    }

    private void GenerateLeftSide(Vector3Int blockPosition)
    {
        verticals.Add(new Vector3(0, 0, 0) + blockPosition);
        verticals.Add(new Vector3(0, 0, 1) + blockPosition);
        verticals.Add(new Vector3(0, 1, 0) + blockPosition);
        verticals.Add(new Vector3(0, 1, 1) + blockPosition);

        AddLastVerticalSquare();
    }
    
    private void GenerateFrontSide(Vector3Int blockPosition)
    {
        verticals.Add(new Vector3(0, 0, 1) + blockPosition);
        verticals.Add(new Vector3(1, 0, 1) + blockPosition);
        verticals.Add(new Vector3(0, 1, 1) + blockPosition);
        verticals.Add(new Vector3(1, 1, 1) + blockPosition);

        AddLastVerticalSquare();
    }
    
    private void GenerateBackSide(Vector3Int blockPosition)
    {
        verticals.Add(new Vector3(0, 0, 0) + blockPosition);
        verticals.Add(new Vector3(0, 1, 0) + blockPosition);
        verticals.Add(new Vector3(1, 0, 0) + blockPosition);
        verticals.Add(new Vector3(1, 1, 0) + blockPosition);

        AddLastVerticalSquare();
    }
    
    private void GenerateTopSide(Vector3Int blockPosition)
    {
        verticals.Add(new Vector3(0, 1, 0) + blockPosition);
        verticals.Add(new Vector3(0, 1, 1) + blockPosition);
        verticals.Add(new Vector3(1, 1, 0) + blockPosition);
        verticals.Add(new Vector3(1, 1, 1) + blockPosition);

        AddLastVerticalSquare();
    }
    
    private void GenerateBottomSide(Vector3Int blockPosition)
    {
        verticals.Add(new Vector3(0, 0, 0) + blockPosition);
        verticals.Add(new Vector3(1, 0, 0) + blockPosition);
        verticals.Add(new Vector3(0, 0, 1) + blockPosition);
        verticals.Add(new Vector3(1, 0, 1) + blockPosition);

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