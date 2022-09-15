using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerList : NetworkBehaviour
{
    [SyncVar]
    public List<PlayerScript> players;

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    [Command(requiresAuthority = false)]
    public void CmdAddPlayers(PlayerScript player)
    {
        RpcAddPlayers(player);
    }

    [ClientRpc]
    public void RpcAddPlayers(PlayerScript player)
    {
        players.Add(player);
    }
}

