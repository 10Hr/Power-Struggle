using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

//CLIENT SCRIPT
public class PlayerScript : NetworkBehaviour
{

    ////Fields

    ////Networking attributes
    //NetworkIdentity netID;
    //[SyncVar]
    //private int playerCount;
    //public int playerNum;

    ////Player attributes
    //[SyncVar]
    //private int charisma = 0;
    //[SyncVar]
    //private int cunning = 0;
    //[SyncVar]
    //private int intelligence = 0;
    //[SyncVar]
    //private int strength = 0;
    //private string highest;
    //[SyncVar]
    //public int availablePoints = 8;
    //private int maxPoints = 8;
    //public bool readied = false;
    //private bool roatated = false;
    //public int numSelected = 0;
    //[SyncVar]
    //string myName;

    ////player belongings
    //public bool hasDeck = false;
    //public bool called = false;

    ////data structures
    //public List<GameObject> buttons;
    //public GameObject[] childButtons;
    //public GameObject[] player1GUI;
    //public GameObject[] player2GUI;
    //public GameObject[] player3GUI;
    //public GameObject[] player4GUI;

    //public int handCount = 0;
    //public List<GameObject> hand = new List<GameObject>();
    //public List<GameObject> selectedCards = new List<GameObject>();


    ////Other Objects
    //PlayerManager playerManager;
    //PassiveManager passiveManager;
    //GameState gameManager;
    //StatManager statManager;
    //GameStates currentState;
    //GameObject camera1;
    //GameObject camera2;
    //GameObject camera3;
    //GameObject camera4;
    //List<GameObject> cameraList = new List<GameObject>();
    //Canvas canvas;
    //GameObject objectPivot;
    //GameObject cardPivot;
    //public GameObject lockInButton;
    //GameObject txtChoice1;
    //GameObject txtChoice2;
    //GameObject txtChoice3;
    //GameObject bntChoice1;
    //GameObject bntChoice2;
    //GameObject bntChoice3;
    //Passive passive = new Passive();
    //public bool lockedIn = false;

    //Stat Properties
    //public int Charisma
    //{
    //    get { return charisma; }
    //    set { charisma = value; }
    //}
    //public int Cunning
    //{
    //    get { return cunning; }
    //    set { cunning = value; }
    //}
    //public int Intelligence
    //{
    //    get { return intelligence; }
    //    set { intelligence = value; }
    //}
    //public int Strength
    //{
    //    get { return strength; }
    //    set { strength = value; }
    //}

    ////highest stat peroperty
    //public string Highest
    //{
    //    get { return highest; }
    //}

    ////stat check propeties
    //public int Available
    //{
    //    get { return availablePoints;}
    //    set { availablePoints = value; }
    //}
    //public int Max
    //{
    //    get { return maxPoints; }
    //    set { maxPoints = value; }
    //}
    ////player passive 
    //public Passive Passive
    //{
    //    get { return passive; }
    //    set { passive = value; }
    //}

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
        CmdSetPlayer();
    }

    public void Update()
    {
        if (!isLocalPlayer)
            return;

        switch(FSM.currentState)
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
