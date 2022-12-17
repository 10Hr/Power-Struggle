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

    public PassiveManager passiveManager;

    public PlayerScript GetPlayer() {

        foreach (PlayerScript p in playerList.players)
            if (p.netId == NetworkClient.localPlayer.netId) {
                Debug.Log("Player" + (int)p.netId + "clicked this button.");
                return p;
            }
        return null; // we have a problem
    }

    public void ReadyUp()
    {
        PlayerScript p = GetPlayer();
        CmdReadyUp(p);
    }

    [Command(requiresAuthority = false)]
    public void CmdReadyUp(PlayerScript p)
    {
        p.ready = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdChangeStats(PlayerScript p, string thisButName, string buttontag)
    {
        switch (buttontag)
        {
            case "add":
                switch (thisButName)
                {
                    case "Charisma":
                        p.Charisma = 8;
                        p.AvailablePoints = -8;
                        break;
                    case "Strength":
                        p.Strength = 8;
                        p.AvailablePoints = -8;
                        break;
                    case "Intelligence":
                        p.Intelligence = 8;
                        p.AvailablePoints = -8;
                        break;
                    case "Cunning":
                        p.Cunning = 8;
                        p.AvailablePoints = -8;
                        break;
                }
                break;
            case "sub":
                switch (thisButName)
                {
                    case "Charisma":
                        if (p.Charisma > 0)
                        {
                            p.Charisma = -1;
                            p.AvailablePoints = 1;
                        }
                        break;
                    case "Strength":
                        if (p.Strength > 0)
                        {
                            p.Strength = -1;
                            p.AvailablePoints = 1;
                        }
                        break;
                    case "Intelligence":
                        if (p.Intelligence > 0)
                        {
                            p.Intelligence = -1;
                            p.AvailablePoints = 1;
                        }
                        break;
                    case "Cunning":
                        if (p.Cunning > 0)
                        {
                            p.Cunning = -1;
                            p.AvailablePoints = 1;
                        }
                        break;
                }
                break;
        }
    }

    public void ChangeStats() {
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string thisButName = thisButton.name;
        string buttontag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

        PlayerScript p = GetPlayer();

        CmdChangeStats(p, thisButName, buttontag);
    }

    public void selectPassive() {
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string buttontag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

        PlayerScript p = GetPlayer();

        switch (buttontag) {
            case "passiveChoice1":
                CmdSelectPassive(p, passiveManager.getChoiceList(0));
                Debug.Log(passiveManager.getChoiceList(0).PassiveName + " selected!");
                break;
            case "passiveChoice2":
                CmdSelectPassive(p, passiveManager.getChoiceList(1));
                Debug.Log(passiveManager.getChoiceList(1).PassiveName + " selected!");
                break;
            case "passiveChoice3":
                CmdSelectPassive(p, passiveManager.getChoiceList(2));
                Debug.Log(passiveManager.getChoiceList(2).PassiveName + " selected!");
                break;
        }

   
      //  CmdSetPassiveName(p, p.Passive.PassiveName);
    }

    [Command(requiresAuthority = false)]
    public void CmdSelectPassive(PlayerScript p, Passive ps) {
        p.Passive = ps;
        p.PassiveName = ps.PassiveName;

    }

    [Command (requiresAuthority = false)]
    public void CmdSetPassiveName(PlayerScript p, string passiveName)
    {
        p.passiveName = passiveName;
    }
}