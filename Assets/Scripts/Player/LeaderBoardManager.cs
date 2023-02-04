using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LeaderBoardManager : NetworkBehaviour
{
    [SerializeField]
    public PlayerList playerList;
    [SerializeField]
    public Text first;
    [SerializeField]
    public Text second;
    [SerializeField]
    public Text third;
    [SerializeField]
    public Text fourth;
    PlayerScript firstPlace;
    PlayerScript secondPlace;
    PlayerScript thirdPlace;
    PlayerScript fourthPlace;
    string firstString;
    string secondString;
    string thirdString;
    string fourthString;
    public bool called = false;
    [SerializeField]
    GameState FSM;

    public List<PlayerScript> TurnOrder = new List<PlayerScript>();

    public void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame

    private void Update()
    {
        if (playerList.players.Count != 4)
            return;

        if (!called)
        {
            firstPlace = playerList.players[0];
            secondPlace = playerList.players[1];
            thirdPlace = playerList.players[2];
            fourthPlace = playerList.players[3];
            TurnOrder.Add(firstPlace);
            TurnOrder.Add(secondPlace);
            TurnOrder.Add(thirdPlace);
            TurnOrder.Add(fourthPlace);
            called = true;
        }

        PlayerScript current = NetworkClient.localPlayer.GetComponent<PlayerScript>();
        first.color = current.netId == firstPlace.netId ? Color.green : Color.red;
        second.color = current.netId == secondPlace.netId ? Color.green : Color.red;
        third.color = current.netId == thirdPlace.netId ? Color.green : Color.red;
        fourth.color = current.netId == fourthPlace.netId ? Color.green : Color.red;

        if (firstPlace != null && secondPlace != null && thirdPlace != null && fourthPlace != null)
        {
            TurnOrder[0] = firstPlace;
            TurnOrder[1] = secondPlace;
            TurnOrder[2] = thirdPlace;
            TurnOrder[3] = fourthPlace;

            for (int i = 0; i < 4; i++)
            {
                if (firstPlace.netId == playerList.players[i].netId)
                {
                    firstString = "Player " + (i + 1) + ": " + playerList.players[i].Power;
                }
                else if (secondPlace.netId == playerList.players[i].netId)
                {
                    secondString = "Player " + (i + 1) + ": " + playerList.players[i].Power;
                }
                else if (thirdPlace.netId == playerList.players[i].netId)
                {
                    thirdString = "Player " + (i + 1) + ": " + playerList.players[i].Power;
                }
                else if (fourthPlace.netId == playerList.players[i].netId)
                {
                    fourthString = "Player " + (i + 1) + ": " + playerList.players[i].Power;
                }

            }

            first.text = firstString;
            second.text = secondString;
            third.text = thirdString;
            fourth.text = fourthString;
        }
    }

    public void CmdUpdateLeaderBoard()
    {
        //if (!(FSM.turn == 0 && FSM.currentState == GameStates.Event))
        //{
        //    return;
        //}

        if (secondPlace.Power > firstPlace.Power)
        {
            PlayerScript temp = secondPlace;
            secondPlace = firstPlace;
            firstPlace = temp;
        }
        if (thirdPlace.Power > secondPlace.Power)
        {
            PlayerScript temp = thirdPlace;
            thirdPlace = secondPlace;
            secondPlace = temp;
        }
        if (fourthPlace.Power > thirdPlace.Power)
        {
            PlayerScript temp = fourthPlace;
            fourthPlace = thirdPlace;
            thirdPlace = temp;
        }
    }
}
