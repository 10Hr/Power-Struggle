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

    [Command(requiresAuthority = false)]
    public void CmdAddPlayers(PlayerScript player)
    {
        players.Add(player);
    }
}

