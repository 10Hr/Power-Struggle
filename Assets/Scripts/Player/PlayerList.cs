using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerList : NetworkBehaviour
{
    public readonly SyncList<PlayerScript> players = new SyncList<PlayerScript>();

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    [Command(requiresAuthority = false)]
    public void CmdAddPlayers(PlayerScript player)
    {
        players.Add(player);
    }
}

