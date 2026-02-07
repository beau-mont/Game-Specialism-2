using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A wave factory that provides behaviour for spawning a boss wave.
/// there should only be one of these in the asset list.
/// </summary>
[CreateAssetMenu(fileName = "BaseWaveFactory", menuName = "Wave/BossWaveFactory")]
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