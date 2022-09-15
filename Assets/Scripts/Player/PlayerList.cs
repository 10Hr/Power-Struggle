using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayersList : NetworkBehaviour
{
    public GameObject[] players;

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    [ClientRpc]
    public void FindPlayersByTag()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
}

