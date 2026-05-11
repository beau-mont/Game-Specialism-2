using System.Collections;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public GameObject Rorby;
    public GameObject Canvas;
    public void Exit()
    {
        if (Rorby && Canvas) 
        {
            Rorby.SetActive(true);
            Rorby.GetComponent<Animator>().enabled = false;
            Rorby.transform.eulerAngles = new Vector3(0f, -20f, 0f);
            Canvas.SetActive(false);
        }

        Application.Quit();
        #if UNITY_EDITOR
        Debug.Log("Quitting Application");
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
