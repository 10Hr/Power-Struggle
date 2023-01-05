using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//List of game states
public enum GameStates
{
    FillingLobby, //Waiting for 4 players to connect
    Setup, //players picking stats
    Passive, //players picking passive
    StartEvent, //starting event
    DrawCards, //Draw self cards
    LoadEnemyCards, //Load enemy cards
    Turn, //turn
    Event //event
}

public class GameState : NetworkBehaviour
{
    [SyncVar]
    public GameStates currentState;

    [SyncVar]
    public PlayerScript currentPlayer;

    public PlayerList playerList;
    private bool allReady = false;
    private bool allDrawn = false;
    private bool passivesSelected = false;
    private bool allConnected = false;
    int turn = 1;
    public bool PassivesSelected
    {
        set { passivesSelected = value; }
    }
    public GameStates CurrentState
    {
        get { return currentState; }
    }
    public bool AllDrawn
    {
        set { allDrawn = value; }
        get { return allDrawn; }
    }

    void Awake() {
        Debug.Log("Game Started!");
        currentState = GameStates.FillingLobby;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
        Debug.Log(playerList);
    }

    // Update is called once per frame
    //Handles the Finite State Machine
    void Update()
    {
        if (!isServer)
            return;

        switch (currentState)
        {
            case GameStates.FillingLobby:
                if(allConnected)
                {
                    currentState = GameStates.Setup;
                }
                else
                {
                    if (playerList.players.Count == 4)
                        allConnected = true;
                }
                break;
            
            case GameStates.Setup:
                if (!allReady && playerList.players[0].ready && playerList.players[1].ready
                    && playerList.players[2].ready && playerList.players[3].ready)
                      allReady = true;

                if (allReady)
                {
                    currentState = GameStates.Passive;
                }
                break;

            case GameStates.Passive:
                if (playerList.players[0].hasPassive && playerList.players[1].hasPassive 
                    && playerList.players[2].hasPassive && playerList.players[3].hasPassive)
                {
                    currentState = GameStates.DrawCards;
                }
                break;

            case GameStates.StartEvent:
                break;

            case GameStates.DrawCards:
                if (playerList.players[0].cardsSpawned && playerList.players[1].cardsSpawned
                    && playerList.players[2].cardsSpawned && playerList.players[3].cardsSpawned)
                {
                    currentState = GameStates.LoadEnemyCards;
                }
                break;
            case GameStates.LoadEnemyCards:
                if (playerList.players[0].LockedIn && playerList.players[1].LockedIn
                    && playerList.players[2].LockedIn && playerList.players[3].LockedIn)
                {
                    currentState = GameStates.Turn;
                }
                break;
            case GameStates.Turn:
                if (turn == 12) {
                    turn = 1;
                    currentState = GameStates.Event;
                } else {
                    currentPlayer = playerList.players[turn % 4];
                    currentPlayer.Turn();
                    turn++;
                }

                break;

            case GameStates.Event:
                break;

        }
    }
}
