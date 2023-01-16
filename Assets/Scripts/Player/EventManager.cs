using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EventManager : NetworkBehaviour
{
    readonly public SyncList<string> eventList = new SyncList<string>();

    [SyncVar]
    public string currentEvent;

    public PlayerList playerList;
    GameObject ReadyButton;
    GameObject[] gos;
    GameObject[] e1gs;
    GameObject[] e2gs;
    GameObject[] e3gs;
    GameState FSM;
    private bool ran = false;

    private void Start()
    {
        playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
        ReadyButton = GameObject.Find("Ready");
        gos = GameObject.FindGameObjectsWithTag("eventLabels");
        e1gs = GameObject.FindGameObjectsWithTag("e1g");
        e2gs = GameObject.FindGameObjectsWithTag("e2g");
        e3gs = GameObject.FindGameObjectsWithTag("e3g");
        FSM = GameObject.Find("FSM").GetComponent<GameState>();

        foreach (GameObject g in e1gs)
            g.SetActive(false);
        foreach (GameObject g in e2gs)
            g.SetActive(false);
        foreach (GameObject g in e3gs)
            g.SetActive(false);
        foreach (GameObject g in gos)
            g.SetActive(false);
    }

    private void Update()
    {
        switch (currentEvent)
        {
            case "One":
                if (playerList.players[0].ready && playerList.players[1].ready
                    && playerList.players[2].ready && playerList.players[3].ready)
                {
                    ReadyButton.SetActive(false);
                    foreach (GameObject g in e1gs)
                        g.SetActive(true);
                    foreach (GameObject g in e2gs)
                        g.SetActive(true);
                    foreach (GameObject g in e3gs)
                        g.SetActive(true);
                }
                if (playerList.players[0].guess1 != "" && playerList.players[0].guess2 != "" && playerList.players[0].guess3 != "" &&
                    playerList.players[1].guess1 != "" && playerList.players[1].guess2 != "" && playerList.players[1].guess3 != "" &&
                    playerList.players[2].guess1 != "" && playerList.players[2].guess2 != "" && playerList.players[2].guess3 != "" &&
                    playerList.players[3].guess1 != "" && playerList.players[3].guess2 != "" && playerList.players[3].guess3 != "")
                {
                    CmdEndEvent();
                }
                break;
            case "Two":
                break;
            default:
                break;
        }
    }

    [Command (requiresAuthority = false)]
    public void GetEvents()
    {
        eventList.Add("One");
        eventList.Add("Two");
        eventList.Add("Three");
    }

    [Command (requiresAuthority = false)]
    public void SelectEvent()
    {
        currentEvent = eventList[0];
        RunEvent();
    }

    [Command(requiresAuthority = false)]
    public void RunEvent()
    {
        if (!ran)
        {
            ran = true;
            switch (currentEvent)
            {
                case "One":
                    RpcSpawnReady();
                    foreach (PlayerScript p in playerList.players)
                    {
                        p.ready = false;
                        p.ResetStats();
                    }
                    break;
                case "Two":
                    
                    break;
                default:
                    break;
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdEndEvent()
    {
        eventList.Remove(eventList[0]);
        FSM.EndEvent = true;
        currentEvent = "";

        RpcDespawnAll();

        playerList.players[0].CheckGuess(playerList.players[0].connectionToClient);
        playerList.players[1].CheckGuess(playerList.players[1].connectionToClient);
        playerList.players[2].CheckGuess(playerList.players[2].connectionToClient);
        playerList.players[3].CheckGuess(playerList.players[3].connectionToClient);

        playerList.players[0].setUntargetable(false);
        playerList.players[1].setUntargetable(false);
        playerList.players[2].setUntargetable(false);
        playerList.players[3].setUntargetable(false);
        playerList.players[0].CmdDisablePLoss(false);
        playerList.players[1].CmdDisablePLoss(false);
        playerList.players[2].CmdDisablePLoss(false);
        playerList.players[3].CmdDisablePLoss(false);
        playerList.players[0].CmdDisableSLoss(false);
        playerList.players[1].CmdDisableSLoss(false);
        playerList.players[2].CmdDisableSLoss(false);
        playerList.players[3].CmdDisableSLoss(false);
    }

    [ClientRpc]
    public void RpcSpawnReady()
    {
        ReadyButton.SetActive(true);
    }

    [ClientRpc]
    public void RpcDespawnAll()
    {
        foreach (GameObject g in e1gs)
            g.SetActive(false);
        foreach (GameObject g in e2gs)
            g.SetActive(false);
        foreach (GameObject g in e3gs)
            g.SetActive(false);
        foreach (GameObject g in gos)
            g.SetActive(false);
    }
}
