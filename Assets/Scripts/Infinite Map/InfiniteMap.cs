using System;
using UnityEngine;

public class InfiniteMap : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject mapChunkPrefab;

    [Header("Settings")]
    [SerializeField] private int mapChunkSize;

    private void Start() => GenerateMap();
    
    private void GenerateMap()
    {
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                GenerateMapChunk(x, y);
    }

    private void GenerateMapChunk(int x, int y)
    {
        Vector3 spawnPosition = new Vector3(x, y) * mapChunkSize;
        
        Instantiate(mapChunkPrefab, spawnPosition, Quaternion.identity, transform);
        
    }
}
