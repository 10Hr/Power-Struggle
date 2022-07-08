using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//List of game states
public enum GameStates
{
    Setup, //players picking stats
    Passive, //players picking passive
    StartEvent, //starting event
    Turn, //turn
    Event //event
}

public class GameState : MonoBehaviour
{
    private GameStates currentState;
    private bool allReady = false;
    private bool passivesSelected = false;

    //properties
    public bool AllReady
    {
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
                    currentState = GameStates.Passive;
                }
                break;

            case GameStates.Passive:
                if (passivesSelected)
                {
                    currentState = GameStates.StartEvent;
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
