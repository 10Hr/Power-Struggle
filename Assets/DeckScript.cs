using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Mirror;

public class DeckScript : NetworkBehaviour
{
    private string type;
    //public Sprite[] spriteArray;
    public List<GameObject> cards = new List<GameObject>();

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    private void Awake()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        type = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Highest;
    }

    // Start is called before the first frame update
    void Start()
    {

        if (!isLocalPlayer)
        {
            return;
        }

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
