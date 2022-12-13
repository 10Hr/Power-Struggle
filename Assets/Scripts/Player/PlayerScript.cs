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
        playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
        FSM = GameObject.Find("FSM").GetComponent<GameState>();
        CmdSetPlayer(this);
    }

    public void Update()
    {
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
    }

    [ClientRpc]
    public void RpcSetPlayer()
    {
        playerNumber = (int)netIdentity.netId;
        gameObject.name = $"{playerNumber}";
    }
}
