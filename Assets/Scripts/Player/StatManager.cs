using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.IO;

public class StatManager : NetworkBehaviour
{
    [SerializeField]
    PlayerList playerList;

    public void OnPointerClick()
    {
        Debug.Log(playerList.players[0].netIdentity.netId);
        //Debug.Log((int)NetworkServer.localConnection.identity.netId);
        Debug.Log(NetworkClient.localPlayer.netId);
        foreach (PlayerScript p in playerList.players)
        {
            if (p.netId == NetworkServer.localConnection.connectionId)
            {
                // test
                Debug.Log("Player" + (int)p.netId + "clicked this button.");
            }
        }
    }


    //public PlayerManager playerManager;
    //public PassiveManager passiveManager;
    //List<Passive> passives;
    
    //[SyncVar]
    //private int count;
    //public void getCount() { count++; }


    ////public void getPlayerNum() { playerNum = playerManager.playerNum; }

    //void Awake()
    //{
    //    //playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    //}

    //public void Increment()
    //{
    //    //Get the button that is being pressed
    //    GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
    //    string thisButName = thisButton.name;
    //    string buttontag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

    //    switch (buttontag)
    //    {
    //        case "Player1":
    //            playerManager.CmdIncrement(thisButName, playerManager.player1, 1);
    //            break;
    //        case "Player2":
    //            playerManager.CmdIncrement(thisButName, playerManager.player2, 2);
    //            break;
    //        case "Player3":
    //            playerManager.CmdIncrement(thisButName, playerManager.player3, 3);
    //            break;
    //        case "Player4":
    //            playerManager.CmdIncrement(thisButName, playerManager.player4, 4);
    //            break;
    //    }
    //}

    //// make more efficient later
    //public void Decrement()
    //{
    //    GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
    //    string thisButName = thisButton.name;
    //    string buttontag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

    //    switch (buttontag)
    //    {
    //        case "Player1":
    //            playerManager.CmdDecrement(thisButName, playerManager.player1, 1);
    //            break;
    //        case "Player2":
    //            playerManager.CmdDecrement(thisButName, playerManager.player2, 2);
    //            break;
    //        case "Player3":
    //            playerManager.CmdDecrement(thisButName, playerManager.player3, 3);
    //            break;
    //        case "Player4":
    //            playerManager.CmdDecrement(thisButName, playerManager.player4, 4);
    //            break;
    //    }
    //}

    //public void readyUP()
    //{
    //    string bntTag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;
    //    if (bntTag == "Player1")
    //    {
    //        if (playerManager.player1.ReadyUp())
    //        {
    //            playerManager.CmdReadyPlayer(bntTag);
    //        }
    //    }
    //    else if (bntTag == "Player2")
    //    {
    //        if (playerManager.player2.ReadyUp())
    //        {
    //            playerManager.CmdReadyPlayer(bntTag);
    //        }
    //    }
    //    else if (bntTag == "Player3")
    //    {
    //        if (playerManager.player3.ReadyUp())
    //        {
    //            playerManager.CmdReadyPlayer(bntTag);
    //        }
    //    }
    //    else if (bntTag == "Player4")
    //    {
    //        if (playerManager.player4.ReadyUp())
    //        {
    //            playerManager.CmdReadyPlayer(bntTag);
    //        }
    //    }
    //}

    //public void selectPassive() { // being called by button

    //    string bntName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name; //problem child for player 2
    //    string bntTag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

    //    passives = passiveManager.getChoices();
    //    for (int i = 0; i < passives.Count; i++) 
    //        if (bntName == "txtChoice" + (i + 1)) 
    //            for (int j = 1; j < 5; j++)
    //                if (bntTag == "Player" + j) 
    //                    playerManager.CmdGetPassive(passives[i].PassiveType, passives[i].PassiveName, passives[i].PassiveDescription, passives[i].PassiveEffect, int.Parse(bntTag.Substring(6)));
    //            //                         Debug.Log("Passive: " + passives[i].PassiveEffect + " tag = " + int.Parse(bntTag.Substring(6)));      
        
    //}

    //public void lockIn()
    //{
    //    string btnName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        
    //    for (int i = 1; i < 5; i++)
    //    {
    //        if (btnName == "LockInButton" + i)
    //        {
    //            playerManager.CmdLockIn(i - 1);
    //        }
    //    }
    //}
}