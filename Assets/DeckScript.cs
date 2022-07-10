using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Mirror;

public class DeckScript : NetworkBehaviour
{
    private string type;
    public List<GameObject> cards = new List<GameObject>();
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
    private GameObject thisPlayer;
    private NetworkIdentity thisID;
    PlayerManager playerManager;

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    private void Awake()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        //type = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Highest;
    }

    public void getOwner(NetworkIdentity netID)
    {
        thisID = netID;

        // server kicks people trying to spawn cards
        //comment this first if out to test player 2 spawning cards
        if (playerManager.getPlayer(0).GetComponent<NetworkIdentity>() == thisID)
        {
            thisPlayer = playerManager.getPlayer(0);
        }
        else if(playerManager.getPlayer(1).GetComponent<NetworkIdentity>() == thisID)
        {
            thisPlayer = playerManager.getPlayer(1);
        }
        else if(playerManager.getPlayer(2).GetComponent<NetworkIdentity>() == thisID)
        {
            thisPlayer = playerManager.getPlayer(2);
        }
        else if(playerManager.getPlayer(3).GetComponent<NetworkIdentity>() == thisID)
        {
            thisPlayer = playerManager.getPlayer(3);
        }
        type = thisPlayer.GetComponent<PlayerScript>().Highest;
        CreateDeck();
    }

    // Start is called before the first frame update
    void CreateDeck()
    {

        string path = null;
        string line = null;
        StreamReader input = null;

        switch (type)
        {
            case "cunning":

                path = Application.dataPath + " /StreamingAssets/cardsCunning.txt";
                input = new StreamReader(path);
                line = null;

                while ((line = input.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    cards.Add(new GameObject("card"));
                    cards[cards.Count - 1].name = data[0];
                    cards[cards.Count - 1].AddComponent<SpriteRenderer>();
                    cards[cards.Count - 1].AddComponent<CardScript>();
                    cards[cards.Count - 1].GetComponent<CardScript>().Title = data[0];
                    cards[cards.Count - 1].GetComponent<CardScript>().Effect = data[1];
                    cards[cards.Count - 1].GetComponent<CardScript>().Stat = data[2];
                }
                input.Close();
                    break;

            case "charisma":

                path = Application.dataPath + " /StreamingAssets/cardsCharisma.txt";
                input = new StreamReader(path);
                line = null;

                while ((line = input.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    cards.Add(new GameObject("card"));
                    cards[cards.Count - 1].name = data[0];
                    cards[cards.Count - 1].AddComponent<SpriteRenderer>();
                    cards[cards.Count - 1].AddComponent<CardScript>();
                    cards[cards.Count - 1].GetComponent<CardScript>().Title = data[0];
                    cards[cards.Count - 1].GetComponent<CardScript>().Effect = data[1];
                    cards[cards.Count - 1].GetComponent<CardScript>().Stat = data[2];
                }
                input.Close();
                break;

            case "intelligence":

                path = Application.dataPath + " /StreamingAssets/cardsIntelligence.txt";
                input = new StreamReader(path);
                line = null;

                while ((line = input.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    cards.Add(new GameObject("card"));
                    cards[cards.Count - 1].name = data[0];
                    cards[cards.Count - 1].AddComponent<SpriteRenderer>();
                    cards[cards.Count - 1].AddComponent<CardScript>();
                    cards[cards.Count - 1].GetComponent<CardScript>().Title = data[0];
                    cards[cards.Count - 1].GetComponent<CardScript>().Effect = data[1];
                    cards[cards.Count - 1].GetComponent<CardScript>().Stat = data[2];
                }
                input.Close();
                break;

            case "strength":

                path = Application.dataPath + " /StreamingAssets/cardsStrength.txt";
                input = new StreamReader(path);
                line = null;

                while ((line = input.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    cards.Add(new GameObject("card"));
                    cards[cards.Count - 1].name = data[0];
                    cards[cards.Count - 1].AddComponent<SpriteRenderer>();
                    cards[cards.Count - 1].AddComponent<CardScript>();
                    cards[cards.Count - 1].GetComponent<CardScript>().Title = data[0];
                    cards[cards.Count - 1].GetComponent<CardScript>().Effect = data[1];
                    cards[cards.Count - 1].GetComponent<CardScript>().Stat = data[2];
                }
                input.Close();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
