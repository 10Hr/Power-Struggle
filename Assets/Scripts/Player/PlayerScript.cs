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
    [SyncVar]
    private int charisma;
    [SyncVar]
    private int strength;
    [SyncVar]
    private int intelligence;
    [SyncVar]
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

    private GameObject[] addButtons;
    private GameObject[] subButtons;

    private int maxPoints = 8;
    [SyncVar]
    private int availablePoints = 8;

    public int AvailablePoints
    {
        get { return availablePoints; }
        set { availablePoints += value; }
    }
    #endregion

    private GameObject readyButton;

    [SyncVar]
    public PlayerList playerList;

    [SyncVar]
    public int playerNumber;

    [SyncVar]
    public bool ready;

    [SyncVar]
    public string highest;

    [SyncVar]
    public bool hasHighest = false;

    [SyncVar]
    public bool hasDeck = false;

    [SyncVar]
    public bool hasPassive = false;

    [SyncVar]
    public bool cardSlotsSpawned = false;

    public DeckScript deck;

    public PassiveManager passiveManager;

    [SyncVar]
    public Passive passive;

    [SyncVar]
    public string passiveName;

    public PlayerScript[] playerCheck;

    public GameObject[] cardSlots;

    int g = 0;
    public SyncList<string[]> cards = new SyncList<string[]>();
    public SyncList<string[]> hand = new SyncList<string[]>();
    public SyncList<Passive> choicesList = new SyncList<Passive>();


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

        passive = gameObject.GetComponent<Passive>();

        charismaText = GameObject.Find("CharismaCounter").GetComponent<Text>();
        strengthText = GameObject.Find("StrengthCounter").GetComponent<Text>();
        intelligenceText = GameObject.Find("IntelligenceCounter").GetComponent<Text>();
        cunningText = GameObject.Find("CunningCounter").GetComponent<Text>();
        addButtons = GameObject.FindGameObjectsWithTag("add");
        subButtons = GameObject.FindGameObjectsWithTag("sub");

        cardSlots = GameObject.FindGameObjectsWithTag("CardSlot");

        readyButton = GameObject.Find("Ready");

        passiveManager = GameObject.Find("PassiveManager").GetComponent<PassiveManager>();
    }

    public void Update()
    {
        if (isServer)
        {
            playerCheck = GameObject.FindObjectsOfType<PlayerScript>();
        }
        //START ONCE ALL PLAYERS JOIN
        if (playerCheck.Length == 4 && !added && isServer)
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
            case GameStates.Passive:
                //calculate players highest stat
                CmdCalcHighest(this);

                //create deck and get cards data
                if (!hasDeck && hasHighest)
                {
                    hasDeck = true;
                    deck = new DeckScript();
                    deck.CreateDeck(highest);
                    if (deck.cardData.Count > 7)
                    {
                        CmdFillDeck(deck.cardData, this);
                    }
                }

                //spawn passive choices
                if (g == 0 && hasHighest) {
                    passiveManager.CmdSelectPassive(highest, this);
                    CmdSelectedPassive(this);
                    g = 1;
                }

                ////spawn card slots to plug in card data to
                //if (!cardSlotsSpawned && hasDeck && hasHighest)
                //{
                //    foreach (string[] s in cards)
                //    {
                        
                //    }
                //}

                break;

            case GameStates.Turn:
                CmdDrawCard(this);
                if (hand.Count == 6)
                {
                    TransferData();
                }
                break;
        }

        #region statChanges
        //DETECT CHANGES IN STATS
        charismaText.text = "Charisma: " + charisma;
        strengthText.text = "Strength: " + strength;
        intelligenceText.text = "Intelligence: " + intelligence;
        cunningText.text = "Cunning: " + cunning;

        if (availablePoints > 0 && availablePoints < maxPoints)
        {
            foreach (GameObject b in addButtons)
            {
                b.SetActive(true);
            }
            foreach (GameObject b in subButtons)
            {
                b.SetActive(true);
            }
        }
        else if(availablePoints == maxPoints)
        {
            foreach (GameObject b in addButtons)
            {
                b.SetActive(true);
            }
            foreach (GameObject b in subButtons)
            {
                b.SetActive(false);
            }
        }
        else if (availablePoints == 0)
        {
            foreach (GameObject b in subButtons)
            {
                b.SetActive(false);
            }
            foreach (GameObject b in addButtons)
            {
                b.SetActive(false);
            }
        }
        else if (availablePoints < 0)
        {
            foreach (GameObject b in subButtons)
            {
                b.SetActive(true);
            }
            foreach (GameObject b in addButtons)
            {
                b.SetActive(false);
            }
        }
        #endregion
    }

    [Command(requiresAuthority = false)]
    public void CmdSelectedPassive(PlayerScript p)
    {
        p.hasPassive = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdDrawCard(PlayerScript p)
    {
        if (p.hand.Count < 6)
        {
            Debug.Log("Hand: " + p.gameObject.name + " " + p.hand.Count);
            Debug.Log("Deck: " + p.gameObject.name + " " + p.cards.Count);
            p.hand.Add(p.cards[0]);
            p.cards.Remove(p.cards[0]);

            //foreach (GameObject g in slots)
            //{
            //    if (g.GetComponent<CardScript>().Title == "")
            //    {
            //        g.GetComponent<CardScript>().Title = p.hand[p.hand.Count - 1][1];
            //        g.GetComponent<CardScript>().Type = p.hand[p.hand.Count - 1][0];
            //        break;
            //    }
            //}
        }
    }

    public void TransferData()
    {
        int index = 0;
        foreach (GameObject g in cardSlots)
        {
            if (g.GetComponent<CardScript>().Title == "")
            {
                g.GetComponent<CardScript>().Title = hand[index][1];
                g.GetComponent<CardScript>().Type = hand[index][0];
                break;
            }
            index++;
        }
    }

    [Command (requiresAuthority = false)]
    private void CmdCalcHighest(PlayerScript p)
    {
        int currentHigh = 0;
        if (p.charisma >= currentHigh)
        {
            currentHigh = charisma;
            p.highest = "charisma";
        }
        if (p.strength >= currentHigh)
        {
            currentHigh = strength;
            p.highest = "strength";
        }
        if (p.intelligence >= currentHigh)
        {
            currentHigh = intelligence;
            p.highest = "intelligence";
        }
        if (p.cunning >= currentHigh)
        {
            currentHigh = cunning;
            p.highest = "cunning";
        }
        p.hasHighest = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdFillDeck(List<string[]> cards, PlayerScript p)
    {
        foreach (string[] s in cards)
        {
            //p.cards.Add(c);
            //p.cards[p.cards.Count - 1].Title = c.Title;
            //p.cards[p.cards.Count - 1].Effect = c.Effect;
            //p.cards[p.cards.Count - 1].Stat = c.Stat;
            p.cards.Add(s);
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
