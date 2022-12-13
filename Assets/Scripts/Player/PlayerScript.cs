using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

//CLIENT SCRIPT
public class PlayerScript : NetworkBehaviour
{

    //Fields
    public List<CardScript> playerHand;

    public List<CardScript> playerDeck;

    public GameState FSM;

    public GameObject[] GUI;

    public bool added = false;

    [SyncVar]
    public PlayerList playerList;

    [SyncVar]
    public int playerNumber;

    [SyncVar]
    public bool deckGenerated;

    [SyncVar]
    public bool ready;
    //Properties
    //Methods
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    public void Start()
    {
        //CmdGetScripts();
        playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
        FSM = GameObject.Find("FSM").GetComponent<GameState>();
        CmdSetPlayer(this);
    }

    public void Update()
    {

        //if (isServer && playerList.players.Count == 4)
        //{
        //    CmdSendPlayers(playerList.players);
        //    Debug.Log("It shoudl be working");
        //}

        if (GameObject.Find("13") != null && !added && isServer)
        {
            CmdSendPlayers(this);
            added = true;
        }

        if (!isLocalPlayer)
            return;
        if (FSM == null)
            return;

        switch (FSM.CurrentState)
        {
            case GameStates.Setup:

                break;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSendPlayers(PlayerScript player)
    {
        playerList.CmdAddPlayers(player);
    }

    [Command(requiresAuthority = false)]
    public void CmdSetPlayer(PlayerScript player)
    {
        RpcSetPlayer();
        //if (!playerList.players.Contains(player))
        //playerList.CmdAddPlayers(player);
    }

    [ClientRpc]
    public void RpcSetPlayer()
    {
        playerNumber = (int)netIdentity.netId;
        gameObject.name = $"{playerNumber}";
    }

    //[Command(requiresAuthority = false)]
    //public void CmdSendPlayers(List<PlayerScript> players)
    //{
    //    RpcRecievePlayers(players);
    //}

    //[ClientRpc]
    //public void RpcRecievePlayers(List<PlayerScript> players)
    //{
    //    this.playerList.players = players;
    //}




















    //[Command(requiresAuthority = false)]
    //public void CmdGetScripts()
    //{
    //    Debug.Log("getting scripts");
    //    if (!isServer)
    //        RpcGetScripts();
    //}

    //[ClientRpc]
    //public void RpcGetScripts()
    //{
    //    if
    //    FSM = GameObject.Find("FSM").GetComponent<GameState>();
    //    Debug.Log(FSM + " fsm");
    //    playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
    //}
}
