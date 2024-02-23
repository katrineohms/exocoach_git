using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneToLoad;
    public void goToScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
