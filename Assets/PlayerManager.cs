using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;

public class PlayerManager : NetworkBehaviour {

    NetworkIdentity player1ID;
    NetworkIdentity player2ID;
    NetworkIdentity player3ID;
    NetworkIdentity player4ID;
    List<NetworkIdentity> playerIDs;

    public PlayerScript player1;
    public PlayerScript player2;
    public PlayerScript player3;
    public PlayerScript player4;

    [SyncVar]
    public GameObject deck1;
    [SyncVar]
    public GameObject deck2;
    [SyncVar]
    public GameObject deck3;
    [SyncVar]
    public GameObject deck4;
    [SyncVar]
    public int playerCount;
    public List<GameObject> hand1 = new List<GameObject>();
    public List<GameObject> hand2 = new List<GameObject>();
    public List<GameObject> hand3 = new List<GameObject>();
    public List<GameObject> hand4 = new List<GameObject>();
    NetworkIdentity[] listObjects;
    GameState gameManager;
    PassiveManager passiveManager;

    public void getCount() { playerCount++; } //player count

    public void updateList() {
        listObjects = FindObjectsOfType<NetworkIdentity>();
    }


    // Start is called before the first frame update
    void Awake()
    {
        playerIDs = new List<NetworkIdentity>();
        gameManager = GameObject.Find("FSM").GetComponent<GameState>();
        deck1 = GameObject.Find("Deck1");
        deck2 = GameObject.Find("Deck2");
        deck3 = GameObject.Find("Deck3");
        deck4 = GameObject.Find("Deck4");
        passiveManager = GameObject.Find("PassiveManager").GetComponent<PassiveManager>();
    }

    // Update is called once per frame
    void Update() { 
        listObjects = FindObjectsOfType<NetworkIdentity>(); 
    }

    public int AddNet(NetworkIdentity playerID) {
        playerIDs.Add(playerID);

        if (playerIDs[0] != null && playerIDs.Count == 1) {
            player1ID = playerIDs[0];
            setPlayer(0);
        }
        else if (playerIDs[1] != null && playerIDs.Count == 2) {
            player2ID = playerIDs[1];
            setPlayer(1);
        }
        else if (playerIDs[2] != null && playerIDs.Count == 3) {
            player3ID = playerIDs[2];
            setPlayer(2);
        }
        else if (playerIDs[3] != null && playerIDs.Count == 4) {
            player4ID = playerIDs[3];
            setPlayer(3);
        }
        return playerIDs.Count;
    }

    public void setPlayer(int whichPlayer) {
        //each time a player connects, assign their IDs to fields
        updateList();

        foreach (NetworkIdentity obj in listObjects) {    // gets player object    
                                                          // Debug.Log("obj.GetComponent<NetworkIdentity>() " + obj.GetComponent<NetworkIdentity>() + " player1ID " + player1ID + " playercount " + playerCount + " whichPlayer " + whichPlayer);
            if (obj.GetComponent<NetworkIdentity>() == player1ID && playerCount == 1 && whichPlayer == 0) {
                player1 = obj.GetComponent<PlayerScript>();
                Debug.Log("Player 1 is created!");
            }
            else if (obj.GetComponent<NetworkIdentity>() == player2ID && playerCount == 2 && whichPlayer == 1) {
                player2 = obj.GetComponent<PlayerScript>();
                Debug.Log("Player 2 is created!");
            }
            else if (obj.GetComponent<NetworkIdentity>() == player3ID && playerCount == 3 && whichPlayer == 2) {
                player3 = obj.GetComponent<PlayerScript>();
                Debug.Log("Player 3 is created!");
            }
            else if (obj.GetComponent<NetworkIdentity>() == player4ID && playerCount == 4 && whichPlayer == 3) {
                player4 = obj.GetComponent<PlayerScript>();
                Debug.Log("Player 4 is created!");
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdReadyPlayer(string btnTag) {
        switch (btnTag)
        {
            case "Player1":
                player1.readied = true;
                break;
            case "Player2":
                player2.readied = true;
                break;
            case "Player3":
                player3.readied = true;
                break;
            case "Player4":
                player4.readied = true;
                break;
        }

        if (player1.readied && player2.readied && player3.readied && player4.readied)
        {
            Debug.Log("All players are ready!");
            //start game
            gameManager.AllReady = true;
            gameManager.PassivesSelected = true;
            RpcAllReady(gameManager);
        }

    }
    //probably something to do with this method
    [ClientRpc]
    public void RpcAllReady(GameState gameManager)
    {
        gameManager.AllReady = true;
        gameManager.PassivesSelected = true;
    }

    //Find decks objects in scene
    //Depending on player spawn that deck
    //create deck properties for both server and all clients
    [Command(requiresAuthority = false)]
    public void CmdDeckMaker(string highest, int pNum)
    {
        //get deck objects
        deck1 = GameObject.Find("Deck1");
        deck2 = GameObject.Find("Deck2");
        deck3 = GameObject.Find("Deck3");
        deck4 = GameObject.Find("Deck4");

        //IF SOMETHING GOES WRONG WITH CARD OBJECTS, TRY UNCOMMENTING FOR LOOPS
        //depending on player
        switch (pNum)
        {
            case 1:
                //create deck properties for server and clients
                RPCGiveDeck(deck1, player1, highest);
                deck1.GetComponent<DeckScript>().CreateDeck(highest, this.GetComponent<InstantiatePrefab>().cardPrefab);

                //spawns all the cards in that deck
                //for (int i = 0; i < deck1.GetComponent<DeckScript>().cards.Count; i++)
                    //NetworkServer.Spawn(deck1.GetComponent<DeckScript>().cards[i]);
                break;

            case 2:
                RPCGiveDeck(deck2, player2, highest);
                deck2.GetComponent<DeckScript>().CreateDeck(highest, this.GetComponent<InstantiatePrefab>().cardPrefab);
                //for (int i = 0; i < deck2.GetComponent<DeckScript>().cards.Count; i++) 
                    //NetworkServer.Spawn(deck2.GetComponent<DeckScript>().cards[i]);
                break;

            case 3:
                RPCGiveDeck(deck3, player3, highest);
                deck3.GetComponent<DeckScript>().CreateDeck(highest, this.GetComponent<InstantiatePrefab>().cardPrefab);
                //for (int i = 0; i < deck3.GetComponent<DeckScript>().cards.Count; i++)
                    //NetworkServer.Spawn(deck3.GetComponent<DeckScript>().cards[i]);
                break;

            case 4:
                RPCGiveDeck(deck4, player4, highest);
                deck4.GetComponent<DeckScript>().CreateDeck(highest, this.GetComponent<InstantiatePrefab>().cardPrefab);
                //for (int i = 0; i < deck4.GetComponent<DeckScript>().cards.Count; i++)
                    //NetworkServer.Spawn(deck4.GetComponent<DeckScript>().cards[i]);
                break;

            default:
                break;
        }
    }

    
    //Creates players hands depending on which player is ready to draw
    //draws the cards after hand is created
    //draws cards 1 at a time (1 per call of this method)
    [Command(requiresAuthority = false)]
    public void HandMaker(int pNum, NetworkConnectionToClient conn)  //Shorter name for PlayerNum
    {
        //Depending on Player
        switch (pNum)
        {
            case 1:
                //add the card to the hand
                // NOTE: LISTS ITEMS SHIFT FOWARD IF ONE IS REMOVED
                // SO IF cards[0] IS REMOVED cards[1] WILL TAKE ITS PLACE
                hand1.Add(deck1.GetComponent<DeckScript>().cards[0]);
                //remove the card from the deck
                deck1.GetComponent<DeckScript>().cards.RemoveAt(0);
                //instaniate and spawn card
                Draw(deck1, hand1, pNum, conn);
                break;

            case 2:
                hand2.Add(deck2.GetComponent<DeckScript>().cards[0]);
                deck2.GetComponent<DeckScript>().cards.RemoveAt(0);
                Draw(deck2, hand2, pNum, conn);
                break;

            case 3:
                hand3.Add(deck3.GetComponent<DeckScript>().cards[0]);
                deck3.GetComponent<DeckScript>().cards.RemoveAt(0);
                Draw(deck3, hand3, pNum, conn);
                break;

            case 4:
                hand4.Add(deck4.GetComponent<DeckScript>().cards[0]);
                deck4.GetComponent<DeckScript>().cards.RemoveAt(0);
                Draw(deck4, hand4, pNum, conn);
                break;

            default:
                Debug.Log("Invalid player.");
                break;
        }


    }

    //Gets the correct sprite/prefab for the card and instatiates and spawns it
    //adjusts the hand to represent the spawned cards
    public void Draw(GameObject deck, List<GameObject> hand, int pNum, NetworkConnectionToClient conn)
    {
        //As long as the deck has cards to draw
        if (deck.GetComponent<DeckScript>().cards.Count >= 0) //Looking back on it, this should be in HandMaker... It shouldn't be causing the problem though.
        {
            //get correct prefab
            GameObject prefab;
            switch (deck.GetComponent<DeckScript>().Type)
            {
                case "charisma":
                    prefab = this.GetComponent<InstantiatePrefab>().chaPrefab;
                    break;
                case "cunning":
                    prefab = this.GetComponent<InstantiatePrefab>().cunPrefab;
                    break;
                case "intelligence":
                    prefab = this.GetComponent<InstantiatePrefab>().intPrefab;
                    break;
                case "strength":
                    prefab = this.GetComponent<InstantiatePrefab>().strPrefab;
                    break;
                default:
                    Debug.Log("The deck is null.");
                    prefab = null;
                    break;
            }
            //Instantiate and spawn
            GameObject cardToSpawn = Instantiate(prefab);
            //cardToSpawn.AddComponent<CardScript>();
            NetworkServer.Spawn(cardToSpawn, conn);
            //update hand
            switch (pNum)
            {
                case 1:
                    cardToSpawn.GetComponent<CardScript>().Effect = hand1[hand1.Count - 1].GetComponent<CardScript>().Effect;
                    cardToSpawn.GetComponent<CardScript>().Title = hand1[hand1.Count - 1].GetComponent<CardScript>().Title;
                    cardToSpawn.GetComponent<CardScript>().Stat = hand1[hand1.Count - 1].GetComponent<CardScript>().Stat;
                    hand1[hand1.Count - 1] = cardToSpawn;
                    player1.hand.Add(hand1[hand1.Count - 1]);
                    AdjustCards(hand1, pNum);
                    break;
                case 2:
                    cardToSpawn.GetComponent<CardScript>().Effect = hand2[hand2.Count - 1].GetComponent<CardScript>().Effect;
                    cardToSpawn.GetComponent<CardScript>().Title = hand2[hand2.Count - 1].GetComponent<CardScript>().Title;
                    cardToSpawn.GetComponent<CardScript>().Stat = hand2[hand2.Count - 1].GetComponent<CardScript>().Stat;
                    hand2[hand2.Count - 1] = cardToSpawn;
                    player2.hand.Add(hand2[hand2.Count - 1]);
                    AdjustCards(hand2, pNum);
                    break;
                case 3:
                    cardToSpawn.GetComponent<CardScript>().Effect = hand3[hand3.Count - 1].GetComponent<CardScript>().Effect;
                    cardToSpawn.GetComponent<CardScript>().Title = hand3[hand3.Count - 1].GetComponent<CardScript>().Title;
                    cardToSpawn.GetComponent<CardScript>().Stat = hand3[hand3.Count - 1].GetComponent<CardScript>().Stat;
                    hand3[hand3.Count - 1] = cardToSpawn;
                    player3.hand.Add(hand3[hand3.Count - 1]);
                    AdjustCards(hand3, pNum);
                    break;
                case 4:
                    cardToSpawn.GetComponent<CardScript>().Effect = hand4[hand4.Count - 1].GetComponent<CardScript>().Effect;
                    cardToSpawn.GetComponent<CardScript>().Title = hand4[hand4.Count - 1].GetComponent<CardScript>().Title;
                    cardToSpawn.GetComponent<CardScript>().Stat = hand4[hand4.Count - 1].GetComponent<CardScript>().Stat;
                    hand4[hand4.Count - 1] = cardToSpawn;
                    player4.hand.Add(hand4[hand4.Count - 1]);
                    AdjustCards(hand4, pNum);
                    break;
            }
        }
    }

    //Is Called on all clients and server
    //doesnt work on server to prevent the host having multiple copies
    //creates deck properties for clients
    [ClientRpc]
    public void RPCGiveDeck(GameObject deck, PlayerScript player, string highest)
    {
        if (!isServer)
            deck.GetComponent<DeckScript>().CreateDeck(highest, this.GetComponent<InstantiatePrefab>().cardPrefab);
    }

    //List<GameObject> hand, GameObject cardToSpawn
    [ClientRpc]
    public void AdjustCards(List<GameObject> hand, int pNum)
    {
        switch (pNum)
        {
            case 1:
                hand[hand.Count - 1].transform.position = new Vector3((hand.Count * 1.25f) + 3.5f, -2, 0);
                hand[hand.Count - 1].transform.localScale -= new Vector3(0.2f, 0.2f, 0);
                hand[hand.Count - 1].GetComponent<SpriteRenderer>().sortingOrder = 8 - hand.Count;
                hand[hand.Count - 1].GetComponent<CardScript>().sortingDefault = hand[hand.Count - 1].GetComponent<SpriteRenderer>().sortingOrder;
                break;
            case 2:
                hand[hand.Count - 1].transform.position = new Vector3(18, (hand.Count * 1.25f) + .25f, 0);
                hand[hand.Count - 1].transform.Rotate(0, 0, 90);
                hand[hand.Count - 1].transform.localScale -= new Vector3(0.2f, 0.2f, 0);
                hand[hand.Count - 1].GetComponent<SpriteRenderer>().sortingOrder = 8 - hand.Count;
                hand[hand.Count - 1].GetComponent<CardScript>().sortingDefault = hand[hand.Count - 1].GetComponent<SpriteRenderer>().sortingOrder;
                break;
            case 3:
                hand[hand.Count - 1].transform.position = new Vector3(16 - (hand.Count * 1.25f), 14, 0);
                hand[hand.Count - 1].transform.Rotate(0, 0, 180);
                hand[hand.Count - 1].transform.localScale -= new Vector3(0.2f, 0.2f, 0);
                hand[hand.Count - 1].GetComponent<SpriteRenderer>().sortingOrder = 8 - hand.Count;
                hand[hand.Count - 1].GetComponent<CardScript>().sortingDefault = hand[hand.Count - 1].GetComponent<SpriteRenderer>().sortingOrder;
                break;
            case 4:
                hand[hand.Count - 1].transform.position = new Vector3(1, 10 - (hand.Count * 1.25f), 0);
                hand[hand.Count - 1].transform.Rotate(0, 0, 270);
                hand[hand.Count - 1].transform.localScale -= new Vector3(0.2f, 0.2f, 0);
                hand[hand.Count - 1].GetComponent<SpriteRenderer>().sortingOrder = 8 - hand.Count;
                hand[hand.Count - 1].GetComponent<CardScript>().sortingDefault = hand[hand.Count - 1].GetComponent<SpriteRenderer>().sortingOrder;
                break;
        }
        hand[hand.Count - 1].GetComponent<CardScript>().Flip();
        
    }

    [Command(requiresAuthority = false)]
    public void Enlarge(PlayerScript player, int playerNum)
    {
        RpcEnlarge(player.connectionToClient, hand1, hand2, hand3, hand4, playerNum);
    }
    
    [TargetRpc]
    public void RpcEnlarge(NetworkConnection conn, List<GameObject> hand1, List<GameObject> hand2, List<GameObject> hand3, List<GameObject> hand4, int playerNum)
    {
        List<GameObject> thisHand;

        switch(playerNum)
        {
            case 1:
                thisHand = hand1;
                break;
            case 2:
                thisHand = hand2;
                break;
            case 3:
                thisHand = hand3;
                break;
            case 4:
                thisHand = hand4;
                break;
            default:
                thisHand = hand1;
                break;
        }

        for (int i = 0; i < thisHand.Count; i++)
        {
            if (thisHand[i].GetComponent<CardScript>().hasAuthority)
                thisHand[i].GetComponent<CardScript>().Enlarge();
        }
        
    }

    [Command(requiresAuthority = false)]
    public void CmdIncrement(string butName, PlayerScript player, int playerNum)
    {
        if (player.Available > 0)
        {
            player.Available--;
            switch (butName)
            {
                case "addCharisma":
                    player.Charisma++;
                    RpcTextEditor(player.connectionToClient, "Charisma ", player.Charisma, "CharismaCounter" + playerNum);
                    break;

                case "addCunning":
                    player.Cunning++;
                    RpcTextEditor(player.connectionToClient, "Cunning ", player.Cunning, "CunningCounter" + playerNum);
                    break;

                case "addStrength":
                    player.Strength++;
                    RpcTextEditor(player.connectionToClient, "Strength ", player.Strength, "StrengthCounter" + playerNum);
                    break;

                case "addIntelligence":
                    player.Intelligence++;
                    RpcTextEditor(player.connectionToClient, "Intelligence ", player.Intelligence, "IntelligenceCounter" + playerNum);
                    break;
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdDecrement(string butName, PlayerScript player, int playerNum)
    {
        if (player.Available != player.Max)
        {
            switch (butName)
            {
                case "subCharisma":
                    if (player.Charisma > 0)
                    {
                        player.Charisma--;
                        player.Available++;
                        RpcTextEditor(player.connectionToClient, "Charisma ", player.Charisma, "CharismaCounter" + playerNum);
                    }
                    break;

                case "subCunning":
                    if (player.Cunning > 0)
                    {
                        player.Cunning--;
                        player.Available++;
                        RpcTextEditor(player.connectionToClient, "Cunning ", player.Cunning, "CunningCounter" + playerNum);
                    }
                    break;

                case "subStrength":
                    if (player.Strength > 0)
                    {
                        player.Strength--;
                        player.Available++;
                        RpcTextEditor(player.connectionToClient, "Strength ", player.Strength, "StrengthCounter" + playerNum);
                    }
                    break;

                case "subIntelligence":
                    if (player.Intelligence > 0)
                    {
                        player.Intelligence--;
                        player.Available++;
                        RpcTextEditor(player.connectionToClient, "Intelligence ", player.Intelligence, "IntelligenceCounter" + playerNum);
                    }
                    break;
            }
        }
    }

    [TargetRpc]
    public void RpcTextEditor(NetworkConnection conn, string stat, int num, string txtName)
    {
        GameObject.Find(txtName).GetComponent<Text>().text = stat + num.ToString();
    }

    [Command(requiresAuthority = false)]
    public void CmdTrackSelected(PlayerScript player)
    {
        for (int i = 0; i < player.hand.Count; i++)
        {
            if (player.hand[i].GetComponent<CardScript>().selected && !player.hand[i].GetComponent<CardScript>().prevSelected)
            {
                player.numSelected++;
                player.hand[i].GetComponent<CardScript>().prevSelected = true;
            }
            else if (!player.hand[i].GetComponent<CardScript>().selected && player.hand[i].GetComponent<CardScript>().prevSelected)
            {
                player.numSelected--;
                player.hand[i].GetComponent<CardScript>().prevSelected = false;
            }
        }
        if (player.numSelected == 3)
        {
            //show lockIn button
        }
        else
        {
            //hide lockIn button
        }
    }
}
