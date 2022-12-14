using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
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

    #region stats
    private int charisma;
    private int strength;
    private int intelligence;
    private int cunning;

    public int Charisma
    {
        get { return charisma; }
        set { charisma += value; }
    }
    public int Strength
    {
        get { return strength; }
        set { strength += value; }
    }
    public int Intelligence
    {
        get { return intelligence; }
        set { intelligence += value; }
    }
    public int Cunning
    {
        get { return cunning; }
        set { cunning += value; }
    }

    public Text charismaText;
    public Text strengthText;
    public Text intelligenceText;
    public Text cunningText;
    #endregion

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

        charismaText = GameObject.Find("CharismaCounter").GetComponent<Text>();
        strengthText = GameObject.Find("StrengthCounter").GetComponent<Text>();
        intelligenceText = GameObject.Find("IntelligenceCounter").GetComponent<Text>();
        cunningText = GameObject.Find("CunningCounter").GetComponent<Text>();
    }

    public void Update()
    {
        //START ONCE ALL PLAYERS JOIN
        if (GameObject.Find("13") != null && !added && isServer)
        {
            CmdSendPlayers(this);
            added = true;
        }

        if (!isLocalPlayer)
            return;
        if (FSM == null)
            return;

        //GAMESTATE SPECIFIC EVENTS
        switch (FSM.CurrentState)
        {
            case GameStates.Setup:

                break;
        }

        //DETECT CHANGES IN STATS
        charismaText.text = "Charisma: " + charisma;
        strengthText.text = "Strength: " + strength;
        intelligenceText.text = "Intelligence: " + intelligence;
        cunningText.text = "Cunning: " + cunning;
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
