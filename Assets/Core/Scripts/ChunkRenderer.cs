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
    
    private void GenerateBlock(int x, int y, int z)
    {
        if (Blocks[x, y, z] == 0) 
            return;

        Vector3Int blockPosition = new Vector3Int(x, y, z);
            
        GenerateRightSide(new Vector3Int(x, y, z));
        GenerateLeftSide(new Vector3Int(x, y, z));
    }
    
    private void GenerateRightSide(Vector3Int blockPosition)
    {
        verticals.Add(new Vector3(0, 0, 0) + blockPosition);
        verticals.Add(new Vector3(0, 1, 0) + blockPosition);
        verticals.Add(new Vector3(0, 0, 1) + blockPosition);
        verticals.Add(new Vector3(0, 1, 1) + blockPosition);
        
        AddLastVerticalSquare();
    }

    private void GenerateLeftSide(Vector3Int blockPosition)
    {
        verticals.Add(new Vector3(1, 0, 0) + blockPosition);
        verticals.Add(new Vector3(1, 0, 1) + blockPosition);
        verticals.Add(new Vector3(1, 1, 0) + blockPosition);
        verticals.Add(new Vector3(1, 1, 1) + blockPosition);

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