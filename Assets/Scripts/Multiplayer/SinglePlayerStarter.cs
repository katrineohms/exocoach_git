using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerStarter : MonoBehaviour
{
    // Add any variables or references you need here

    public void StartSinglePlayer()
    {
        // Access the NetworkDebugStart script and call its StartSinglePlayer method
        NetworkDebugStart networkDebugStart = FindObjectOfType<NetworkDebugStart>();
        if (networkDebugStart != null)
        {
            networkDebugStart.StartSinglePlayer();
        }
        else
        {
            Debug.LogError("NetworkDebugStart script not found in the scene.");
        }
    }

}