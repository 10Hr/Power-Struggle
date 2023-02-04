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
    public GameObject e1Button;
    public GameObject e1Sub;
    public GameObject e1Add;
    public GameObject e2Button;
    public GameObject e2Sub;
    public GameObject e2Add;
    public GameObject e3Button;
    public GameObject e3Sub;
    public GameObject e3Add;

    private bool ran = false;

    public GameObject Praise;
    public GameObject Censure;

    private void Start()
    {
//        playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
        ReadyButton = GameObject.Find("Ready");
        gos = GameObject.FindGameObjectsWithTag("eventLabels");
        e1gs = GameObject.FindGameObjectsWithTag("e1g");
        e2gs = GameObject.FindGameObjectsWithTag("e2g");
        e3gs = GameObject.FindGameObjectsWithTag("e3g");
//      //  FSM = GameObject.Find("FSM").GetComponent<GameState>();

        Praise = GameObject.Find("Praise");
        Censure = GameObject.Find("Censure");

        e1Button = GameObject.Find("Enemy1Votes");
        e1Add = GameObject.Find("e1a");
        e1Sub = GameObject.Find("e1s");
        e2Button = GameObject.Find("Enemy2Votes");
        e2Add = GameObject.Find("e2a");
        e2Sub = GameObject.Find("e2s");
        e3Button = GameObject.Find("Enemy3Votes");
        e3Add = GameObject.Find("e3a");
        e3Sub = GameObject.Find("e3s");

        e1Button.SetActive(false);
        e2Button.SetActive(false);
        e3Button.SetActive(false);
        e1Add.SetActive(false);
        e1Sub.SetActive(false);
        e2Add.SetActive(false);
        e2Sub.SetActive(false);
        e3Add.SetActive(false);
        e3Sub.SetActive(false);

        foreach (GameObject g in e1gs)
            g.SetActive(false);
        foreach (GameObject g in e2gs)
            g.SetActive(false);
        foreach (GameObject g in e3gs)
            g.SetActive(false);
        foreach (GameObject g in gos)
            g.SetActive(false);

        Praise.SetActive(false);
        Censure.SetActive(false);
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
                    CmdCheckGuess();
                    CmdEndEvent();
                }
                break;
            case "Two":

                break;
            case "Three":
                if (isServer)
                {
                    foreach (PlayerScript p in playerList.players)
                    {
                        RpcUpdateLabels(p.connectionToClient, p);
                    }
                }
                if (playerList.players[0].Praise == 0 && playerList.players[0].Censure == 0
                    && playerList.players[1].Praise == 0 && playerList.players[1].Censure == 0
                    && playerList.players[2].Praise == 0 && playerList.players[2].Censure == 0
                    && playerList.players[3].Praise == 0 && playerList.players[3].Censure == 0)
                {
                    int currentHigh = playerList.players[0].eVotes;
                    PlayerScript currentWinner = playerList.players[0];
                    foreach (PlayerScript p in playerList.players)
                    {
                        if (p.eVotes > currentHigh)
                        {
                            currentHigh = p.eVotes;
                            currentWinner = p;
                        }
                    }
                    currentWinner.ModifyPower(100);
                    currentWinner.AddPoints(3);
                    CmdEndEvent();
                }
                break;

            default:
                break;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdCheckGuess()
    {
        playerList.players[0].CheckGuess(playerList.players[0].connectionToClient);
        playerList.players[1].CheckGuess(playerList.players[1].connectionToClient);
        playerList.players[2].CheckGuess(playerList.players[2].connectionToClient);
        playerList.players[3].CheckGuess(playerList.players[3].connectionToClient);
    }

    [TargetRpc]
    public void RpcUpdateLabels(NetworkConnection conn, PlayerScript p)
    {
        e1Button.GetComponentInChildren<TextMeshProUGUI>().text = "Enemy1: " + p.e1Total;
        e2Button.GetComponentInChildren<TextMeshProUGUI>().text = "Enemy2: " + p.e2Total;
        e3Button.GetComponentInChildren<TextMeshProUGUI>().text = "Enemy3: " + p.e3Total;
        Praise.GetComponent<Text>().text = "Praise Votes: " + p.Praise;
        Censure.GetComponent<Text>().text = "Censure Votes: " + p.Censure;
    }

    [TargetRpc]
    public void RpcSpawnPassives(NetworkConnection conn, PlayerScript p)
    {
        p.passiveOption1.SetActive(true);
        p.passiveOption2.SetActive(true);
        p.passiveOption3.SetActive(true);
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
                    GameObject.Find("Instructions").GetComponent<TextMeshProUGUI>().text = "Funny Vote";
                    RpcSpawnBet();
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
        Praise.SetActive(true);
        Censure.SetActive(true);
    }

    [Command(requiresAuthority = false)]
    public void CmdEndEvent()
    {
        eventList.Remove(eventList[0]);
        FSM.EndEvent = true;
        currentEvent = "";
        ran = false;

        RpcDespawnAll();
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

        Praise.SetActive(false);
        Censure.SetActive(false);
        e1Button.SetActive(false);
        e2Button.SetActive(false);
        e3Button.SetActive(false);
        e1Add.SetActive(false);
        e1Sub.SetActive(false);
        e2Add.SetActive(false);
        e2Sub.SetActive(false);
        e3Add.SetActive(false);
        e3Sub.SetActive(false);
    }

    [ClientRpc]
    public void RpcSpawnBet()
    {
        e1Button.SetActive(true);
        e2Button.SetActive(true);
        e3Button.SetActive(true);
        e1Add.SetActive(true);
        e1Sub.SetActive(true);
        e2Add.SetActive(true);
        e2Sub.SetActive(true);
        e3Add.SetActive(true);
        e3Sub.SetActive(true);
    }
}
