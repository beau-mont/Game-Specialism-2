using UnityEngine;
using UnityEngine.SceneManagement;

public class Startbutton : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
