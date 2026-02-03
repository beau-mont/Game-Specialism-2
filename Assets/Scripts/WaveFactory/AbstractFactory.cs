using System.Collections.Generic;
using UnityEngine;

public abstract class IWaveFactory : ScriptableObject // abstract factory class
{
    public abstract List<GameObject> CreateWave(WaveData waveData);
    public abstract string FactoryName { get; }
}

[CreateAssetMenu(fileName = "New Wave", menuName = "ScriptableObjects/Waves"), System.Serializable]
public class WaveData : ScriptableObject  // wave data container
{
    public string WaveName;
    public EnemySpawn[] enemySpawns;

    [System.Serializable]
    public class EnemySpawn // enemy spawn data container
    {
        public Vector3 SpawnLocation;
        public Quaternion SpawnRotation;
        public GameObject EnemyPrefab;
    }
}