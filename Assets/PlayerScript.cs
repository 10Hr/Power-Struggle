using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
//CLIENT SCRIPT
public class PlayerScript : NetworkBehaviour
{
    NetworkIdentity netID;
    private GameObject deck;
    private int charisma = 0;
    private int cunning = 0;
    private int intelligence = 0;
    private int strength = 0;
    private string highest;
    public List<GameObject> hand = new List<GameObject>();
    private List<GameObject> realHand = new List<GameObject>();
    private int availablePoints = 8;
    private int maxPoints = 8;
    private bool readied = false;
    public GameObject[] buttons;
    [SyncVar]
    private int playerCount;
    StatManager stats;
    GameState gameManager;
    GameStates currentState;



    GameObject camera1;
    GameObject camera2;
    GameObject camera3;
    GameObject camera4;
    GameObject canvas;

    public int Charisma
    {
        get { return charisma; }
        set { charisma = value;}
    }
    public int Cunning
    {
        get { return cunning; }
        set { cunning = value;}
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
    public string Highest
    {
        get { return highest; }
    }
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
    void Awake()
    {
        //get net ID and send it to player manager
        netID = netIdentity; // runs
        playerCount = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().AddNet(netID);

        //add all increment and decrement buttons to list
        buttons = GameObject.FindGameObjectsWithTag("Change");

        camera1 = GameObject.Find("playerCamera1");
        camera2 = GameObject.Find("playerCamera2");
        camera3 = GameObject.Find("playerCamera3");
        camera4 = GameObject.Find("playerCamera4");

        canvas = GameObject.FindGameObjectWithTag("MainCanvas");

        gameManager = GameObject.Find("FSM").GetComponent<GameState>();
    }

    // Start is called before the first frame update
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
                    camera1.SetActive(true);
                    canvas.GetComponent<Canvas>().worldCamera = camera1.GetComponent<Camera>();
                    break;
                case 2:
                    camera2.SetActive(true);
                    canvas.GetComponent<Canvas>().worldCamera = camera2.GetComponent<Camera>();
                    break;
                case 3:
                    camera3.SetActive(true);
                    canvas.GetComponent<Canvas>().worldCamera = camera3.GetComponent<Camera>();
                    break;
                case 4:
                    camera4.SetActive(true);
                    canvas.GetComponent<Canvas>().worldCamera = camera4.GetComponent<Camera>();
                    break;
                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!isLocalPlayer)
        {
            return;
        }

        currentState = gameManager.CurrentState;

        switch (currentState)
        {
            case GameStates.Setup:
                //Repeat all of this in turn
                if ((readied && deck == null) || (readied && deck.GetComponent<DeckScript>().Type != highest))
                {
                    Debug.Log("I want to get my deck");
                    highest = FindHighestStat();
                    deck = new GameObject("deck");
                    deck.AddComponent<DeckScript>();
                    Debug.Log("My deck is " + highest);
                }
                if (deck != null && deck.GetComponent<DeckScript>().cards.Count != 0)
                {
                    Debug.Log("I want to draw");
                    while (hand.Count != 8)
                    {
                        Draw();
                    }
                    Debug.Log("I drew " + hand.Count);
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
        
    }

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

    public void Draw() {

        hand.Add(deck.GetComponent<DeckScript>().cards[0]);
        deck.GetComponent<DeckScript>().cards.RemoveAt(0);
        GameObject cardReplacement;
        
        switch (deck.GetComponent<DeckScript>().Type) {
            case "charisma":
                 cardReplacement = Instantiate(camera1.GetComponent<InstantiatePrefab>().chaPrefab);
                break;

            case "cunning":
                 cardReplacement = Instantiate(camera1.GetComponent<InstantiatePrefab>().cunPrefab);
                break;

            case "strength":
                 cardReplacement = Instantiate(camera1.GetComponent<InstantiatePrefab>().strPrefab);
                break;

            case "intelligence":
                 cardReplacement = Instantiate(camera1.GetComponent<InstantiatePrefab>().intPrefab);
                break;

            default:
                cardReplacement = null;
                break;
        }
        
        realHand.Add(cardReplacement);
        cardReplacement.AddComponent<CardScript>();
        cardReplacement.GetComponent<CardScript>().Effect = hand[hand.Count - 1].GetComponent<CardScript>().Effect;
        cardReplacement.GetComponent<CardScript>().Title = hand[hand.Count - 1].GetComponent<CardScript>().Title;
        cardReplacement.GetComponent<CardScript>().Stat = hand[hand.Count - 1].GetComponent<CardScript>().Stat;
        cardReplacement.transform.position = new Vector3(hand.Count * 2, 0, 0);
        cardReplacement.GetComponent<SpriteRenderer>().enabled = true;
        cardReplacement.GetComponent<SpriteRenderer>().sortingOrder = 1;
        cardReplacement.GetComponent<CardScript>().name = hand[hand.Count - 1].GetComponent<CardScript>().name;
        hand[hand.Count - 1].SetActive(false);
    }

    public void ReadyUp() {
        Debug.Log("I am READY!");
        readied = true;
    }

}
