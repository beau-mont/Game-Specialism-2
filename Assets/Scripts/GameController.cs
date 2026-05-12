using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

/// <summary>
/// A basic game controller script for managing game state and whatnot
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField] public List<Wave> waves;
    [SerializeField]private List<GameObject> activeEnemies = new();
    public int waveIndex = 0; // temporary variable to control what wave to spawn, later will be controlled by game logic
    public PlayerData playerData;
    public GameObject canvas;   
    public GameObject gameOverPrefab;

    void Start()
    {
        PlayerEventController eventController = FindFirstObjectByType<PlayerEventController>();
        if (eventController != null)
        {
            eventController.OnKill += CheckWaveCompletion;
        }
        //var sceneController = FindFirstObjectByType<SceneController>();
        //if (sceneController) waveIndex = sceneController.waveIndex;
        DisplayWaveName(waves[waveIndex]);
        StartCoroutine(WaveEnumerator(waves[waveIndex]));
    }

    void CheckWaveCompletion()
    {
        activeEnemies.RemoveAll(a => a == null || !a.activeInHierarchy);
        if (activeEnemies.Count == 0 && waveIndex < waves.Count && waves[waveIndex].SpawnedWave) // temporary win condition, later will be controlled by game logic
        {
            waveIndex++;
            DisplayWaveName(waves[waveIndex]);
            StartCoroutine(WaveEnumerator(waves[waveIndex]));
            var sceneController = FindFirstObjectByType<SceneController>();
            if (sceneController) sceneController.waveIndex = waveIndex;
        }
    }

    void DisplayWaveName(Wave wave)
    {
        if (wave.DisplayPrefab != null && this.isActiveAndEnabled)
        {
            GameObject displayInstance = Instantiate(wave.DisplayPrefab, canvas.transform);
            displayInstance.GetComponent<TextMeshProUGUI>().text = wave.waveData.WaveName;
            displayInstance.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"No display prefab assigned for wave {wave.waveData.name}");
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
        wave.factory.MaxHealthMod = playerData.MaxHealthMod; // set max health mod for enemies
        List<GameObject> newEnemies = wave.factory.CreateWave(wave.waveData);
        newEnemies.ForEach(enemy => enemy.SetActive(true)); // ensure all newly spawned enemies are active (second foreach loop afterwards is slower but fuck you)
        activeEnemies.AddRange(newEnemies);
        wave.SpawnedWave = true;
    }

    private IEnumerator WaveEnumerator(Wave wave)
    {
        if (wave.SpawnDelay > 0)
        {
            yield return new WaitForSeconds(wave.SpawnDelay);
        }
        SpawnWave(wave);
        yield break;
    }

    public void OpenMenu(float waitTime)
    {
        Instantiate(gameOverPrefab, new Vector3(0f, 10f, 0f), quaternion.identity);
        StartCoroutine(OpenMenuCoroutine(waitTime));
    }

    private IEnumerator OpenMenuCoroutine(float waitTime)
    {

        if (waitTime > 0)
        {
            yield return new WaitForSeconds(waitTime);
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield break;
    }
    
    public void Respawn(GameObject player)
    {
        StartCoroutine(RespawnCoroutine(player));
    }

    private IEnumerator RespawnCoroutine(GameObject player)
    {
        yield return new WaitForSeconds(1f);

        player.GetComponent<PlayerController>().lives--;
        player.transform.position = new Vector3(0, -4f, 0);
        player.SetActive(true);

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
    public float SpawnDelay;
    public bool SpawnedWave;
}