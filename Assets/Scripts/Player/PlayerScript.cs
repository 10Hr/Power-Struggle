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

        if (!isLocalPlayer)
            return;

        //GUI = GameObject.FindGameObjectsWithTag("Player1");
        //foreach (GameObject g in GUI)
        //{
        //    g.SetActive(false);
        //}

        CmdGetScripts();
   
    }

    public void Start()
    {
        CmdSetPlayer();
    }

    public void Update()
    {
        if (!isLocalPlayer)
            return;

        switch(FSM.CurrentState)
        {
            case GameStates.Setup:
                
                break;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSetPlayer()
    {
        RpcSetPlayer();
        playerList.CmdAddPlayers(this);
    }

    [ClientRpc]
    public void RpcSetPlayer()
    {
        playerNumber = (int)netIdentity.netId;
        gameObject.name = $"{playerNumber}";
    }

    [Command(requiresAuthority = false)]
    public void CmdGetScripts()
    {
        RpcGetScripts();
    }

    [ClientRpc]
    public void RpcGetScripts()
    {
        FSM = GameObject.Find("FSM").GetComponent<GameState>();
        playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
    }
}
