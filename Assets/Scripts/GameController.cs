using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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
            StartCoroutine(WaveEnumerator(waves[waveIndex]));
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
        List<GameObject> newEnemies = wave.factory.CreateWave(wave.waveData);
        newEnemies.ForEach(enemy => enemy.SetActive(true)); // ensure all newly spawned enemies are active (second foreach loop afterwards is slower but fuck you)
        activeEnemies.AddRange(newEnemies);
    }

    private IEnumerator WaveEnumerator(Wave wave)
    {
        if (wave.NameDisplayTime > 0)
        {
            // Display wave name
            yield return new WaitForSeconds(wave.NameDisplayTime);
        }
        SpawnWave(wave);
        yield break;
    }
}

/// <summary>
/// A custom class for storing wave data as a designated factory and the wave data to go along with it.
/// </summary>
[System.Serializable]
public class Wave
{
    public WaveData waveData;
    public IWaveFactory factory;
    public GameObject DisplayPrefab;
    public float NameDisplayTime;
}