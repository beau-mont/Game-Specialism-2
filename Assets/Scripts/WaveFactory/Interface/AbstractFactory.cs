using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract factory class
/// </summary>
public abstract class IWaveFactory : ScriptableObject // abstract factory class
{
    public abstract List<GameObject> CreateWave(WaveData waveData);
    public abstract string FactoryName { get; }
}

/// <summary>
/// The wave data container, these store only enemies and the name of the wave.
/// determining what events should happen when a wave begins is done by a concrete factory.
/// </summary>
[CreateAssetMenu(fileName = "New Wave", menuName = "Enemy Wave"), System.Serializable]
public class WaveData : ScriptableObject  // wave data container
{
    public string WaveName;
    public EnemySpawn[] enemySpawns;

    /// <summary>
    /// A data container storing a spawn location, rotation and prefab for an enemy to be created in.
    /// </summary>
    [System.Serializable]
    public class EnemySpawn // enemy spawn data container
    {
        public Vector3 SpawnLocation;
        public Quaternion SpawnRotation;
        public GameObject EnemyPrefab;
    }
}