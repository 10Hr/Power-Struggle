using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

//CLIENT SCRIPT
public class PlayerScript : NetworkBehaviour
{

    //Fields

    //Networking attributes
    NetworkIdentity netID;
    [SyncVar]
    private int playerCount;
    private int playerNum;

    //Player attributes
    private int charisma = 0;
    private int cunning = 0;
    private int intelligence = 0;
    private int strength = 0;
    private string highest;
    private int availablePoints = 8;
    private int maxPoints = 8;
    private bool readied = false;
    [SyncVar]
    string myName;

    //player belongings
    public bool hasDeck = false;
    //[SyncVar]
    GameObject cardReplacement;

    //data structures
    public List<GameObject> buttons;
    public GameObject[] childButtons;
    public GameObject[] player1GUI;
    public GameObject[] player2GUI;
    public GameObject[] player3GUI;
    public GameObject[] player4GUI;
    public int handCount = 0;
    //public List<GameObject> hand = new List<GameObject>();
    //private List<GameObject> realHand = new List<GameObject>();

    //Other Objects
    PlayerManager playerManager;
    StatManager stats;
    GameState gameManager;
    GameStates currentState;
    GameObject camera1;
    GameObject camera2;
    GameObject camera3;
    GameObject camera4;
    GameObject canvas;


    //Stat Properties
    public int Charisma
    {
        get { return charisma; }
        set { charisma = value; }
    }
    public int Cunning
    {
        get { return cunning; }
        set { cunning = value; }
    }
    public int Intelligence
    {
        get { return intelligence; }
        set { intelligence = value; }
    }
    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }

    //highest stat peroperty
    public string Highest
    {
        get { return highest; }
    }

    //stat check propeties
    public int Available
    {
        get { return availablePoints; }
        set { availablePoints = value; }
    }
    public int Max
    {
        get { return maxPoints; }
        set { maxPoints = value; }
    }

    public void getCount() { playerCount++; }
    //before start

    //initializations
    void Awake()
    {
        //get net ID and send it to player manager
        netID = netIdentity; // runs
        playerCount = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().AddNet(netID);
        playerNum = playerCount;

        myName = "player " + playerCount;
        this.name = myName;

        //add all increment and decrement buttons to list
        player1GUI = GameObject.FindGameObjectsWithTag("Player1");
        player2GUI = GameObject.FindGameObjectsWithTag("Player2");
        player3GUI = GameObject.FindGameObjectsWithTag("Player3");
        player4GUI = GameObject.FindGameObjectsWithTag("Player4");

        camera1 = GameObject.Find("playerCamera1");
        camera2 = GameObject.Find("playerCamera2");
        camera3 = GameObject.Find("playerCamera3");
        camera4 = GameObject.Find("playerCamera4");

        canvas = GameObject.FindGameObjectWithTag("MainCanvas");

        gameManager = GameObject.Find("FSM").GetComponent<GameState>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    // Start is called before the first frame update
    //assign camera
    //remove other players GUI
    void Start()
    {

        if (isLocalPlayer)
        {
            //assign cameras
            //GameObject.Find("Main Camera").SetActive(false);
            camera1.SetActive(false);
            camera2.SetActive(false);
            camera3.SetActive(false);
            camera4.SetActive(false);

            switch (playerCount)
            {
                case 1:
                    //camera setup
                    camera1.SetActive(true);
                    canvas.GetComponent<Canvas>().worldCamera = camera1.GetComponent<Camera>();

                    //gui setup
                    foreach (GameObject g in player2GUI)
                        g.SetActive(false);
                    foreach (GameObject g in player3GUI)
                        g.SetActive(false);
                    foreach (GameObject g in player4GUI)
                        g.SetActive(false);

                    //get this players add/sub buttons
                    childButtons = GameObject.FindGameObjectsWithTag("Change");
                    foreach (GameObject c in childButtons)
                        buttons.Add(c.transform.parent.gameObject);

                    break;
                case 2:
                    camera2.SetActive(true);
                    canvas.GetComponent<Canvas>().worldCamera = camera2.GetComponent<Camera>();

                    foreach (GameObject g in player1GUI)
                        g.SetActive(false);
                    foreach (GameObject g in player3GUI)
                        g.SetActive(false);
                    foreach (GameObject g in player4GUI)
                        g.SetActive(false);

                    childButtons = GameObject.FindGameObjectsWithTag("Change");
                    foreach (GameObject c in childButtons)
                        buttons.Add(c.transform.parent.gameObject);
                    break;
                case 3:
                    camera3.SetActive(true);
                    canvas.GetComponent<Canvas>().worldCamera = camera3.GetComponent<Camera>();

                    foreach (GameObject g in player1GUI)
                        g.SetActive(false);
                    foreach (GameObject g in player2GUI)
                        g.SetActive(false);
                    foreach (GameObject g in player4GUI)
                        g.SetActive(false);

                    childButtons = GameObject.FindGameObjectsWithTag("Change");
                    foreach (GameObject c in childButtons)
                        buttons.Add(c.transform.parent.gameObject);
                    break;
                case 4:
                    camera4.SetActive(true);
                    canvas.GetComponent<Canvas>().worldCamera = camera4.GetComponent<Camera>();

                    foreach (GameObject g in player1GUI)
                        g.SetActive(false);
                    foreach (GameObject g in player2GUI)
                        g.SetActive(false);
                    foreach (GameObject g in player3GUI)
                        g.SetActive(false);

                    childButtons = GameObject.FindGameObjectsWithTag("Change");
                    foreach (GameObject c in childButtons)
                        buttons.Add(c.transform.parent.gameObject);
                    break;

                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    //FSM CODE
    void Update()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        currentState = gameManager.CurrentState;

        switch (currentState)
        {
            //setup stage of the game
            //Get your deck and draw your cards
            case GameStates.Setup:
                //Repeat all of this in turn
                if (readied && !hasDeck)
                {
                    //Debug.Log("I want to get my deck");
                    highest = FindHighestStat();
                    CmdSpawnDeck(highest, playerNum);
                    hasDeck = true;

                    break;
                }
                if (hasDeck && handCount < 8)
                {
                    handCount++;
                    //Debug.Log("I want to draw");
                    CmdDraw(playerCount);
                }
                if (availablePoints == 0 && readied)
                {
                    foreach (GameObject b in buttons)
                    {
                        b.SetActive(false);
                    }
                }
                else
                {
                    foreach (GameObject b in buttons)
                    {
                        b.SetActive(true);
                    }
                }
                break;
            case GameStates.Passive:
                
                break;
            default:
                break;
        }

        if(handCount > 0)
        CmdEnlarge();

    }

    //Finds players highest stat
    public string FindHighestStat()
    {

        if (cunning > charisma && cunning > intelligence && cunning > strength)
        {
            return "cunning";
        }
        else if (charisma > cunning && charisma > intelligence && charisma > strength)
        {
            return "charisma";
        }
        else if (strength > charisma && strength > intelligence && strength > cunning)
        {
            return "strength";
        }
        else
        {
            return "intelligence";
        }
    }

    //Create players deck
    //Create players hand
    //spawn hand in game
    [Command]
    public void CmdDraw(int playerCount)
    {
        playerManager.HandMaker(playerCount, connectionToClient);
    }

    //sets player up for passive phase
    public bool ReadyUp() {
        //Debug.Log("I am READY!");

        if (availablePoints == 0)
        {
            readied = true;
            return true;
        }
        else
            return false;
    }

    //if weird error try playerCount
    [Command]
    public void CmdSpawnDeck(string highest, int playerNum) {
        playerManager.DeckMaker(highest, playerNum);

    }

    [Command]
    public void CmdEnlarge()
    {
        playerManager.Enlarge();
    }
}
