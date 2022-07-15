using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class PlayerManager : NetworkBehaviour {

    NetworkIdentity player1ID;
    NetworkIdentity player2ID;
    NetworkIdentity player3ID;
    NetworkIdentity player4ID;
    List<NetworkIdentity> playerIDs;
    //[SyncVar]
    //List<GameObject> decks;
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
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

    string thisName;
    NetworkIdentity[] listObjects;
    StatManager stats;
    GameState gameManager;

    bool p1ready, p2ready, p3ready, p4ready;



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
        //decks = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() { listObjects = FindObjectsOfType<NetworkIdentity>(); }

    public int AddNet(NetworkIdentity playerID) {
        playerIDs.Add(playerID);


        if (playerIDs[0] != null && playerIDs.Count == 1) {
            player1ID = playerIDs[0];
            setPlayer(0);
        }
        else if (playerIDs[1] != null && playerIDs.Count == 2) {
            player2ID = playerIDs[1];
            //            Debug.Log("Player 2 is " + player2ID);
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

        //stats.beginFind(); //error causer
        return playerIDs.Count;

    }

    public void setPlayer(int whichPlayer) {
        //each time a player connects, assign their IDs to fields
        updateList();

        foreach (NetworkIdentity obj in listObjects) {    // gets player object    
                                                          // Debug.Log("obj.GetComponent<NetworkIdentity>() " + obj.GetComponent<NetworkIdentity>() + " player1ID " + player1ID + " playercount " + playerCount + " whichPlayer " + whichPlayer);
            if (obj.GetComponent<NetworkIdentity>() == player1ID && playerCount == 1 && whichPlayer == 0) {
                player1 = obj.gameObject;
                Debug.Log("Player 1 is created!");
            }
            else if (obj.GetComponent<NetworkIdentity>() == player2ID && playerCount == 2 && whichPlayer == 1) {
                player2 = obj.gameObject;
                Debug.Log("Player 2 is created!");
            }
            else if (obj.GetComponent<NetworkIdentity>() == player3ID && playerCount == 3 && whichPlayer == 2) {
                player3 = obj.gameObject;
                Debug.Log("Player 3 is created!");
            }
            else if (obj.GetComponent<NetworkIdentity>() == player4ID && playerCount == 4 && whichPlayer == 3) {
                player4 = obj.gameObject;
                Debug.Log("Player 4 is created!");
            }
        }
    }

    public GameObject getPlayer(int whichPlayer) {
        if (whichPlayer == 0)
            return player1;
        else if (whichPlayer == 1)
            return player2;
        else if (whichPlayer == 2)
            return player3;
        else if (whichPlayer == 3)
            return player4;
        else
            return null;
    }

    public void readyUP() {
        string bntTag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;
        if (bntTag == "Player1") {
            if (player1.GetComponent<PlayerScript>().ReadyUp())
                p1ready = true;
        }
        else if (bntTag == "Player2") {
            if (player2.GetComponent<PlayerScript>().ReadyUp())
                p2ready = true;
        }
        else if (bntTag == "Player3") {
            if (player3.GetComponent<PlayerScript>().ReadyUp())
                p3ready = true;
        }
        else if (bntTag == "Player4") {
            if (player4.GetComponent<PlayerScript>().ReadyUp())
                p4ready = true;
        }

        if (p1ready && p2ready && p3ready && p4ready) {
            Debug.Log("All players are ready!");
            //start game
            gameManager.AllReady = true;
        }

    }

    //Find decks objects in scene
    //Depending on player spawn that deck
    //create deck properties for both server and all clients
    public void DeckMaker(string highest, int pNum)
    {
        //get deck objects
        deck1 = GameObject.Find("Deck1");
        deck2 = GameObject.Find("Deck2");
        deck3 = GameObject.Find("Deck3");
        deck4 = GameObject.Find("Deck4");

        //depending on player
        switch (pNum)
        {
            case 1:
                //create deck properties for server and clients
                RPCGiveDeck(deck1, player1, highest);
                deck1.GetComponent<DeckScript>().CreateDeck(highest, this.GetComponent<InstantiatePrefab>().cardPrefab);

                //spawns all the cards in that deck
                for (int i = 0; i < deck1.GetComponent<DeckScript>().cards.Count; i++)
                    NetworkServer.Spawn(deck1.GetComponent<DeckScript>().cards[i]);
                break;

            case 2:
                RPCGiveDeck(deck2, player2, highest);
                deck2.GetComponent<DeckScript>().CreateDeck(highest, this.GetComponent<InstantiatePrefab>().cardPrefab);
                for (int i = 0; i < deck2.GetComponent<DeckScript>().cards.Count; i++) 
                    NetworkServer.Spawn(deck2.GetComponent<DeckScript>().cards[i]);
                break;

            case 3:
                RPCGiveDeck(deck3, player3, highest);
                deck3.GetComponent<DeckScript>().CreateDeck(highest, this.GetComponent<InstantiatePrefab>().cardPrefab);
                for (int i = 0; i < deck3.GetComponent<DeckScript>().cards.Count; i++)
                    NetworkServer.Spawn(deck3.GetComponent<DeckScript>().cards[i]);
                break;

            case 4:
                RPCGiveDeck(deck4, player4, highest);
                deck4.GetComponent<DeckScript>().CreateDeck(highest, this.GetComponent<InstantiatePrefab>().cardPrefab);
                for (int i = 0; i < deck4.GetComponent<DeckScript>().cards.Count; i++)
                    NetworkServer.Spawn(deck4.GetComponent<DeckScript>().cards[i]);
                break;

            default:
                break;
        }
    }

    
    //Creates players hands depending on which player is ready to draw
    //draws the cards after hand is created
    //draws cards 1 at a time (1 per call of this method)
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
        Debug.Log(conn);
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
                    //Debug.Log(hand1[hand1.Count - 1].GetComponent<CardScript>().netIdentity);
                    AdjustCards(hand1, pNum);
                    //hand1[hand1.Count - 1].transform.position = new Vector3(hand1.Count * 2, 0, 0);
                    break;
                case 2:
                    cardToSpawn.GetComponent<CardScript>().Effect = hand2[hand2.Count - 1].GetComponent<CardScript>().Effect;
                    cardToSpawn.GetComponent<CardScript>().Title = hand2[hand2.Count - 1].GetComponent<CardScript>().Title;
                    cardToSpawn.GetComponent<CardScript>().Stat = hand2[hand2.Count - 1].GetComponent<CardScript>().Stat;
                    hand2[hand2.Count - 1] = cardToSpawn;
                    //hand2[hand2.Count - 1].transform.position = new Vector3(hand2.Count * 2, 0, 0);
                    AdjustCards(hand2, pNum);
                    break;
                case 3:
                    cardToSpawn.GetComponent<CardScript>().Effect = hand3[hand3.Count - 1].GetComponent<CardScript>().Effect;
                    cardToSpawn.GetComponent<CardScript>().Title = hand3[hand3.Count - 1].GetComponent<CardScript>().Title;
                    cardToSpawn.GetComponent<CardScript>().Stat = hand3[hand3.Count - 1].GetComponent<CardScript>().Stat;
                    hand3[hand3.Count - 1] = cardToSpawn;
                    //hand3[hand3.Count - 1].transform.position = new Vector3(hand3.Count * 2, 0, 0);
                    AdjustCards(hand3, pNum);
                    break;
                case 4:
                    cardToSpawn.GetComponent<CardScript>().Effect = hand4[hand4.Count - 1].GetComponent<CardScript>().Effect;
                    cardToSpawn.GetComponent<CardScript>().Title = hand4[hand4.Count - 1].GetComponent<CardScript>().Title;
                    cardToSpawn.GetComponent<CardScript>().Stat = hand4[hand4.Count - 1].GetComponent<CardScript>().Stat;
                    hand4[hand4.Count - 1] = cardToSpawn;
                    //hand4[hand4.Count - 1].transform.position = new Vector3(hand4.Count * 2, 0, 0);
                    AdjustCards(hand4, pNum);
                    break;
            }


            //TODO

            //Debug.Log(hand[hand.Count - 1]);
            /////////////////////////////////////CMDSpawnCard(deck, pNum);
            //Debug.Log(hand[hand.Count - 1]);
            //cardReplacement.AddComponent<CardScript>();
            //cardReplacement.GetComponent<CardScript>().Effect = hand[hand.Count - 1].GetComponent<CardScript>().Effect;
            //cardReplacement.GetComponent<CardScript>().Title = hand[hand.Count - 1].GetComponent<CardScript>().Title;
            //cardReplacement.GetComponent<CardScript>().Stat = hand[hand.Count - 1].GetComponent<CardScript>().Stat;
            //cardReplacement.transform.position = new Vector3(hand.Count * 2, 0, 0);
            //Debug.Log("Adjusted position");
        }
    }

    //Is Called on all clients and server
    //doesnt work on server to prevent the host having multiple copies
    //creates deck properties for clients
    [ClientRpc]
    public void RPCGiveDeck(GameObject deck, GameObject player, string highest)
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
                break;
            case 2:
                hand[hand.Count - 1].transform.position = new Vector3(18, (hand.Count * 1.25f) + .25f, 0);
                hand[hand.Count - 1].transform.Rotate(0, 0, 90);
                hand[hand.Count - 1].transform.localScale -= new Vector3(0.2f, 0.2f, 0);
                break;
            case 3:
                hand[hand.Count - 1].transform.position = new Vector3(16 - (hand.Count * 1.25f), 14, 0);
                hand[hand.Count - 1].transform.Rotate(0, 0, 180);
                hand[hand.Count - 1].transform.localScale -= new Vector3(0.2f, 0.2f, 0);
                break;
            case 4:
                hand[hand.Count - 1].transform.position = new Vector3(1, 10 - (hand.Count * 1.25f), 0);
                hand[hand.Count - 1].transform.Rotate(0, 0, 270);
                hand[hand.Count - 1].transform.localScale -= new Vector3(0.2f, 0.2f, 0);
                break;
        }

        //if (hasAuthority)
        //{
       // Debug.Log(hand[hand.Count - 1].GetComponent<NetworkIdentity>().connectionToClient);
        //Debug.Log(hand[hand.Count - 1].GetComponent<NetworkIdentity>().hasAuthority);
        hand[hand.Count - 1].GetComponent<CardScript>().flip();
        //}
        
    }
}
