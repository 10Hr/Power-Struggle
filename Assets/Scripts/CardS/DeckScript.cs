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

    private delegate void GetEffects();
    private List<GetEffects> effects = new List<GetEffects>();



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
    public void CreateDeck(string highest) {

        Debug.Log("creating deck");
        Debug.Log(highest);
        string path = null;
        string line = null;
        StreamReader input = null;

        switch (highest)
        {
            case "cunning":
                path = Application.dataPath + " /StreamingAssets/cardsCunning.txt";
                break;

            case "charisma":
                path = Application.dataPath + " /StreamingAssets/cardsCharisma.txt";
                break;

            case "intelligence":
                path = Application.dataPath + " /StreamingAssets/cardsIntelligence.txt";
                break;

            case "strength":
                path = Application.dataPath + " /StreamingAssets/cardsStrength.txt";
                break;
        }

            input = new StreamReader(path);
            line = null;

            while ((line = input.ReadLine()) != null)
            {
                string[] data = line.Split(',');
                cards.Add(new CardScript());
                cardData.Add(data);
                Debug.Log(cardData[cardData.Count - 1][0]);
                cards[cards.Count - 1].Type = data[0];
                cards[cards.Count - 1].Title = data[1];
                cards[cards.Count - 1].Cost = data[2];
                cards[cards.Count - 1].Description = data[3];

            }
            input.Close();

            // when writing a new card
            // type,title,cost,Description
            createEffectList();
    }
       public void pullEff(string title) { // waste of time
        for (int i = 0; i < effects.Count; i++) 
                if (title == effects[i].Method.Name) 
                    effects[i]();     

    }

    void createEffectList()
    {
        // create a list of delegate objects as placeholders for the methods.
        // note the methods must all be of type void with no parameters
        // that is they must all have the same signature.
        Debug.Log("creating card effect list");
        effects.Add(gainstr1);
        effects.Add(gainchr1);
        effects.Add(gainint1);
        effects.Add(gaincun1);
        effects.Add(gainstr2);
        effects.Add(trglose1);
        effects.Add(trglose1);
        effects.Add(gainstr6);
        effects.Add(losePG1str);
       
}

    //-----------------------------------------default-----------------------------------------
    public void gainstr1() {
        Debug.Log("gain 1 strength point");
    }
    public void gainchr1() {
        Debug.Log("gain 1 charisma point");
    }
    public void gainint1() {
        Debug.Log("gain 1 intelligence point");
    }
    public void gaincun1() {
        Debug.Log("gain 1 cunning point");
    }
    //-----------------------------------------intelligence-----------------------------------------

    //-----------------------------------------charisma-----------------------------------------

    //-----------------------------------------strength-----------------------------------------
    public void gainstr2() {
        Debug.Log("gain 2 strength points");
    }
    public void trglose1() {
        Debug.Log("target player loses 1 point");
    }
    public void gainstr6() {
        Debug.Log("gain 6 strength points");
    }
    public void losePG1str() {
        Debug.Log("lose 1 point from each player");
    }

     //-----------------------------------------cunning-----------------------------------------





}
