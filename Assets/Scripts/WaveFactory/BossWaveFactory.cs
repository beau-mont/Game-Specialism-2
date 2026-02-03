using UnityEngine;
using System.Collections.Generic;

public class ConcreteBossFactory : IWaveFactory // specialized factory for boss waves
{
    public override string FactoryName => "Boss Wave Factory";
    public override List<GameObject> CreateWave(WaveData waveData)
    {
        if (!waveData) return null;
        List<GameObject> spawnedEnemies = new List<GameObject>();
        foreach (var spawn in waveData.enemySpawns)
        {
            spawnedEnemies.Add(GameObject.Instantiate(spawn.EnemyPrefab, spawn.SpawnLocation + Vector3.up, spawn.SpawnRotation));
        }
        // Additional logic for boss waves will be added here
        return spawnedEnemies;
    }
}