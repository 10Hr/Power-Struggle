using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Mirror;
using System;
using System.Linq;
using TMPro;
using Utp;
using UnityEngine.SceneManagement;
//CLIENT SCRIPT
public class PlayerScript : NetworkBehaviour
{

//----------------------Relay Testing---------------------------------
    [SyncVar]
    public string sessionId = "";

    public string username;

    public string ip;

    public string platform;

    public void Awake() {
        //username = SystemInfo.deviceName;
        // platform = Application.platform.ToString();
        //ip = NetworkManager.singleton.networkAddress;
        //if (SceneManager.GetActiveScene().name == "Game")
        // {
        //DontDestroyOnLoad(transform.gameObject);
        instructions = GameObject.Find("Instructions").GetComponent<TextMeshProUGUI>();
            instructions.text = "DON'T TOUCH ANYTHING!!!!!";
       // }

    }
//--------------------------------------------------------------------

    //Fields
    public GameState FSM;

    public MessageLogManager logger;

    public EventManager eMan;

    public bool added = false;

    public GameObject turnToken;

    [SyncVar]
    public int Praise = 8;
    [SyncVar]
    public int Censure = 4;
    [SyncVar]
    public int eVotes = 0;
    [SyncVar]
    public int e1Total = 0;
    [SyncVar]
    public int e2Total = 0;
    [SyncVar]
    public int e3Total = 0;

    [SyncVar]
    public string playerName = "";

    [SyncVar]
    public bool untargetable = false;

    [SyncVar]
    public bool selectedTrg = false;

    [SyncVar]
    public int powerGained = 0;

    [SyncVar]
    public int powerLost = 0;

    [SyncVar]
    public bool turnTaken = false;

    public int currentBet = 0;

    [SyncVar]
    public bool cantLosePower = false;

    [SyncVar]
    public string guess1 = "";
    [SyncVar]
    public string guess2 = "";
    [SyncVar]
    public string guess3 = "";

    [SyncVar]
    public string passive2 = "";

    [SyncVar]
    public bool cantLoseStats = false;

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
    [SyncVar]
    public string allyStat = "";

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

    [SyncVar]
    public PlayerList playerList;

    [SyncVar]
    public int playerNumber;

    [SyncVar]
    public bool ready;

    [SyncVar]
    public string highest = "";

    [SyncVar]
    public string lowest = "";

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
    public bool sawDeck = false;

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

    public LeaderBoardManager leaderBoard;

    int g = 0;
    public readonly SyncList<int> cards = new SyncList<int>();
    public readonly SyncList<int> discardDeck = new SyncList<int>();

    //public List<string[]> cards = new List<string[]>();
    public readonly SyncList<int> hand = new SyncList<int>();
    public readonly SyncList<Passive> choicesList = new SyncList<Passive>();

    GameObject bntTop;
    GameObject bntLeft;
    GameObject bntRight;
    GameObject lockInButton;
    TextMeshProUGUI instructions;

    Text pLabel1;
    Text pLabel2;
    Text pLabel3;

    public GameObject maincanvas;

    public GameObject menucanvas;

    public int disabled1 = 0;

    //Properties
    //Methods

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }
    
    public void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        menucanvas = GameObject.Find("MenuCanvas");
        leaderBoard = GameObject.Find("Leaderboard").GetComponent<LeaderBoardManager>();
        playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
        FSM = GameObject.Find("FSM").GetComponent<GameState>();
        logger = GameObject.Find("LogManager").GetComponent<MessageLogManager>();

        passive = gameObject.GetComponent<Passive>();

        passiveOption1 = GameObject.Find("bntChoice1");
        passiveOption2 = GameObject.Find("bntChoice2");
        passiveOption3 = GameObject.Find("bntChoice3");

        lockInButton = GameObject.Find("LockInButton1");

        eMan = GameObject.Find("EventManager").GetComponent<EventManager>();

        charismaText = GameObject.Find("CharismaCounter").GetComponent<Text>();
        strengthText = GameObject.Find("StrengthCounter").GetComponent<Text>();
        intelligenceText = GameObject.Find("IntelligenceCounter").GetComponent<Text>();
        cunningText = GameObject.Find("CunningCounter").GetComponent<Text>();
        addButtons = GameObject.FindGameObjectsWithTag("add");
        subButtons = GameObject.FindGameObjectsWithTag("sub");

        turnToken = GameObject.Find("Token");

        cardSlots = GameObject.FindGameObjectsWithTag("CardSlot");
        enemySlots1 = GameObject.FindGameObjectsWithTag("1");
        enemySlots2 = GameObject.FindGameObjectsWithTag("2");
        enemySlots3 = GameObject.FindGameObjectsWithTag("3");

        passiveManager = GameObject.Find("PassiveManager").GetComponent<PassiveManager>();

        bntTop = GameObject.Find("PlayerTop");
        bntLeft = GameObject.Find("PlayerLeft");
        bntRight = GameObject.Find("PlayerRight");

        pLabel1 = GameObject.Find("PlayerLabel1").GetComponent<Text>();
        pLabel2 = GameObject.Find("PlayerLabel2").GetComponent<Text>();
        pLabel3 = GameObject.Find("PlayerLabel3").GetComponent<Text>();

        if (isLocalPlayer)
        {
            bntTop.SetActive(false);
            bntLeft.SetActive(false);
            bntRight.SetActive(false);
            passiveOption1.SetActive(false);
            passiveOption2.SetActive(false);
            passiveOption3.SetActive(false);
            lockInButton.SetActive(false);
            turnToken.SetActive(false);
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
                if (playerList.players.Count == 4)
                {
                    CmdSetPlayer();
                    leaderBoard.CmdUpdateLeaderBoard();
                    instructions.text = "Allocate points to your stats,\n you have 8 points to start with.";
                    if (menucanvas.activeSelf == true)
                    {
                        CmdSetAllyStat("");
                    }
                    menucanvas.SetActive(false);
                }
                break;
            case GameStates.Passive:
                //calculate players highest stat
                instructions.text = "Choose a Passive from 1 of the 3 listed.";
                CalcHighest(); //finds highest and sets has highesst to true
                CalcLowest();

                //create deck and get cards data
                if (!hasDeck && hasHighest)
                {
                    hasDeck = true;
                    DeckScript deck = gameObject.AddComponent(typeof(DeckScript)) as DeckScript;
                    deck.CreateDeck(highest);
                    if (deck.cardDataIDs.Count > 36)
                    {
                        foreach (int i in deck.cardDataIDs)
                        {
                            CmdFillDeck(i);
                        }
                    }
                }

                //spawn passive choices
                if (g == 0 && hasHighest) {
                    passiveOption1.SetActive(true);
                    passiveOption2.SetActive(true);
                    passiveOption3.SetActive(true);
                    passiveManager.CmdSelectPassive(highest, this);
                    g = 1;
                }
                if (passive.passiveName != "" && isLocalPlayer)
                {
                    CmdSelectedPassive();
                }
                break;

            case GameStates.DrawCards:
                CmdDrawCard();
                if (hand.Count == 6 && !cardsSpawned)
                {
                    TransferData(cardSlots, this);
                    if (cardSlots[5].GetComponent<CardScript>().Title != "")
                        CmdSpawnedCards();
                }

                break;

            case GameStates.LoadEnemyCards:
                instructions.text = "Select three cards and press lock in.";
                if (passive.passiveName == "Blackmarket")
                    instructions.text = "Select 3, to lock in. Right click card to use BlackMarket";
                if (passive.passiveName == "Tactician")
                    instructions.text = "Click enemy cards to disable, then select 3 cards and lock in";
                if (FSM.EventTwo)
                    instructions.text = instructions.text + " Due to he last event gamerule, you don't discard cards you play this turn";
                TransferEnemyData();
                switch (passive.passiveName)
                {
                    case "StrongAllies":
                        if (allyStat != "strength")
                            CmdSetAllyStat("strength");
                        break;
                    case "SmartAllies":
                        if (allyStat != "intelligence")
                            CmdSetAllyStat("intelligence");
                        break;
                    case "ShadyAllies":
                        if (allyStat != "cunning")
                            CmdSetAllyStat("cunning");
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
                if (numSelected == 3 && !LockedIn)
                {
                    CmdThreeSelected(true);
                    lockInButton.SetActive(true);
                }
                else
                {
                    CmdThreeSelected(false);
                    lockInButton.SetActive(false);
                }
                leaderBoard.CmdUpdateLeaderBoard();
                if (enemy1.hand.Count == 6)
                    UpdateData(enemySlots1, enemy1);
                if (enemy2.hand.Count == 6)
                    UpdateData(enemySlots2, enemy2);
                if (enemy3.hand.Count == 6)
                    UpdateData(enemySlots3, enemy3);
                if (hand.Count == 6)
                    UpdateData(cardSlots, this);
                break;

            case GameStates.Turn:
                if (!FSM.EventTwo)
                    instructions.text = "When it is your turn, select a card to play it.";
                else
                    instructions.text = "When it is your turn, select a card to play it." + "\nDue to the last event gamerule, you don't discard cards you play this turn";
                //if (passive.passiveName == "Tactician") {

                // }
                if (LockedIn)
                    CmdUnlock();
                if (enemy1.hand.Count == 6)
                    UpdateData(enemySlots1, enemy1);
                if (enemy2.hand.Count == 6)
                    UpdateData(enemySlots2, enemy2);
                if (enemy3.hand.Count == 6)
                    UpdateData(enemySlots3, enemy3);
                if (hand.Count == 6)
                    UpdateData(cardSlots, this);
                if (FSM.currentPlayer.netId == this.netIdentity.netId)
                    turnToken.SetActive(true);
                else
                    turnToken.SetActive(false);
                break;

            case GameStates.Event:

                foreach (GameObject g in cardSlots)
                {
                    if (g.GetComponent<CardScript>().disabled)
                    {
                        g.GetComponent<CardScript>().disabled = false;
                    }

                }
                disabled1 = 0;

                leaderBoard.CmdUpdateLeaderBoard();
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
                b.SetActive(true);
            foreach (GameObject b in subButtons)
                b.SetActive(true);
        }
        else if (availablePoints == maxPoints)
        {
            foreach (GameObject b in addButtons)
                b.SetActive(true);
            foreach (GameObject b in subButtons)
                b.SetActive(false);
        }
        else if (availablePoints <= 0)
        {
            foreach (GameObject b in subButtons)
                b.SetActive(true);
            foreach (GameObject b in addButtons)
                b.SetActive(false);
        }
        if (ready && availablePoints == 0)
        {
            foreach (GameObject b in subButtons)
                b.SetActive(false);
            foreach (GameObject b in addButtons)
                b.SetActive(false);
        }
        if (!ready && availablePoints == 0 && FSM.CurrentState == GameStates.Turn)
        {
            ready = true;
            foreach (GameObject b in subButtons)
                b.SetActive(false);
            foreach (GameObject b in addButtons)
                b.SetActive(false);
        }
        if (!ready && FSM.currentState == GameStates.Event && availablePoints != 0)
        {
            foreach (GameObject b in subButtons)
                b.SetActive(true);
            foreach (GameObject b in addButtons)
                b.SetActive(true);
        }
        else if (!ready && FSM.currentState == GameStates.Event && availablePoints == 0)
        {
            ready = true;
            foreach (GameObject b in subButtons)
                b.SetActive(false);
            foreach (GameObject b in addButtons)
                b.SetActive(false);
        }
            
        #endregion
    }

    [Command(requiresAuthority = false)]
    public void CmdUnlock()
    {
        LockedIn = false;
        threeSelected = false;
        numSelected = 0;
    }

    [Command(requiresAuthority = false)]
    public void CmdResetCards()
    {
        cards.Clear();
        //hand.Clear();
    }

    [Command(requiresAuthority = false)]
    public void CmdConfirm()
    {
        print(playerName + " Made it 2");
    }

    [TargetRpc]
    public void SwapDeck(string h)
    {
        logger.AppendMessage(playerName + " is swapping decks");
        gameObject.GetComponent<DeckScript>().CreateDeck(h);
        CmdResetCards();

        while (gameObject.GetComponent<DeckScript>().cardDataIDs.Count < 40) {}

        foreach (int i in gameObject.GetComponent<DeckScript>().cardDataIDs)
            CmdFillDeck(i);//works for host, no clients
    }

    [Command (requiresAuthority = false)]
    public void CmdThreeSelected(bool b)
    {
        threeSelected = b;
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
            TransferData(enemySlots1, enemy1);
        else if (enemySlots2[5].GetComponent<CardScript>().Title == "")
            TransferData(enemySlots2, enemy2);
        else if (enemySlots3[5].GetComponent<CardScript>().Title == "")
            TransferData(enemySlots3, enemy3);
        if (enemy1 != null && enemy2 != null && enemy3 != null)
        {
            pLabel1.text = enemy1.playerName;
            pLabel2.text = enemy2.playerName;
            pLabel3.text = enemy3.playerName;
        }
        sendPlayerData();
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawnedCards()
    {
        cardsSpawned = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdSelectedPassive()
    {
        hasPassive = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdDrawCard()
    {
        int rand = UnityEngine.Random.Range(0, cards.Count - 1);
        if (hand.Count < 6)
        {
            hand.Add(cards[rand]);
            cards.Remove(cards[rand]);
        }
    }
    

    public void UpdateData(GameObject[] slots, PlayerScript p)
    {
        int index = 0;
        foreach (GameObject g in slots)
        {
            if (g.GetComponent<CardScript>().Title != gameObject.GetComponent<DeckScript>().sendCardName(p.hand[index]))
            {
                g.GetComponent<CardScript>().revealed = false;
            }
            g.GetComponent<CardScript>().Title = gameObject.GetComponent<DeckScript>().sendCardName(p.hand[index]);
            g.GetComponent<CardScript>().Type = gameObject.GetComponent<DeckScript>().sendCardType(p.hand[index]);
            g.GetComponent<CardScript>().ID = p.hand[index].ToString();
            index++;
        }
    }

    public void TransferData(GameObject[] slots, PlayerScript p)
    {
        int index = 0;
        foreach (GameObject g in slots)
        {
            if (g.GetComponent<CardScript>().Title == "")
            {
                Debug.Log(p.hand[index]); 
                //Debug.Log(deck.sendCardName(p.hand[index]));
                g.GetComponent<CardScript>().Title = gameObject.GetComponent<DeckScript>().sendCardName(p.hand[index]);
                g.GetComponent<CardScript>().Type = gameObject.GetComponent<DeckScript>().sendCardType(p.hand[index]);
                g.GetComponent<CardScript>().ID = p.hand[index].ToString();
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
            CmdSendHighest(newHighest);
        }
    }

    public void CalcLowest() {
        string currentLowest = lowest;
        string newlow;
        int currentlow = 1000;

        switch (currentLowest)
        {
            case "strength":
                currentlow = strength;
                break;
            case "intelligence":
                currentlow = intelligence;
                break;
            case "charisma":
                currentlow = charisma;
                break;
            case "cunning":
                currentlow = cunning;
                break;
        }

        int[] statList = {strength, intelligence, charisma, cunning};
        int min = statList.Min();

        if (min < currentlow) {
            if (statList[0] == min)
                newlow = "strength";
            else if (statList[1] == min)
                newlow = "intelligence";
            else if(statList[2] == min)
                newlow = "charisma";
            else
                newlow = "cunning";
            CmdSendLowest(newlow);
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSendHighest(string h)
    {
        highest = h;
        hasHighest = true;
        if (FSM.CurrentState == GameStates.Turn || FSM.CurrentState == GameStates.Event)
        {
            if (passive.passiveName == "ShadyBusiness" || passive2 == "ShadyBuisness")
                ModifyPower(50);
            SwapDeck(h);
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSendLowest(string l)
    {
        lowest = l;
    }

    [Command(requiresAuthority = false)]
    public void CmdFillDeck(int cardID)
    {
        cards.Add(cardID);
    }

    [Command(requiresAuthority = false)]
    public void CmdSendPlayers(PlayerScript player)
    {
        playerList.CmdAddPlayers(player);
    }

    [Command(requiresAuthority = false)]
    public void CmdSetPlayer()
    {
        //string[] gamer = { "hi", "bye" };
        //cards.Add(gamer);
        RpcSetPlayer();
    }

    [ClientRpc]
    public void RpcSetPlayer()
    {
        playerNumber = (int)netIdentity.netId;
        playerName = "Player " + (playerList.players.IndexOf(this) + 1);
        gameObject.name = $"{playerNumber}";
    } 

    public List<PlayerScript> sendPlayerData() {
        List<PlayerScript> playerslots = new List<PlayerScript>();
        playerslots.Add(enemy1); //bntright
        playerslots.Add(enemy2); //bntleft
        playerslots.Add(enemy3); //bnttop

        return playerslots;
    }

    public bool CheckPassive(string s)
    {
        if (passive.passiveName == s || passive2 == s)
            return true;
        else
            return false;
    }

    public void UnhideButtons() {
        PlayerScript[] enemies = { enemy1, enemy2, enemy3 };
        GameObject[] btns = {bntRight, bntTop, bntLeft };
        /*
         So bascially, we only want an enemy to be targetable if they are targetable.
        To be targetable, players cant be 'untargetable', they're passive and second passive cannot be 'Scrapper' (unless you are below them on the leaderboard),
        and they're highest stat is not the same as the charisma players allyStat passive or second passive (and vice versa.)
        */
        for (int i = 0; i < 3; i++)
            if (CheckPassive("Precise")
            || ((!enemies[i].untargetable) 
            && !(enemies[i].CheckPassive("Scrapper") && enemies[i].Power < power)
            && !(allyStat == "strength" && enemies[i].Highest == "strength")
            && !(allyStat == "intelligence" && enemies[i].Highest == "intelligence")
            && !(allyStat == "cunning" && enemies[i].Highest == "cunning")
            && !(enemies[i].allyStat == "strength" && Highest == "strength")
            && !(enemies[i].allyStat == "intelligence" && Highest == "intelligence")
            && !(enemies[i].allyStat == "cunning" && Highest == "cunning")))
                btns[i].SetActive(true);

        if(!bntLeft.activeSelf && !bntRight.activeSelf && !bntTop.activeSelf)
        {
            bntTop.SetActive(true);
            bntRight.SetActive(true);
            bntLeft.SetActive(true);
        }
    }

    public void hideButtons() {
        GameObject[] btns = { bntRight, bntTop, bntLeft };
        for (int i = 0; i < 3; i++)
            if (btns[i].activeInHierarchy)
                btns[i].SetActive(false);
    }

        [Command(requiresAuthority = false)]
        public void ModifyStats(string type, int amount) {
        if (amount == 0)
            return;
        if (cantLoseStats && amount < 0)
            return;
        if (netId != FSM.currentPlayer.netId && FSM.turn < 4 && FSM.currentPlayer.passive.passiveName == "DoubleDown")
            amount *= 2;
        switch (type) {
                case "strength":
                   Strength = amount;
                    break;
                case "charisma":
                    Charisma = amount;
                    break;
                case "intelligence":
                    Intelligence = amount;
                    break;
                case "cunning":
                    Cunning = amount;
                    break;
            }
            MaxPoints = amount;
        logger.AppendMessage(string.Format("{0} gained {1} {2}", playerName, amount, type));
        CalcLowest();
        CalcHighest();
    }

    [Command(requiresAuthority = false)]
    public void ResetStats()
    {
        logger.AppendMessage(string.Format("{0} is resetting their stats", playerName));
        AvailablePoints = Charisma + Strength + Cunning + Intelligence;
        Strength = 0;
        Charisma = 0;
        Intelligence = 0;
        Cunning = 0;
    }

    [Command(requiresAuthority = false)]
    public void ModifyPower(int amount)
    {
        logger.AppendMessage(string.Format("{0} gained {1} power", playerName, amount));
        if (cantLosePower && amount < 0)
            return;
        if (amount < 0 && (passive.passiveName == "Taunt" || passive2 == "Taunt"))
        {
            Strength = 1;
        }
        if (passive.passiveName == "Unstable" || passive2 == "Unstable")
        {
            amount *= 2;
        }
        Power = amount;
        if (amount > 0)
            powerGained += amount;
        else
            powerLost += -amount;
    }

    [Command(requiresAuthority = false)]
    public void AddPoints(int amount)
    {
        logger.AppendMessage(string.Format("{0} gained {1} available points", playerName, amount));
        maxPoints = amount;
        availablePoints = amount;
    }

    [Command(requiresAuthority = false)]
    public void DiscardCard(int index, GameObject[] slots)
    {
        if (!FSM.EventTwo) {
            int rand = UnityEngine.Random.Range(0, cards.Count - 1);
            discardDeck.Add(hand[index]);
            hand[index] = cards[rand];
            cards.Remove(cards[rand]);

            if (cards.Count == 0)
                foreach (int i in gameObject.GetComponent<DeckScript>().cardDataIDs)
                    CmdFillDeck(i);//works for host, no clients

            RpcFillSlot(connectionToClient, slots, hand[index]);
        }
        else
        {
            RpcFillSlot(connectionToClient, slots, hand[index]);
        }
    }

    [Command(requiresAuthority = false)]
    public void DiscardRevealed(GameObject[] slots, PlayerScript targetPlayer, List<int> indexes)
    {
        for (int i = 0; i < indexes.Count; i++)
        {
                int rand = UnityEngine.Random.Range(0, cards.Count - 1);
                discardDeck.Add(hand[indexes[i]]);
                hand[indexes[i]] = cards[rand];
                cards.Remove(cards[rand]);
                RpcFillSlot(connectionToClient, slots, hand[indexes[i]]);
        }
    }

    [TargetRpc]
    public void RpcFillSlot(NetworkConnection conn, GameObject[] slots, int id)
    {
        foreach (GameObject g in slots)
        {
            if (g.GetComponent<CardScript>().Title == "")
            {
                g.GetComponent<CardScript>().Title = gameObject.GetComponent<DeckScript>().sendCardName(id);
                g.GetComponent<CardScript>().Type = gameObject.GetComponent<DeckScript>().sendCardType(id);
                g.GetComponent<CardScript>().ID = id.ToString();
            }
        }
    }

    [TargetRpc]
    public void Turn(NetworkConnection conn)
    {
        if (passive.passiveName == "SeeDeck" || passive2 == "SeeDeck")
        {
            gameObject.GetComponent<DeckScript>().SeeDeck();
            CmdSetSawDeck(true);
        }
    }

    [Command(requiresAuthority = false)]
    public void setUntargetable(bool b)
    {
        untargetable = b;
    }

    [Command(requiresAuthority = false)]
    public void CmdturnIncrease()
    {
        FSM.turn++;
        turnTaken = false;
    }

    [Command (requiresAuthority = false)]
    public void CmdDisablePLoss(bool b)
    {
        cantLosePower = b;
    }
    [Command(requiresAuthority = false)]
    public void CmdDisableSLoss(bool b)
    {
        cantLoseStats = b;
    }

    [Command(requiresAuthority = false)]
    public void CmdSetAllyStat(string s)
    {
        allyStat = s;
        if (FSM.currentState != GameStates.LoadEnemyCards)
            logger.AppendMessage(playerName + " changed their allyStat to " + s);
    }

    [TargetRpc]
    public void CheckGuess(NetworkConnection conn)
    {
            if (guess1 == enemy1.Highest)
            {
                enemy1.ModifyPower(-50);
                ModifyPower(50);
            }
            else
            {
                enemy1.ModifyPower(50);
                ModifyPower(-50);
            }

            if (guess2 == enemy2.Highest)
            {
                enemy2.ModifyPower(-50);
                ModifyPower(50);
            }
            else
            {
                enemy2.ModifyPower(50);
                ModifyPower(-50);
            }

            if (guess3 == enemy3.Highest)
            {
                enemy3.ModifyPower(-50);
                ModifyPower(50);
            }
            else
            {
                enemy3.ModifyPower(50);
                ModifyPower(-50);
            }
    }

    [Command (requiresAuthority = false)]
    public void CmdSetSawDeck(bool b)
    {
        sawDeck = b;
    }

    [Command(requiresAuthority = false)]
    public void CmdSelectedTrg(bool b)
    {
        selectedTrg = b;
    }

    [Command(requiresAuthority = false)]
    public void CmdDisableCard(PlayerScript e, string ID) {
        for (int i = 0; i < 6; i++) {
            //logger.AppendMessage(string.Format("EnemyID: {0} SearchID: {1}", e.hand[i], ID));
            if (e.hand[i] == int.Parse(ID)) { 
                logger.AppendMessage(string.Format("{0} disabled a card in {1}'s hand", playerName, e.playerName));
                DisableCard(e.connectionToClient, i);
                break;
            }    
        }
    }

    [TargetRpc]
    public void DisableCard(NetworkConnection conn, int index) {
            conn.identity.GetComponent<PlayerScript>().cardSlots[index].GetComponent<CardScript>().disabled = true;
    }

    [Command (requiresAuthority = false)]
    public void ResetTrackers()
    {
        powerGained = 0;
        powerLost = 0;
    }

}
