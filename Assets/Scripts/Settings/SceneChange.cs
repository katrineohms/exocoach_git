using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;


public class SceneChange : MonoBehaviour//, INetworkRunnerCallbacks
{
    public int sceneToLoad;
    public SceneRef fusionSceneRef;
    private NetworkRunner runner;
    private NetworkSceneManagerDefault sceneManager;

    public void goToScene()
    {
        fusionSceneRef = sceneToLoad;
        NetworkRunner runner = GameObject.FindObjectOfType<NetworkRunner>();
        NetworkSceneManagerDefault sceneManager = GameObject.FindObjectOfType<NetworkSceneManagerDefault>();

        Debug.Log(sceneToLoad + fusionSceneRef);
        //currentScene = sceneToLoad; 
        runner.SetActiveScene(fusionSceneRef);

    }

    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
