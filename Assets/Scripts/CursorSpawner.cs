using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnHost : NetworkBehaviour
{
    [SerializeField] NetworkObject player;
 
    [Networked] private NetworkBool spawned { get; set; }

    private bool done;
    private NetworkObject hostPlayer;

    private void Update()
    {
        if (Runner != null && !done)
        {
            Spawn();
            spawned = true;
        }
    }

    public void Spawn()
    {
        //if (Runner.IsServer)
        PlayerNetworkController detectPlayer = FindObjectOfType<PlayerNetworkController>();
        if (Runner != null && detectPlayer == null && !spawned)
        {
            hostPlayer = Runner.Spawn(player, Vector3.zero, Quaternion.identity, Runner.LocalPlayer, onBeforeSpawned: null);
            spawned = true;
        }
        if (detectPlayer != null && spawned)
        {
            done = true;
        }
    }

    public void SpawnClient(PlayerRef pRef)
    {
        Runner.Despawn(hostPlayer);

        PlayerController currentPlayer = FindObjectOfType<PlayerController>();
        currentPlayer.gameObject.SetActive(false);

        //Runner.Spawn(player, Vector3.zero, Quaternion.identity, pRef, onBeforeSpawned: null);
    }
}
