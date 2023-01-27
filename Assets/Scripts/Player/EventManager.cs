using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField]
    public PlayerList playerList;
    GameObject ReadyButton;
    GameObject[] gos;
    GameObject[] e1gs;
    GameObject[] e2gs;
    GameObject[] e3gs;
    [SerializeField]
    GameState FSM;
    public GameObject BetButton;
    public GameObject BetSub;
    public GameObject BetAdd;
    private bool ran = false;

    public GameObject OneFour;
    public GameObject TwoThree;

    private void Start()
    {
//        playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
        ReadyButton = GameObject.Find("Ready");
        gos = GameObject.FindGameObjectsWithTag("eventLabels");
        e1gs = GameObject.FindGameObjectsWithTag("e1g");
        e2gs = GameObject.FindGameObjectsWithTag("e2g");
        e3gs = GameObject.FindGameObjectsWithTag("e3g");
//      //  FSM = GameObject.Find("FSM").GetComponent<GameState>();

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
                if (isServer)
                {
                    RpcSpawnBet(currentBetter.connectionToClient);
                    foreach (PlayerScript p in playerList.players)
                    {
                        if (p.netId != currentBetter.netId)
                            RpcDespawnBet(p.connectionToClient);
                    }
                }
                if (currentIndex == 4)
                    currentIndex = 0;
                currentBetter = playerList.players[currentIndex];
                OneFour.GetComponent<Text>().text = "Player 1 and 4's Total: " + OneFourTotal;
                TwoThree.GetComponent<Text>().text = "Player 2 and 3's Total: " + TwoThreeTotal;
                if (turnsInARow == 3)
                {
                    if(OneFourTotal > TwoThreeTotal)
                    {
                        RpcSpawnPassives(playerList.players[0].connectionToClient, playerList.players[0]);
                        RpcSpawnPassives(playerList.players[3].connectionToClient, playerList.players[3]);

                        playerList.players[0].passive2 = playerList.players[0].passive.passiveName;
                        playerList.players[3].passive2 = playerList.players[3].passive.passiveName;
                        playerList.players[0].passiveManager.CmdSelectPassive(playerList.players[0].highest, playerList.players[0]);
                        playerList.players[3].passiveManager.CmdSelectPassive(playerList.players[3].highest, playerList.players[3]);
                    }
                    else if (OneFourTotal < TwoThreeTotal)
                    {
                        RpcSpawnPassives(playerList.players[1].connectionToClient, playerList.players[1]);
                        RpcSpawnPassives(playerList.players[2].connectionToClient, playerList.players[2]);
                        playerList.players[1].passive2 = playerList.players[1].passive.passiveName;
                        playerList.players[2].passive2 = playerList.players[2].passive.passiveName;
                        playerList.players[1].passiveManager.CmdSelectPassive(playerList.players[1].highest, playerList.players[1]);
                        playerList.players[2].passiveManager.CmdSelectPassive(playerList.players[2].highest, playerList.players[2]);
                    }
                    else
                    {
                        RpcSpawnPassives(playerList.players[0].connectionToClient, playerList.players[0]);
                        RpcSpawnPassives(playerList.players[3].connectionToClient, playerList.players[3]);
                        RpcSpawnPassives(playerList.players[1].connectionToClient, playerList.players[1]);
                        RpcSpawnPassives(playerList.players[2].connectionToClient, playerList.players[2]);
                        playerList.players[0].passive2 = playerList.players[0].passive.passiveName;
                        playerList.players[3].passive2 = playerList.players[3].passive.passiveName;
                        playerList.players[1].passive2 = playerList.players[1].passive.passiveName;
                        playerList.players[2].passive2 = playerList.players[2].passive.passiveName;
                        playerList.players[0].passiveManager.CmdSelectPassive(playerList.players[0].highest, playerList.players[0]);
                        playerList.players[3].passiveManager.CmdSelectPassive(playerList.players[3].highest, playerList.players[3]);
                        playerList.players[1].passiveManager.CmdSelectPassive(playerList.players[1].highest, playerList.players[1]);
                        playerList.players[2].passiveManager.CmdSelectPassive(playerList.players[2].highest, playerList.players[2]);
                    }
                    CmdEndEvent();
                }
                break;

            default:
                break;
        }
    }
    [TargetRpc]
    public void RpcSpawnPassives(NetworkConnection conn, PlayerScript p)
    {
        p.passiveOption1.SetActive(true);
        p.passiveOption2.SetActive(true);
        p.passiveOption3.SetActive(true);
    }

    [TargetRpc]
    public void RpcDespawnBet(NetworkConnection conn)
    {
        BetButton.SetActive(false);
        BetAdd.SetActive(false);
        BetSub.SetActive(false);
    }

    [Command (requiresAuthority = false)]
    public void GetEvents()
    {
        eventList.Add("Three");
        eventList.Add("Two");
        eventList.Add("One");
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
                    GameObject.Find("Instructions").GetComponent<TextMeshProUGUI>().text = "Relocate all your stat points, then guess what each players new highest stat is." +
                        " For every one you guess correct you gain 50 power, and the enemy loses 50 power.";
                    RpcSpawnReady();
                    foreach (PlayerScript p in playerList.players)
                    {
                        p.ready = false;
                        p.ResetStats();
                    }
                    break;
                case "Two":
                    GameObject.Find("Instructions").GetComponent<TextMeshProUGUI>().text = ""; // Make GameRule box
                    FSM.EventTwo = true;
                    CmdEndEvent();
                    break;
                case "Three":
                    GameObject.Find("Instructions").GetComponent<TextMeshProUGUI>().text = "Rework this one";
                    RpcSpawnLabels();
                    currentBetter = playerList.players[0];
                    break;
                default:
                    break;
            }
        }
    }

    [ClientRpc]
    public void RpcSpawnLabels()
    {
        OneFour.SetActive(true);
        TwoThree.SetActive(true);
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
    }
}
