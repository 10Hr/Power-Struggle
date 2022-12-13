using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerList : NetworkBehaviour
{
    public SyncList<PlayerScript> players = new SyncList<PlayerScript>();

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    [Command(requiresAuthority = false)]
    public void CmdAddPlayers(PlayerScript player)
    {
        players.Add(player);
        //for (int i = 0; i < 4; i++)
        //{
        //    if (players[i] == null)
        //    {
        //        players[i] = player;
        //    }
        //}
    }

    //[ClientRpc]
    //public void RpcAddPlayers(PlayerScript player)
    //{
        
    //}
}

