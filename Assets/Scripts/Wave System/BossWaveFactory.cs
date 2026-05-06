using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// A wave factory that provides behaviour for spawning a boss wave.
/// there should only be one of these in the asset list.
/// </summary>
[CreateAssetMenu(fileName = "BossWaveFactory", menuName = "Wave/BossWaveFactory")]
public class ConcreteBossFactory : IWaveFactory // specialized factory for boss waves
{
    public PooledSFX StartSound;
    public override string FactoryName => "Boss Wave Factory";
    public override List<GameObject> CreateWave(WaveData waveData)
    {
        if (!waveData) return null;
        List<GameObject> spawnedEnemies = new List<GameObject>();
        foreach (var spawn in waveData.enemySpawns)
        {
            spawnedEnemies.Add(GameObject.Instantiate(spawn.EnemyPrefab, spawn.SpawnLocation, spawn.SpawnRotation));
        }
        // Additional logic for boss waves will be added here
        var sfx = StartSound.GetPooledObject();
        sfx.SetActive(true);
        return spawnedEnemies;
    }
}