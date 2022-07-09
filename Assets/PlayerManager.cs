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
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
    [SyncVar]
    public int playerCount;
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
 
            foreach(NetworkIdentity obj in listObjects) {    // gets player object    
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
}
