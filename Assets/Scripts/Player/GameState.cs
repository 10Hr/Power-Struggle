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
    public bool EventTwo = false;

    [SyncVar]
    public PlayerScript currentPlayer;

    [SyncVar]
    public bool EndEvent = false;

    public PlayerList playerList;
    public EventManager eMan;
    private bool allReady = false;
    private bool allDrawn = false;
    private bool passivesSelected = false;
    private bool allConnected = false;
    public LeaderBoardManager leaderBoard;

    [SyncVar]
    public int turn = 0;
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
        eMan = GameObject.Find("EventManager").GetComponent<EventManager>();
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
                    eMan.GetEvents();
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
                    currentPlayer = playerList.players[0];
                    currentState = GameStates.Turn;
                }
                break;
            case GameStates.Turn:
                if (turn == 12) {
                    turn = 0;
                    foreach (PlayerScript p in playerList.players)
                    {
                        p.setUntargetable(false);
                        p.CmdDisablePLoss(false);
                        p.CmdDisableSLoss(false);
                        p.CmdSetSawDeck(false);
                        p.CmdSelectedTrg(false);
                    }
                    currentState = GameStates.Event;
                } else {
                    currentPlayer = playerList.players[turn % 4];
                    if (currentPlayer.sawDeck == false)
                    {
                        currentPlayer.Turn(currentPlayer.connectionToClient);
                    }
                }

                break;

            case GameStates.Event:
                if (EndEvent)
                {
                    currentState = GameStates.LoadEnemyCards;
                    EndEvent = false;
                    break;
                }
                if (eMan.currentEvent == "")
                {
                    EventTwo = false;
                    eMan.SelectEvent();
                }
                break;

        }
    }
}
