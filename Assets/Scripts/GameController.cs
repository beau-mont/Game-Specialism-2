using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;

/// <summary>
/// A basic game controller script for managing game state and whatnot
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField] public List<Wave> waves;
    private List<GameObject> activeEnemies = new();
    public int waveIndex = 0; // temporary variable to control what wave to spawn, later will be controlled by game logic
    public PlayerData playerData;
    public GameObject canvas;

    void OnEnable()
    {
        var sceneController = FindFirstObjectByType<SceneController>();
        if (sceneController)
            waveIndex = sceneController.waveIndex;
        StartCoroutine(WaveEnumerator(waves[waveIndex]));
    }

    // Update is called once per frame
    void Update()
    {
        // #if UNITY_EDITOR
        // if (Input.GetKeyDown(KeyCode.Space)) // temporary input to spawn waves, later will be controlled by game logic
        // {
        //     if (waveIndex < waves.Count)
        //     {
        //         SpawnWave(waves[waveIndex]);
        //         waveIndex++;
        //     }
        //     else
        //     {
        //         Debug.Log("No more waves to spawn.");
        //     }
        // }
        // #endif

        activeEnemies.RemoveAll(a => a == null || !a.activeInHierarchy);
        if (activeEnemies.Count == 0 && waveIndex < waves.Count && waves[waveIndex].SpawnedWave) // temporary win condition, later will be controlled by game logic
        {
            waveIndex++;
            var sceneController = FindFirstObjectByType<SceneController>();
        if (sceneController)
            sceneController.waveIndex = waveIndex;
            StartCoroutine(WaveEnumerator(waves[waveIndex]));
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
        wave.SpawnedWave = true;
    }

    private IEnumerator WaveEnumerator(Wave wave)
    {
        var display = Instantiate(waves[waveIndex].DisplayPrefab, canvas.transform);
        display.GetComponent<TextMeshProUGUI>().text = waves[waveIndex].DisplayName;
        display.SetActive(true);
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
    public string DisplayName;
    public bool SpawnedWave;
}