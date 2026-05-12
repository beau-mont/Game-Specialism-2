using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A wave factory that provides behaviour for spawning a basic wave.
/// there should only be one of these in the asset list.
/// </summary>
[CreateAssetMenu(fileName = "BaseWaveFactory", menuName = "Wave/BaseWaveFactory")]
public class ConcreteBaseWaveFactory : IWaveFactory // simple instantiation factory
{
    public override string FactoryName => "Base Wave Factory";
    public override float MaxHealthMod { get; set; }
    public override List<GameObject> CreateWave(WaveData waveData) // returns a list of all enemies spawned in this wave
    {
        if (!waveData) return null;
        List<GameObject> spawnedEnemies = new List<GameObject>();
        foreach (var spawn in waveData.enemySpawns)
        {
            GameObject temp = GameObject.Instantiate(spawn.EnemyPrefab, spawn.SpawnLocation, spawn.SpawnRotation);
            if (temp.TryGetComponent<GenericDamageable>(out var damageable)) damageable.ModifyMaxHealth(MaxHealthMod);
            spawnedEnemies.Add(temp); // replace with object pooling later
        }
        return spawnedEnemies; // return list of spawned enemies
    }
}