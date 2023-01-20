using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class StatManager : NetworkBehaviour
{
    [SerializeField]
    PlayerList playerList;

    public PassiveManager passiveManager;
    public GameState gameState;
    public EventManager eMan;

    void Start() {
        gameState = GameObject.Find("FSM").GetComponent<GameState>();
        eMan = GameObject.Find("EventManager").GetComponent<EventManager>();
    }
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
        if (p.AvailablePoints == 0)
            GameObject.Find("Ready").SetActive(false);
        CmdReadyUp(p);
    }

    public void Bet()
    {
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        //TextMeshProUGUI txt = thisButton.GetComponentInChildren<TextMeshProUGUI>();
        PlayerScript p = GetPlayer();
        CmdBet(p, thisButton, p.currentBet);
        p.currentBet = 0;
        thisButton.GetComponentInChildren<TextMeshProUGUI>().text = "Bet: 0";
    }

    [Command(requiresAuthority = false)]
    public void CmdBet(PlayerScript p, GameObject button, int bet)
    {
        switch (p.playerName)
        {
            case "Player 1":
            case "Player 4":
                p.Power = -bet;
                eMan.OneFourTotal += bet;
                break;

            case "Player 2":
            case "Player 3":
                p.Power = -bet;
                eMan.TwoThreeTotal += bet;
                break;
        }
        eMan.currentIndex++;
        if (bet == 0)
        {
            eMan.turnsInARow++;
        }
        else
        {
            eMan.turnsInARow = 0;
        }
    }

    public void ModifyBet()
    {
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string thisButName = thisButton.name;
        PlayerScript p = GetPlayer();
        switch (thisButName)
        {
            case "Add":
                if (p.currentBet < p.Power)
                    p.currentBet += 25;
                break;
            case "Sub":
                if (p.currentBet > 0)
                    p.currentBet -= 25;
                break;
        }
        GameObject.Find("Bet").GetComponentInChildren<TextMeshProUGUI>().text = "Bet: " + p.currentBet;
    }

    public void Guess()
    {
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string thisButName = thisButton.name;
        string buttontag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;
        PlayerScript p = GetPlayer();
        CmdGuess(p, buttontag, thisButName);
    }

    [Command(requiresAuthority = false)]
    public void CmdGuess(PlayerScript p, string butTag, string butName)
    {
        switch (butTag)
        {
            case "e1g":
                p.guess1 = butName;
                break;
            case "e2g":
                p.guess2 = butName;
                break;
            case "e3g":
                p.guess3 = butName;
                break;
            default:
                break;
        }    
    }


    [Command(requiresAuthority = false)]
    public void CmdReadyUp(PlayerScript p)
    {
        if (p.AvailablePoints == 0)
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
                            if (eMan.currentEvent == "Three")
                            {
                                p.MaxPoints = -1;
                                p.AvailablePoints = -1;
                                p.Power = 50;
                            }
                        }
                        break;
                    case "Strength":
                        if (p.Strength > 0)
                        {
                            p.Strength = -1;
                            p.AvailablePoints = 1;
                            if (eMan.currentEvent == "Three")
                            {
                                p.MaxPoints = -1;
                                p.AvailablePoints = -1;
                                p.Power = 50;
                            }
                        }
                        break;
                    case "Intelligence":
                        if (p.Intelligence > 0)
                        {
                            p.Intelligence = -1;
                            p.AvailablePoints = 1;
                            if (eMan.currentEvent == "Three")
                            {
                                p.MaxPoints = -1;
                                p.AvailablePoints = -1;
                                p.Power = 50;
                            }
                        }
                        break;
                    case "Cunning":
                        if (p.Cunning > 0)
                        {
                            p.Cunning = -1;
                            p.AvailablePoints = 1;
                            if (eMan.currentEvent == "Three")
                            {
                                p.MaxPoints = -1;
                                p.AvailablePoints = -1;
                                p.Power = 50;
                            }
                        }
                        break;
                }
                break;
        }
        p.CalcLowest();
        p.CalcHighest();
    }

    public void ChangeStats() {
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string thisButName = thisButton.name;
        string buttontag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

        PlayerScript p = GetPlayer();

        CmdChangeStats(p, thisButName, buttontag);
    }

    public void LockIn()
    {
        PlayerScript p = GetPlayer();
        CmdLockIn(p);
    }

    [Command (requiresAuthority = false)]
    public void CmdLockIn(PlayerScript p)
    {
        if (p.threeSelected && gameState.CurrentState == GameStates.LoadEnemyCards)
        {
            p.LockedIn = true;
        }
    }
    public void GetPlayerData() {
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string buttontag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

        List<PlayerScript> playerslots = NetworkClient.localPlayer.GetComponent<PlayerScript>().sendPlayerData();
        switch(buttontag) {
            case "right":
                NetworkClient.localPlayer.GetComponent<PlayerScript>().deck.getTarget(playerslots[0]);
                break;
            case "left":
                NetworkClient.localPlayer.GetComponent<PlayerScript>().deck.getTarget(playerslots[2]);
                break;
            case "top":
                NetworkClient.localPlayer.GetComponent<PlayerScript>().deck.getTarget(playerslots[1]);
                break;

        }
    }

    public void selectPassive() {
        GameObject thisButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string buttontag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

        Debug.Log(thisButton.GetComponentInChildren<TextMeshProUGUI>().text);

        PlayerScript p = GetPlayer();

        switch (buttontag) {
            case "passiveChoice1":
                CmdSelectPassive(p, thisButton.GetComponentInChildren<TextMeshProUGUI>().text);
                break;
            case "passiveChoice2":
                CmdSelectPassive(p, thisButton.GetComponentInChildren<TextMeshProUGUI>().text);
                break;
            case "passiveChoice3":
                CmdSelectPassive(p, thisButton.GetComponentInChildren<TextMeshProUGUI>().text);
                break;
        }
        p.passiveOption1.SetActive(false);
        p.passiveOption2.SetActive(false);
        p.passiveOption3.SetActive(false);

    }

    [Command(requiresAuthority = false)]
    public void CmdSelectPassive(PlayerScript p, string name) {
        p.passive.passiveName = name;
    }

    [Command (requiresAuthority = false)]
    public void CmdSetPassiveName(PlayerScript p, string passiveName)
    {
        p.passiveName = passiveName;
    }
}