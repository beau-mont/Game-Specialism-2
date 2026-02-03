using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    IWaveFactory waveFactory;
    [SerializeField]
    public List<Wave> waves;
    public int waveIndex = 0; // tempoary variable to control what wave to spawn, later will be controlled by game logic

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space)) // press space to spawn next wave (DEBUG)
        {
            if (waveIndex >= waves.Count) waveIndex = 0; // loop back to first wave
            SpawnWave(waves[waveIndex].waveData, waves[waveIndex].factoryType);
            waveIndex++;
            GameObject proj = ProjectilePool.instance.GetPooledObjectOfType(ProjectileType.Basic); // test object pooling
            proj.transform.position = Vector3.zero;
            proj.SetActive(true);
        }
        #endif
    }

    public void SpawnWave(WaveData waveData, string factoryType)
    {
        if (!waveData || waveData.enemySpawns.Length == 0) 
        {
            Debug.Log("Invalid/Null wave data provided.");
            return; 
        }
        if (!waveFactory || waveFactory.FactoryName != factoryType) 
        {
            Destroy(waveFactory);
            Debug.Log("destroyed previous factory instance.");
            waveFactory = FactoryController.GetFactory(factoryType); // reset factory if type has changed
        }
        Debug.Log($"Using factory: {waveFactory.FactoryName} to spawn wave: {waveData.WaveName}");
        var enemies = waveFactory.CreateWave(waveData);
        Debug.Log($"Spawned {enemies.Count} enemies for wave: {waveData.WaveName}");
    }

    [System.Serializable]
    public class Wave
    {
        public WaveData waveData;
        public string factoryType = "Base Wave Factory";
    }
}
