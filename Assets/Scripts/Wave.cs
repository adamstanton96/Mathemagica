using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public Wave(Enemy wavePrefab, int maxSpawnCount, float spawnTime)
    {
        this.wavePrefab = wavePrefab;
        this.maxSpawnCount = maxSpawnCount;
        this.spawnTime = spawnTime;
    }

    public Enemy wavePrefab;
    public int maxSpawnCount;
    public float spawnTime;
}
