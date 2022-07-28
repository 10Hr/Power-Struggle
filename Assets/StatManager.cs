using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class StatManager : NetworkBehaviour
{
    private PlayerScript player1;
    private PlayerScript player2;
    private PlayerScript player3;
    private PlayerScript player4;
    PlayerManager playerManager;
    [SyncVar]
    private int count;

    public void getCount() { count++; }

    void Awake()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        //GameObject.Find("Main Camera").SetActive(false);
    }

    private void Update()
    {
        //NEVER HAPPENS
        if (player4 != null)
        {
            Debug.Log("ALL 4 ARE HERE");
        }
    }

    public void Increment()
    {
        //Get the button that is being pressed
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string thisButName = thisButton.name;
        string buttontag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

        switch (buttontag)
        {
            case "Player1":
                player1 = playerManager.getPlayer(0);
                playerManager.CmdIncrement(thisButName, player1, 1);
                break;
            case "Player2":
                player2 = playerManager.getPlayer(1);
                playerManager.CmdIncrement(thisButName, player2, 2);
                break;
            case "Player3":
                player3 = playerManager.getPlayer(2);
                playerManager.CmdIncrement(thisButName, player3, 3);
                break;
            case "Player4":
                player4 = playerManager.getPlayer(3);
                playerManager.CmdIncrement(thisButName, player4, 4);
                break;
        }
    }

    // make more efficient later
    public void Decrement()
    {
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string thisButName = thisButton.name;
        string buttontag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

        switch (buttontag)
        {
            case "Player1":
                player1 = playerManager.getPlayer(0);
                playerManager.CmdDecrement(thisButName, player1, 1);
                break;
            case "Player2":
                player2 = playerManager.getPlayer(1);
                playerManager.CmdDecrement(thisButName, player2, 2);
                break;
            case "Player3":
                player3 = playerManager.getPlayer(2);
                playerManager.CmdDecrement(thisButName, player3, 3);
                break;
            case "Player4":
                player4 = playerManager.getPlayer(3);
                playerManager.CmdDecrement(thisButName, player4, 4);
                break;
        }
    }

    public void readyUP()
    {
        string bntTag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;
        if (bntTag == "Player1")
        {
            if (player1.ReadyUp())
            {
                playerManager.CmdReadyPlayer(bntTag);
            }
        }
        else if (bntTag == "Player2")
        {
            if (player2.ReadyUp())
            {
                Debug.Log("player 2 ready called");
                playerManager.CmdReadyPlayer(bntTag);
            }
        }
        else if (bntTag == "Player3")
        {
            if (player3.ReadyUp())
            {
                playerManager.CmdReadyPlayer(bntTag);
            }
        }
        else if (bntTag == "Player4")
        {
            if (player4.ReadyUp())
            {
                playerManager.CmdReadyPlayer(bntTag);
            }
        }
    }
}
