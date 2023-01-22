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
    // Start is called before the first frame update
    void Start()
    {
     //   FSM = GameObject.Find("FSM").GetComponent<GameState>();
     //   playerList = GameObject.Find("PlayerList").GetComponent<PlayerList>();
     //   first = GameObject.Find("first").GetComponent<Text>();
     //   second = GameObject.Find("second").GetComponent<Text>();
     //   third = GameObject.Find("third").GetComponent<Text>();
     //   fourth = GameObject.Find("fourth").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerList.players.Count != 4)
            return;

        if (!called)
        {
            firstPlace = playerList.players[0];
            secondPlace = playerList.players[1];
            thirdPlace = playerList.players[2];
            fourthPlace = playerList.players[3];
            called = true;
        }

        if (firstPlace != null && secondPlace != null && thirdPlace != null && fourthPlace != null)
        {
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

        if (!(FSM.turn == 0 && FSM.currentState == GameStates.Event))
        {
            return;
        }

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
