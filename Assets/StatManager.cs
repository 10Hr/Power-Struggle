using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class StatManager : NetworkBehaviour
{
    public PlayerManager playerManager;
    PassiveManager passiveManager;
    List<Passive> passives;

    [SyncVar]
    private int count;

    bool selected = false;

    public void getCount() { count++; }

    void Awake()
    {
        //playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
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
                playerManager.CmdIncrement(thisButName, playerManager.player1, 1);
                break;
            case "Player2":
                playerManager.CmdIncrement(thisButName, playerManager.player2, 2);
                break;
            case "Player3":
                playerManager.CmdIncrement(thisButName, playerManager.player3, 3);
                break;
            case "Player4":
                playerManager.CmdIncrement(thisButName, playerManager.player4, 4);
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
                playerManager.CmdDecrement(thisButName, playerManager.player1, 1);
                break;
            case "Player2":
                playerManager.CmdDecrement(thisButName, playerManager.player2, 2);
                break;
            case "Player3":
                playerManager.CmdDecrement(thisButName, playerManager.player3, 3);
                break;
            case "Player4":
                playerManager.CmdDecrement(thisButName, playerManager.player4, 4);
                break;
        }
    }

    public void readyUP()
    {
        string bntTag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;
        if (bntTag == "Player1")
        {
            if (playerManager.player1.ReadyUp())
            {
                playerManager.CmdReadyPlayer(bntTag);
            }
        }
        else if (bntTag == "Player2")
        {
            if (playerManager.player2.ReadyUp())
            {
                playerManager.CmdReadyPlayer(bntTag);
            }
        }
        else if (bntTag == "Player3")
        {
            if (playerManager.player3.ReadyUp())
            {
                playerManager.CmdReadyPlayer(bntTag);
            }
        }
        else if (bntTag == "Player4")
        {
            if (playerManager.player4.ReadyUp())
            {
                playerManager.CmdReadyPlayer(bntTag);
            }
        }
    }



    public void selectPassive() {

        // get
        string btnName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
       // GameObject btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        
        //passives = passiveManager.getChoices();
        passiveManager.getChoices();

        selected = !selected;
        Debug.Log("selected: " + selected);
        for (int i = 0; i < passives.Count; i++)
        {  
         //   btn.GetComponent<Text>().text = p.PassiveName;
         
        }
    }

    public void lockIn()
    {
        string btnName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        
        for (int i = 1; i < 5; i++)
        {
            if (btnName == "LockInButton" + i)
            {
                playerManager.CmdLockIn(i - 1);
            }
        }
    }
}