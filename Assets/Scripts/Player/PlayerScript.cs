using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Mirror;
using System;
using System.Linq;

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
    public bool untargetable = false;

    [SyncVar]
    public int powerChange = 0;

    [SyncVar]
    public bool turnTaken = false;

    [SyncVar]
    public bool cantLosePower = false;

    [SyncVar]
    public bool cantLoseStats = false;

    [SyncVar]
    public bool swapDeck = false;

    public int numSelected;

    public int numToReveal = 0;

    #region stats
    [SyncVar]
    private int charisma;
    [SyncVar]
    private int strength;
    [SyncVar]
    private int intelligence;
    [SyncVar]
    private int cunning;

    public string allyStat;

    public int Charisma
    {
        get { return charisma; }
        set {
            if (value == 0)
                charisma = value;
            charisma += value;
            if (charisma < 0)
                charisma = 0;
        }
    }
    public int Strength
    {
        get { return strength; }
        set
        {
            if (value == 0)
                strength = value;
            strength += value;
            if (strength < 0)
                strength = 0;
        }
    }
    public int Intelligence
    {
        get { return intelligence; }
        set
        {
            if (value == 0)
                intelligence = value;
            intelligence += value;
            if (intelligence < 0)
                intelligence = 0;
        }
    }
    public int Cunning
    {
        get { return cunning; }
        set
        {
            if (value == 0)
                cunning = value;
            cunning += value;
            if (cunning < 0)
                cunning = 0;
        }
    }

    public Text charismaText;
    public Text strengthText;
    public Text intelligenceText;
    public Text cunningText;

    private GameObject[] addButtons;
    private GameObject[] subButtons;

    public GameObject passiveOption1;
    public GameObject passiveOption2;
    public GameObject passiveOption3;

    [SyncVar]          
    private int maxPoints = 8;
    [SyncVar]
    private int availablePoints = 8;

    public int AvailablePoints
    {
        get { return availablePoints; }
        set { availablePoints += value; }
    }
        public int MaxPoints
    {
        get { return maxPoints; }
        set { maxPoints += value; }
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
    private string highest = "";

    [SyncVar]
    public string lowest;

    public string Highest
    {
        get { return highest; }
        set
        {
            highest = value;
        }
    }

    [SyncVar]
    public bool hasHighest = false;

    [SyncVar]
    public bool hasDeck = false;

    [SyncVar]
    public bool hasPassive = false;

    [SyncVar]
    public bool cardSlotsSpawned = false;

    [SyncVar]
    public bool cardsSpawned = false;

    [SyncVar]
    private int power = 500;

    public int Power
    {
        get { return power; }
        set { power += value; }
    }

    public DeckScript deck;

    public PassiveManager passiveManager;

    [SyncVar]
    public Passive passive;

    [SyncVar]
    public string passiveName;

    [SyncVar]
    public bool LockedIn = false;

    [SyncVar]
    public bool threeSelected;

    public PlayerScript[] playerCheck;

    public GameObject[] cardSlots;
    public GameObject[] enemySlots1;
    public GameObject[] enemySlots2;
    public GameObject[] enemySlots3;
    public PlayerScript enemy1;
    public PlayerScript enemy2;
    public PlayerScript enemy3;

    int g = 0;
    public SyncList<string[]> cards = new SyncList<string[]>();
    public SyncList<string[]> hand = new SyncList<string[]>();
    public SyncList<Passive> choicesList = new SyncList<Passive>();

    GameObject bntTop;
    GameObject bntLeft;
    GameObject bntRight;

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

        passiveOption1 = GameObject.Find("bntChoice1");
        passiveOption2 = GameObject.Find("bntChoice2");
        passiveOption3 = GameObject.Find("bntChoice3");

        charismaText = GameObject.Find("CharismaCounter").GetComponent<Text>();
        strengthText = GameObject.Find("StrengthCounter").GetComponent<Text>();
        intelligenceText = GameObject.Find("IntelligenceCounter").GetComponent<Text>();
        cunningText = GameObject.Find("CunningCounter").GetComponent<Text>();
        addButtons = GameObject.FindGameObjectsWithTag("add");
        subButtons = GameObject.FindGameObjectsWithTag("sub");

        cardSlots = GameObject.FindGameObjectsWithTag("CardSlot");
        enemySlots1 = GameObject.FindGameObjectsWithTag("1");
        enemySlots2 = GameObject.FindGameObjectsWithTag("2");
        enemySlots3 = GameObject.FindGameObjectsWithTag("3");

        readyButton = GameObject.Find("Ready");

        passiveManager = GameObject.Find("PassiveManager").GetComponent<PassiveManager>();

        bntTop = GameObject.Find("PlayerTop");
        bntLeft = GameObject.Find("PlayerLeft");
        bntRight = GameObject.Find("PlayerRight");

        if (isLocalPlayer)
        {
            bntTop.SetActive(false);
            bntLeft.SetActive(false);
            bntRight.SetActive(false);
        }

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
                if (isLocalPlayer)
                    readyButton.SetActive(false);

                //calculate players highest stat
                CalcHighest();

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
                    g = 1;
                }
                if (passive.passiveName != "" && isLocalPlayer)
                {
                    //passiveOption1.SetActive(false);
                    //passiveOption2.SetActive(false);
                    //passiveOption3.SetActive(false);
                    CmdSelectedPassive(this);
                }
                break;

            case GameStates.DrawCards:
                swapDeck = false;
                CmdDrawCard(this);
                if (hand.Count == 6 && !cardsSpawned)
                {
                    //ran = true;
                    TransferData(cardSlots, this);
                    if (cardSlots[5].GetComponent<CardScript>().Title != "")
                        CmdSpawnedCards(this);
                }

                break;

            case GameStates.LoadEnemyCards:
                TransferEnemyData();
                switch (passive.passiveName)
                {
                    case "SeeDeck":
                        break;
                    case "StrongAllies":
                        allyStat = "strength";
                        break;
                    case "SmartAllies":
                        allyStat = "intelligence";
                        break;
                    case "ShadyAllies":
                        allyStat = "cunning";
                        break;
                    default:
                        break;
                }
                numSelected = 0;
                foreach (GameObject g in cardSlots)
                {
                    if (g.GetComponent<CardScript>().selected && !g.GetComponent<CardScript>().prevSelected)
                    {
                        g.GetComponent<SpriteRenderer>().color = Color.green;
                        numSelected++;
                    } else
                        g.GetComponent<SpriteRenderer>().color = Color.white;
                }
                if (numSelected == 3)
                    CmdThreeSelected(this, true);
                else
                    CmdThreeSelected(this, false);
                break;

            case GameStates.Turn:
                if (swapDeck)
                {
                    CmdConfirm();
                    SwapDeck();
                }
                else
                {
                    CalcHighest();
                }
                UpdateData(enemySlots1, enemy1);
                UpdateData(enemySlots2, enemy2);
                UpdateData(enemySlots3, enemy3);
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
    public void CmdConfirm()
    {
        Debug.Log("Swapping Decks");
    }

    [Command(requiresAuthority = false)]
    public void CmdResetCards(PlayerScript p)
    {
        p.cards.Clear();
        p.hand.Clear();
    }

    public void SwapDeck()
    {
        deck.CreateDeck(highest);
        CmdResetCards(this);

        while (deck.cardData.Count < 40) { }

        CmdFillDeck(deck.cardData, this);

        while (cards.Count < 40) { Debug.Log(cards.Count); }

        for (int i = 0; i < 6; i++)
        {
            Debug.Log(i);
            CmdDrawCard(this);
        }
        foreach (GameObject g in cardSlots)
        {
            Debug.Log("making slot blank");
            g.GetComponent<CardScript>().Title = "";
        }
        foreach (GameObject g in cardSlots)
            TransferData(cardSlots, this);

        CmdDeckSwapped(this);
    }

    [Command (requiresAuthority = false)]
    public void CmdDeckSwapped(PlayerScript p)
    {
        if (p.passive.passiveName == "ShadyBusiness")
        {
            p.Power = 50;
        }
        Debug.Log("Deck Swapped");
        p.swapDeck = false;
    }

    [Command (requiresAuthority = false)]
    public void CmdThreeSelected(PlayerScript p, bool b)
    {
        p.threeSelected = b;
    }

    public void TransferEnemyData()
    {
        if (enemy1 == null || enemy2 == null || enemy3 == null)
        {
            foreach (PlayerScript p in playerList.players)
            {
                if (p.netId != this.netId)
                {
                    if (enemy1 == null)
                        enemy1 = p;
                    else if (enemy2 == null)
                        enemy2 = p;
                    else if (enemy3 == null)
                        enemy3 = p;
                }
            }
        }

        if (enemySlots1[5].GetComponent<CardScript>().Title == "")
        {
            TransferData(enemySlots1, enemy1);
        }
        else if (enemySlots2[5].GetComponent<CardScript>().Title == "")
        {
            TransferData(enemySlots2, enemy2);
        }
        else if (enemySlots3[5].GetComponent<CardScript>().Title == "")
        {
            TransferData(enemySlots3, enemy3);
        }
        sendPlayerData();
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawnedCards(PlayerScript p)
    {
        p.cardsSpawned = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdSelectedPassive(PlayerScript p)
    {
        p.hasPassive = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdDrawCard(PlayerScript p)
    {
        int rand = UnityEngine.Random.Range(0, p.cards.Count - 1);
        if (p.hand.Count < 6)
        {
            p.hand.Add(p.cards[rand]);
            p.cards.Remove(p.cards[rand]);
        }
    }

    public void UpdateData(GameObject[] slots, PlayerScript p)
    {
        int index = 0;
        foreach (GameObject g in slots)
        {
            if (g.GetComponent<CardScript>().Title != p.hand[index][1])
            {
                g.GetComponent<CardScript>().revealed = false;
            }
            g.GetComponent<CardScript>().Title = p.hand[index][1];
            g.GetComponent<CardScript>().Type = p.hand[index][0];
            g.GetComponent<CardScript>().ID = p.hand[index][4];
            index++;
        }
    }

    public void TransferData(GameObject[] slots, PlayerScript p)
    {
        int index = 0;
        foreach (GameObject g in slots)
        {
            //Debug.Log(p.netIdentity.netId + " " + g.GetComponent<CardScript>().Title + " " + slots.Length);
            if (g.GetComponent<CardScript>().Title == "")
            {
                g.GetComponent<CardScript>().Title = p.hand[index][1];
                g.GetComponent<CardScript>().Type = p.hand[index][0];
                g.GetComponent<CardScript>().ID = p.hand[index][4];
                break;
            }
            index++;
        }
    }

    public void CalcHighest()
    {
        string currentHighest = highest;
        string newHighest;
        int currentHigh = 0;
        switch (currentHighest)
        {
            case "strength":
                currentHigh = strength;
                break;
            case "intelligence":
                currentHigh = intelligence;
                break;
            case "charisma":
                currentHigh = charisma;
                break;
            case "cunning":
                currentHigh = cunning;
                break;
        }

        int[] statList = {strength, intelligence, charisma, cunning};
        int max = statList.Max();
        if (max > currentHigh)
        {
            if (statList[0] == max)
                newHighest = "strength";
            else if (statList[1] == max)
                newHighest = "intelligence";
            else if(statList[2] == max)
                newHighest = "charisma";
            else
                newHighest = "cunning";
            CmdSendHighest(this, newHighest);
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSendHighest(PlayerScript p, string h)
    {
        Debug.Log(p.netId + " got this far");
        p.Highest = h;
        p.swapDeck = true;
        p.hasHighest = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdFillDeck(List<string[]> cards, PlayerScript p)
    {
        foreach (string[] s in cards)
        {
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

    public List<PlayerScript> sendPlayerData() {
        List<PlayerScript> playerslots = new List<PlayerScript>();
        playerslots.Add(enemy1); //bntright
        playerslots.Add(enemy2); //bntleft
        playerslots.Add(enemy3); //bnttop

        return playerslots;
    }

    public void UnhideButtons() {
        if ((!enemy1.untargetable || (enemy1.untargetable && passive.passiveName == "Precise")) && (enemy1.passive.passiveName != "Scrapper" || (enemy1.passive.passiveName == "Scrapper"  && enemy1.Power < power))
            && !(passive.passiveName == "StrongAllies" && enemy1.Highest == "strength")
            && !(passive.passiveName == "SmartAllies" && enemy1.Highest == "intelligence")
            && !(passive.passiveName == "ShadyAllies" && enemy1.Highest == "cunning"))  //enemy1
            bntRight.SetActive(true);
        if ((!enemy1.untargetable || (enemy1.untargetable && passive.passiveName == "Precise")) && (enemy1.passive.passiveName != "Scrapper" || (enemy1.passive.passiveName == "Scrapper" && enemy1.Power < power))
            && !(passive.passiveName == "StrongAllies" && enemy1.Highest == "strength")
            && !(passive.passiveName == "SmartAllies" && enemy1.Highest == "intelligence")
            && !(passive.passiveName == "ShadyAllies" && enemy1.Highest == "cunning"))  //enemy2
                bntLeft.SetActive(true);
        if ((!enemy1.untargetable || (enemy1.untargetable && passive.passiveName == "Precise")) && (enemy1.passive.passiveName != "Scrapper" || (enemy1.passive.passiveName == "Scrapper" && enemy1.Power < power))
            && !(passive.passiveName == "StrongAllies" && enemy1.Highest == "strength")
            && !(passive.passiveName == "SmartAllies" && enemy1.Highest == "intelligence")
            && !(passive.passiveName == "ShadyAllies" && enemy1.Highest == "cunning"))  //enemy3
                bntTop.SetActive(true);
    }

    public void hideButtons() {
        if (bntRight.activeInHierarchy == true)  //enemy1
            bntRight.SetActive(false);
        if (bntLeft.activeInHierarchy == true)  //enemy2
            bntLeft.SetActive(false);
        if (bntTop.activeInHierarchy == true)  //enemy3
            bntTop.SetActive(false);
    }
    
    public void Turn() {
        //Debug.Log("My Turn " + playerNumber);

    }

        [Command(requiresAuthority = false)]
        public void ModifyStats(string type, int amount, PlayerScript p) {
        if (cantLoseStats && amount > 0)
            return;
        switch (type) {
                case "strength":
                   p.Strength = amount;
                    break;
                case "charisma":
                    p.Charisma = amount;
                    break;
                case "intelligence":
                    p.Intelligence = amount;
                    break;
                case "cunning":
                    p.Cunning = amount;
                    break;
            }
            p.MaxPoints = amount;
    }

    [Command(requiresAuthority = false)]
    public void ResetStats(PlayerScript p)
    {
        p.AvailablePoints = p.Charisma + p.Strength + p.Cunning + p.Intelligence;
        p.Strength = 0;
        p.Charisma = 0;
        p.Intelligence = 0;
        p.Cunning = 0;
    }

    [Command(requiresAuthority = false)]
    public void ModifyPower(int amount, PlayerScript p)
    {
        if (cantLosePower && amount > 0)
            return;
        if (amount < 0 && p.passive.passiveName == "Taunt")
        {
            p.Strength = 1;
        }
        if (p.passive.passiveName == "Unstable")
        {
            amount *= 2;
        }
        p.Power = amount;
    }

    [Command(requiresAuthority = false)]
    public void AddPoints(int amount, PlayerScript p)
    {
        p.maxPoints = amount;
        p.availablePoints = amount;
    }

    [Command(requiresAuthority = false)]
    public void DiscardCard(PlayerScript p, int index, GameObject[] slots)
    {
        int rand = UnityEngine.Random.Range(0, p.cards.Count - 1);
        p.cards.Add(p.hand[index]);
        p.hand[index] = p.cards[rand];
        p.cards.Remove(p.cards[rand]);
        RpcFillSlot(p.connectionToClient, slots, p.hand[index][1], p.hand[index][0], p.hand[index][4]);
    }

    [Command(requiresAuthority = false)]
    public void DiscardRevealed(PlayerScript p, GameObject[] slots, PlayerScript targetPlayer, List<int> indexes)
    {
        for (int i = 0; i < 6; i++)
        {
                int rand = UnityEngine.Random.Range(0, 33);
                targetPlayer.cards.Add(targetPlayer.hand[indexes[i]]);
                targetPlayer.hand[indexes[i]] = targetPlayer.cards[rand];
                targetPlayer.cards.Remove(targetPlayer.cards[rand]);
                RpcFillSlot(targetPlayer.connectionToClient, slots, targetPlayer.hand[indexes[i]][1], targetPlayer.hand[indexes[i]][0], targetPlayer.hand[indexes[i]][4]);
        }
    }

    [TargetRpc]
    public void RpcFillSlot(NetworkConnection conn, GameObject[] slots, string title, string type, string id)
    {
        foreach (GameObject g in slots)
        {
            if (g.GetComponent<CardScript>().Title == "")
            {
                g.GetComponent<CardScript>().Title = title;
                g.GetComponent<CardScript>().Type = type;
                g.GetComponent<CardScript>().ID = id;
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void setUntargetable(PlayerScript p)
    {
        p.untargetable = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdturnIncrease(PlayerScript p)
    {
        FSM.turn++;
        p.turnTaken = false;
    }

    [Command (requiresAuthority = false)]
    public void CmdDisablePLoss(PlayerScript p)
    {
        p.cantLosePower = true;
    }
    [Command(requiresAuthority = false)]
    public void CmdDisableSLoss(PlayerScript p)
    {
        p.cantLoseStats = true;
    }
}
