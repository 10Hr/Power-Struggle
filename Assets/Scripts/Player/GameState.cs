using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//List of game states
public enum GameStates
{
    Setup, //players picking stats
    Passive, //players picking passive
    StartEvent, //starting event
    Turn, //turn
    Event //event
}

public class GameState : NetworkBehaviour
{
    public GameStates currentState;
    public PlayerList playerList;
    private bool allReady = false;
    private bool allDrawn = false;
    private bool passivesSelected = false;

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
        currentState = GameStates.Setup;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
    }

    // Update is called once per frame
    //Handles the Finite State Machine
    void Update()
    {
        if (!allReady && playerList.players[0].ready && playerList.players[1].ready 
            && playerList.players[2].ready && playerList.players[3].ready)
        {
            allReady = true;
        }
        
        switch (currentState)
        {
            
            case GameStates.Setup:
                if (allReady)
                {
                    Debug.Log("Switching Game State");
                    currentState = GameStates.Passive;
                }
                break;

            case GameStates.Passive:
                if (passivesSelected)
                {
                    Debug.Log("Switching Game State");
                    //make start event later
                    currentState = GameStates.Turn;
                }
                break;

            case GameStates.StartEvent:
                break;

            case GameStates.Turn:
                break;

            case GameStates.Event:
                break;

        }
    }
}
