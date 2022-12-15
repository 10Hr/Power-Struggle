using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Mirror;

public class DeckScript : NetworkBehaviour
{
    public List<CardScript> cards = new List<CardScript>();
    public List<string[]> cardData = new List<string[]>();
    //private GameObject player1;
    //private GameObject player2;
    //private GameObject player3;
    //private GameObject player4;
    //private GameObject thisPlayer;
    private NetworkIdentity thisID;
    //public PlayerManager playerManager;

    private void Awake()
    {
        Debug.Log("Deck is awake");
        //playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        //type = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Highest;
    }

    // Start is called before the first frame update
    public void CreateDeck(string highest/*, GameObject prefab*/) {

        Debug.Log("creating deck");
        Debug.Log(highest);
        string path = null;
        string line = null;
        StreamReader input = null;

        switch (highest)
        {
            case "cunning":
                Debug.Log("In cunning");
                path = Application.dataPath + " /StreamingAssets/cardsCunning.txt";
                input = new StreamReader(path);
                line = null;

                while ((line = input.ReadLine()) != null)
                {
                    Debug.Log("about to create a card");
                    string[] data = line.Split(',');
                    cards.Add(new CardScript());
                    cardData.Add(data);
                    Debug.Log(cardData[cardData.Count - 1][0]);
                    //cards.Add(Instantiate(prefab));
                    //cards[cards.Count - 1].name = data[0];
                    //cards[cards.Count - 1].AddComponent<SpriteRenderer>();
                    //cards[cards.Count - 1].AddComponent<CardScript>();
                    //cards[cards.Count - 1].AddComponent<NetworkIdentity>();
                    //cards[cards.Count - 1].GetComponent<CardScript>().Title = data[0];
                    //cards[cards.Count - 1].GetComponent<CardScript>().Effect = data[1];
                    //cards[cards.Count - 1].GetComponent<CardScript>().Stat = data[2];
                    cards[cards.Count - 1].Title = data[0];
                    cards[cards.Count - 1].Effect = data[1];
                    cards[cards.Count - 1].Stat = data[2];
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
                    cards.Add(new CardScript());
                    cardData.Add(data);
                    //cards.Add(Instantiate(prefab));
                    //cards[cards.Count - 1].name = data[0];
                    //cards[cards.Count - 1].AddComponent<SpriteRenderer>();
                    //cards[cards.Count - 1].AddComponent<CardScript>();
                    //cards[cards.Count - 1].AddComponent<NetworkIdentity>();
                    //cards[cards.Count - 1].GetComponent<CardScript>().Title = data[0];
                    //cards[cards.Count - 1].GetComponent<CardScript>().Effect = data[1];
                    //cards[cards.Count - 1].GetComponent<CardScript>().Stat = data[2];
                    cards[cards.Count - 1].Title = data[0];
                    cards[cards.Count - 1].Effect = data[1];
                    cards[cards.Count - 1].Stat = data[2];
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
                    cards.Add(new CardScript());
                    cardData.Add(data);
                    //cards.Add(Instantiate(prefab));
                    //cards[cards.Count - 1].name = data[0];
                    //cards[cards.Count - 1].AddComponent<SpriteRenderer>();
                    //cards[cards.Count - 1].AddComponent<CardScript>();
                    //cards[cards.Count - 1].AddComponent<NetworkIdentity>();
                    //cards[cards.Count - 1].GetComponent<CardScript>().Title = data[0];
                    //cards[cards.Count - 1].GetComponent<CardScript>().Effect = data[1];
                    //cards[cards.Count - 1].GetComponent<CardScript>().Stat = data[2];
                    cards[cards.Count - 1].Title = data[0];
                    cards[cards.Count - 1].Effect = data[1];
                    cards[cards.Count - 1].Stat = data[2];
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
                    cards.Add(new CardScript());
                    cardData.Add(data);
                    //cards.Add(Instantiate(prefab));
                    //cards[cards.Count - 1].name = data[0];
                    //cards[cards.Count - 1].AddComponent<SpriteRenderer>();
                    //cards[cards.Count - 1].AddComponent<CardScript>();
                    //cards[cards.Count - 1].AddComponent<NetworkIdentity>();
                    //cards[cards.Count - 1].GetComponent<CardScript>().Title = data[0];
                    //cards[cards.Count - 1].GetComponent<CardScript>().Effect = data[1];
                    //cards[cards.Count - 1].GetComponent<CardScript>().Stat = data[2];
                    cards[cards.Count - 1].Title = data[0];
                    cards[cards.Count - 1].Effect = data[1];
                    cards[cards.Count - 1].Stat = data[2];
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
