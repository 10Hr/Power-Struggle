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
    private bool allReady = false;
    private bool allDrawn = false;
    private bool passivesSelected = false;

    //properties
    public bool AllReady
    {
        get { return allReady; }
        set { allReady = value; }
    }
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        Debug.Log("Game Started!");
        currentState = GameStates.Setup;
    }

    // Update is called once per frame
    //Handles the Finite State Machine
    void Update()
    {
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
