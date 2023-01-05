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

    public PlayerScript targetPlayer;

    public string currentMethod;

    bool readytrg = false;

    //private GameObject player1;
    //private GameObject player2;
    //private GameObject player3;
    //private GameObject player4;
    //private GameObject thisPlayer;
    private NetworkIdentity thisID;
    //public PlayerManager playerManager;

    private void Awake()
    {




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

    //-----------------------------------------default-----------------------------------------
        effects.Add(gainstr1);
        effects.Add(gainchr1);
        effects.Add(gainint1);
        effects.Add(gaincun1);

    //-----------------------------------------strength-----------------------------------------
        effects.Add(gainstr2);
        effects.Add(trglose1);
        effects.Add(gainstr6);
        effects.Add(losePG1str);
        effects.Add(trglosePGP);
        effects.Add(trglosePG1str);
        effects.Add(loseqstrGP);
        effects.Add(loseHGP);
        effects.Add(trgAloseP);
        effects.Add(GPpeqstr);
       
    }

    // CARD EFFECTS ARE CALLED IN CARDSCRIPT

    //-----------------------------------------default-----------------------------------------
    public void gainstr1() { //Gain 1 point in strength
        Debug.Log("gain 1 strength point");
        ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void gainchr1() { //Gain 1 point in charisma
        Debug.Log("gain 1 charisma point");
        ModifyStats("charisma", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void gainint1() { //Gain 1 point in intelligence
        Debug.Log("gain 1 intelligence point");
        ModifyStats("intelligence", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void gaincun1() { //Gain 1 point in cunning
        Debug.Log("gain 1 cunning point");
        ModifyStats("cunning", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    //-----------------------------------------intelligence-----------------------------------------

    //-----------------------------------------charisma-----------------------------------------

    //-----------------------------------------strength-----------------------------------------
    public void gainstr2() { //Gain 2 point in strength
        Debug.Log("gain 2 strength points");
        ModifyStats("strength", 2, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void trglose1() { // Target 1 player make them lose 1 point of your choice
        Debug.Log("Target 1 player make them lose 1 point of your choice");

        switch(readytrg) {
            case true:
                readytrg = false;
                // effect happens here
                Debug.Log("targeting player" + targetPlayer);
                break;
            case false:
                trgbntActive("trglose1");
            break;
        }
    } 
    public void gainstr6() { //Gain 6 point in strength
        Debug.Log("gain 6 strength points");
        ModifyStats("strength", 6, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void losePG1str() { // lose power and gain 1 strength point per X power lost
        Debug.Log("lose power and gain 1 strength point per X power lost");
        ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void trglosePGP() { //Target 1 player make them lose (GAINER) power, gain gain power = .5 of what player lost
        Debug.Log("Target 1 player make them lose (GAINER) power, gain gain power = .5 of what player lost");
        ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void trglosePG1str() { //Target 1 player make them lose (GAINER) power, gain 1 strength point
        Debug.Log("Target 1 player make them lose (GAINER) power, gain 1 strength point");
        ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void loseqstrGP() { // lose a quarter your strength points and gain power = 12 * lost points
        Debug.Log("lose a quarter your strength points and gain power = X * lost points");
        ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void loseHGP() { // lose half your strength points and gain power = 20 * lost points
        Debug.Log("lose half your strength points and gain power = X * lost points");
        ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void trgAloseP() { // Target all players make them lose power = 2*(GAINER) the amount of strength points you have
        Debug.Log("Target all players make them lose power = 2*(GAINER) the amount of strength points you have");
        ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void GPpeqstr() { //Gain performance points = strength points 
        Debug.Log("Gain performance points = strength points");
        ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }

     //-----------------------------------------cunning-----------------------------------------


    //---------------------------------General Methods-------------------------------------------

    //[Command(requiresAuthority = false)]
    public void ModifyStats(string type, int amount, PlayerScript p) {

        switch (type) {
            case "strength":
                p.Strength = amount;
                break;
            case "charisma":
                p.Charisma = amount;
                break;
            case "intelligence":
                p.Intelligence = amount;
                break;
            case "cunning":
                p.Cunning = amount;
                break;
        }
        p.MaxPoints = amount;
    }

    public void trgbntActive(string meth) {
        NetworkClient.localPlayer.GetComponent<PlayerScript>().UnhideButtons();
        currentMethod = meth;
    }

    public void getTarget(PlayerScript tP) {
        targetPlayer = tP;
        readytrg = true;
        pullEff(currentMethod);

    }



}
