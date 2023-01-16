using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class EventManager : NetworkBehaviour
{
    readonly public SyncList<string> eventList = new SyncList<string>();

    [SyncVar]
    public string currentEvent;

    [SyncVar]
    public PlayerScript currentBetter;

    [SyncVar]
    public int currentIndex = 0;

    [SyncVar]
    public int turnsInARow = 0;

    [SyncVar]
    public int OneFourTotal = 0;

    [SyncVar]
    public int TwoThreeTotal = 0;

    public PlayerList playerList;
    GameObject ReadyButton;
    GameObject[] gos;
    GameObject[] e1gs;
    GameObject[] e2gs;
    GameObject[] e3gs;
    GameState FSM;
    public GameObject BetButton;
    public GameObject BetSub;
    public GameObject BetAdd;
    private bool ran = false;

    public GameObject OneFour;
    public GameObject TwoThree;

    private void Start()
    {
        playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
        ReadyButton = GameObject.Find("Ready");
        gos = GameObject.FindGameObjectsWithTag("eventLabels");
        e1gs = GameObject.FindGameObjectsWithTag("e1g");
        e2gs = GameObject.FindGameObjectsWithTag("e2g");
        e3gs = GameObject.FindGameObjectsWithTag("e3g");
        FSM = GameObject.Find("FSM").GetComponent<GameState>();

        OneFour = GameObject.Find("1and4");
        TwoThree = GameObject.Find("2and3");

        BetButton = GameObject.Find("Bet");
        BetAdd = GameObject.Find("Add");
        BetSub = GameObject.Find("Sub");

        BetButton.SetActive(false);
        BetAdd.SetActive(false);
        BetSub.SetActive(false);

        foreach (GameObject g in e1gs)
            g.SetActive(false);
        foreach (GameObject g in e2gs)
            g.SetActive(false);
        foreach (GameObject g in e3gs)
            g.SetActive(false);
        foreach (GameObject g in gos)
            g.SetActive(false);

        TwoThree.SetActive(false);
        OneFour.SetActive(false);
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
            case "Three":
                RpcSpawnBet(currentBetter.connectionToClient);
                if (currentIndex == 4)
                    currentIndex = 0;
                currentBetter = playerList.players[currentIndex];
                OneFour.GetComponent<Text>().text = "Player 1 and 4's Total: " + OneFourTotal;
                TwoThree.GetComponent<Text>().text = "Player 2 and 3's Total: " + TwoThreeTotal;
                if (turnsInARow == 3)
                {
                    CmdEndEvent();
                }
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
        currentEvent = eventList[2];
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
                case "Three":
                    currentBetter = playerList.players[0];
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
        ran = false;

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

        OneFour.SetActive(false);
        TwoThree.SetActive(false);
        BetButton.SetActive(false);
        BetAdd.SetActive(false);
        BetSub.SetActive(false);
    }

    [TargetRpc]
    public void RpcSpawnBet(NetworkConnection conn)
    {
        BetButton.SetActive(true);
        BetAdd.SetActive(true);
        BetSub.SetActive(true);
        OneFour.SetActive(true);
        TwoThree.SetActive(true);
    }
}
