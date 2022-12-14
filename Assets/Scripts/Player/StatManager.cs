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

    public PlayerScript GetPlayer() {

        foreach (PlayerScript p in playerList.players)
            if (p.netId == NetworkClient.localPlayer.netId) {
                Debug.Log("Player" + (int)p.netId + "clicked this button.");
                return p;
            }
        return null; // we have a problem
    }

    public void ChangeStats() {
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string thisButName = thisButton.name;
        string buttontag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

        PlayerScript p = GetPlayer();

        switch (buttontag)
        {
            case "add":
                switch (thisButName)
                {
                    case "Charisma":
                        p.Charisma = 1;
                        break;
                    case "Strength":
                        p.Strength = 1;
                        break;
                    case "Intelligence":
                        p.Intelligence = 1;
                        break;
                    case "Cunning":
                        p.Cunning = 1;
                        break;
                }
                break;
            case "sub":
                switch (thisButName)
                {
                    case "Charisma":
                        p.Charisma = -1;
                        break;
                    case "Strength":
                        p.Strength = -1;
                        break;
                    case "Intelligence":
                        p.Intelligence = -1;
                        break;
                    case "Cunning":
                        p.Cunning = -1;
                        break;
                }
                break;
        }
    }
}