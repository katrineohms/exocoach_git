using System;
using UnityEngine;
using Fusion;
using System.Collections.Generic;
using UnityEngine.UI;



public class MultiplayerManager : MonoBehaviour
{
    public enum Stage
    {
        Disconnected,
        StartingUp,
        UnloadOriginalScene,
        ConnectingServer,
        ConnectingClients,
        AllConnected,
    }

    public Stage CurrentStage { get; internal set; }

    protected NetworkDebugStart EnsureNetworkDebugStartExists()
    {
        NetworkDebugStart networkDebugStart = FindObjectOfType<NetworkDebugStart>();
        if (networkDebugStart)
        {
            if (networkDebugStart.gameObject == gameObject)
                return networkDebugStart;
        }

        return networkDebugStart;
        /*if (TryGetBehaviour<NetworkDebugStart>(out var found))
        {
            networkDebugStart = found;
            return found;
        }

        networkDebugStart = AddBehaviour<NetworkDebugStart>();
        return networkDebugStart;*/
    }




    public void StartHost()
    {
        NetworkDebugStart networkDebugStart = FindObjectOfType<NetworkDebugStart>();
        networkDebugStart.StartHost();

    }

    public void StartClient()
    {
        var nds = EnsureNetworkDebugStartExists();
        NetworkDebugStart networkDebugStart = FindObjectOfType<NetworkDebugStart>();
        // Implement your own logic to provide connection information (e.g., IP address, port)
        //string serverIPAddress = "127.0.0.1"; // Example IP address
        //int serverPort = 7777; // Example port number

        // Start client and connect to the specified server
        //networkDebugStart.StartClient(serverIPAddress, serverPort);
        networkDebugStart.StartClient();
    }
}
