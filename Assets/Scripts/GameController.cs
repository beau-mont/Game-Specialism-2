using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A basic game controller script for managing game state and whatnot
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField] public List<Wave> waves;
    private List<GameObject> activeEnemies = new();
    public int waveIndex = 0; // temporary variable to control what wave to spawn, later will be controlled by game logic
    public PlayerData playerData;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // temporary input to spawn waves, later will be controlled by game logic
        {
            if (waveIndex < waves.Count)
            {
                SpawnWave(waves[waveIndex]);
                waveIndex++;
            }
            else
            {
                Debug.Log("No more waves to spawn.");
            }
        }
        
        activeEnemies.RemoveAll(a => a == null || !a.activeInHierarchy); 
        if (activeEnemies.Count == 0 && waveIndex < waves.Count) // temporary win condition, later will be controlled by game logic
        {
            SpawnWave(waves[waveIndex]);
            waveIndex++;
        }
    }

    public void SpawnWave(Wave wave)
    {
        if (!wave.waveData || wave.waveData.enemySpawns.Length == 0) 
        {
            Debug.Log("Invalid/Null wave data provided.");
            return; 
        }
        // if (!waveFactory || waveFactory.FactoryName != factoryType) 
        // {
        //     Destroy(waveFactory);
        //     Debug.Log("destroyed previous factory instance.");
        //     waveFactory = FactoryController.GetFactory(factoryType); // reset factory if type has changed
        // }
        
        activeEnemies.RemoveAll(a => a == null || !a.activeInHierarchy); // remove any null or inactive enemies from the list (enemies that have been destroyed)
        activeEnemies.AddRange(wave.factory.CreateWave(wave.waveData));
    }
}

/// <summary>
/// A custom class for storing wave data as a designated factory and the wave data to go along with it.
/// </summary>
[System.Serializable]
    public class Wave
    {
        public WaveData waveData;
        // public string factoryType = "Base Wave Factory";
        public IWaveFactory factory;
    }