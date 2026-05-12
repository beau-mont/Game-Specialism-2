using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public int waveIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator LoadScene(string loadScene)
    {
        //if (loadScene != null) 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
