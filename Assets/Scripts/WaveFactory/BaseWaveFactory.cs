using UnityEngine;
using System.Collections.Generic;

public class ConcreteBaseWaveFactory : IWaveFactory // simple instantiation factory
{
    public override string FactoryName => "Base Wave Factory";
    public override List<GameObject> CreateWave(WaveData waveData) // returns a list of all enemies spawned in this wave
    {
        if (!waveData) return null;
        List<GameObject> spawnedEnemies = new List<GameObject>();
        foreach (var spawn in waveData.enemySpawns)
        {
            spawnedEnemies.Add(GameObject.Instantiate(spawn.EnemyPrefab, spawn.SpawnLocation, spawn.SpawnRotation)); // replace with object pooling later
        }
        return spawnedEnemies; // return list of spawned enemies
    }
}