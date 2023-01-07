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
    public PlayerScript currentPlayer;

    public string currentMethod;
    public string currentID;
    public int index = 0;

    bool readytrg = false;

    private NetworkIdentity thisID;

    // Start is called before the first frame update
    public void CreateDeck(string highest) {

        //reset deck in case of switching
        cards.Clear();
        cardData.Clear();

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
                cards[cards.Count - 1].Type = data[0];
                cards[cards.Count - 1].Title = data[1];
                cards[cards.Count - 1].Cost = data[2];
                cards[cards.Count - 1].Description = data[3];
                cards[cards.Count - 1].ID = data[4];
            }

        path = null;
        line = null;
        input = null;

        path = Application.dataPath + " /StreamingAssets/DefaultDeck.txt";
        input = new StreamReader(path);
        while ((line = input.ReadLine()) != null)
        {
            string[] data = line.Split(',');
            cards.Add(new CardScript());
            cardData.Add(data);
            cards[cards.Count - 1].Type = data[0];
            cards[cards.Count - 1].Title = data[1];
            cards[cards.Count - 1].Cost = data[2];
            cards[cards.Count - 1].Description = data[3];
            cards[cards.Count - 1].ID = data[4];
        }

        input.Close();

            // when writing a new card
            // type,title,cost,Description
            createEffectList();
    }
   
       public void pullEff(string title, string id) { // waste of time
        currentID = id;
        index = 0;
        foreach (GameObject g in NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots)
        {
            if (g.GetComponent<CardScript>().ID == currentID)
            {
                break;
            }
            index++;
        }
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
        effects.Add(trgLPSX10);
        effects.Add(trgRevI);
        effects.Add(gainPCHX10);
        effects.Add(loseCUGP);
        effects.Add(prevSLoss);
        effects.Add(prevPLoss);
        effects.Add(trgLoseHighest);
        effects.Add(trgLoseP50);
        effects.Add(gainPower50);

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

        //-----------------------------------------Intelligence-----------------------------------------
        effects.Add(exTitle);
    }

    // CARD EFFECTS ARE CALLED IN CARDSCRIPT

    //-----------------------------------------default-----------------------------------------
    public void gainstr1() { //Gain 1 point in strength
        Debug.Log("gain 1 strength point");
        NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
        NetworkClient.localPlayer.GetComponent<PlayerScript>().DiscardCard(NetworkClient.localPlayer.GetComponent<PlayerScript>(), index, NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots);
    }
    public void gainchr1() { //Gain 1 point in charisma
        Debug.Log("gain 1 charisma point");
        NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyStats("charisma", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
        NetworkClient.localPlayer.GetComponent<PlayerScript>().DiscardCard(NetworkClient.localPlayer.GetComponent<PlayerScript>(), index, NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots);
    }
    public void gainint1() { //Gain 1 point in intelligence
        Debug.Log("gain 1 intelligence point");
        NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyStats("intelligence", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
        NetworkClient.localPlayer.GetComponent<PlayerScript>().DiscardCard(NetworkClient.localPlayer.GetComponent<PlayerScript>(), index, NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots);
    }
    public void gaincun1() { //Gain 1 point in cunning
        Debug.Log("gain 1 cunning point");
        NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyStats("cunning", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
        NetworkClient.localPlayer.GetComponent<PlayerScript>().DiscardCard(NetworkClient.localPlayer.GetComponent<PlayerScript>(), index, NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots);
    }
    public void trgLPSX10()
    {
        Debug.Log("Target loses power = to 10x strength");

        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(-10 * NetworkClient.localPlayer.GetComponent<PlayerScript>().Strength, targetPlayer);
                NetworkClient.localPlayer.GetComponent<PlayerScript>().hideButtons();
                NetworkClient.localPlayer.GetComponent<PlayerScript>().DiscardCard(NetworkClient.localPlayer.GetComponent<PlayerScript>(), index, NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots);
                break;
            case false:
                trgbntActive("trglose1");
                break;
        }
    }
    public void trgRevI()
    {
        Debug.Log("Target player, reveal cards in their hand up to your intelligence");
    }
    public void gainPCHX10()
    {
        Debug.Log("charisma,Gain power equal to 10 x Charisma");
    }
    public void loseCUGP()
    {
        Debug.Log("lose a point in cunning - gain 75 power");
    }
    public void prevSLoss()
    {
        Debug.Log("prevent any stat point loss after this card is played");
    }
    public void prevPLoss()
    {
        Debug.Log("prevent any power loss after this card is played");
    }
    public void trgLoseHighest()
    {
        Debug.Log("opponent loses stat point of their highest");
    }
    public void trgLoseP50()
    {
        Debug.Log("opponent loses 50 power");
    }
    public void gainPower50()
    {
        Debug.Log("gain 50 power");
    }
    //-----------------------------------------intelligence-----------------------------------------
    public void exTitle()
    {
        Debug.Log("Not implemented yet");
    }
    //-----------------------------------------charisma-----------------------------------------

    //-----------------------------------------strength-----------------------------------------
    public void gainstr2() { //Gain 2 point in strength
        Debug.Log("gain 2 strength points");
        NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyStats("strength", 2, NetworkClient.localPlayer.GetComponent<PlayerScript>());
        NetworkClient.localPlayer.GetComponent<PlayerScript>().DiscardCard(NetworkClient.localPlayer.GetComponent<PlayerScript>(), index, NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots);
    }
    public void trglose1() { // Target 1 player make them lose 1 point of your choice
        Debug.Log("Target 1 player make them lose 1 point of their highest stat");

        switch(readytrg) {
            case true:
                readytrg = false;
                targetPlayer.ModifyStats(targetPlayer.Highest, -1, targetPlayer);
                NetworkClient.localPlayer.GetComponent<PlayerScript>().hideButtons();      
                NetworkClient.localPlayer.GetComponent<PlayerScript>().DiscardCard(NetworkClient.localPlayer.GetComponent<PlayerScript>(), index, NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots);    
                break;
            case false:
                trgbntActive("trglose1");
            break;
        }
        
    } 
    public void gainstr6() { //Gain 4 point in strength
        Debug.Log("gain 4 strength points");
        NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyStats("strength", 4, NetworkClient.localPlayer.GetComponent<PlayerScript>());
        NetworkClient.localPlayer.GetComponent<PlayerScript>().DiscardCard(NetworkClient.localPlayer.GetComponent<PlayerScript>(), index, NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots);
    }
    public void losePG1str() { // lose power and gain 1 strength point per X power lost
        Debug.Log("lose power and gain 1 strength point per 30 power lost");
        int X = -30; // power lost per strength gained
        int am = 0; // amount of strength points player wants to gain

        am += NetworkClient.localPlayer.GetComponent<PlayerScript>().Charisma;
        am += NetworkClient.localPlayer.GetComponent<PlayerScript>().Intelligence;
        am += NetworkClient.localPlayer.GetComponent<PlayerScript>().Cunning;

        // need to make input for am
        if (NetworkClient.localPlayer.GetComponent<PlayerScript>().Power > am * -X)
        {
            NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyStats("strength", am, NetworkClient.localPlayer.GetComponent<PlayerScript>());
            NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyPower(X * am, NetworkClient.localPlayer.GetComponent<PlayerScript>());
        }
    }
    public void trglosePGP() { //Target 1 player make them lose (GAINER) power, gain gain power = .5 of what player lost
        Debug.Log("Target 1 player make them lose (GAINER) power, gain gain power = .5 of what player lost");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(-3 * NetworkClient.localPlayer.GetComponent<PlayerScript>().Strength, targetPlayer);
                NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyPower(Mathf.RoundToInt(3 * NetworkClient.localPlayer.GetComponent<PlayerScript>().Strength / 2), NetworkClient.localPlayer.GetComponent<PlayerScript>());
                NetworkClient.localPlayer.GetComponent<PlayerScript>().hideButtons();
                NetworkClient.localPlayer.GetComponent<PlayerScript>().DiscardCard(NetworkClient.localPlayer.GetComponent<PlayerScript>(), index, NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots);
                break;
            case false:
                trgbntActive("trglose1");
                break;
        }
    }
    public void trglosePG1str() { //Target 1 player make them lose (GAINER) power, gain 1 strength point
        Debug.Log("Target 1 player make them lose (GAINER) power, gain 1 strength point");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(-5 * NetworkClient.localPlayer.GetComponent<PlayerScript>().Strength, targetPlayer);
                NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
                NetworkClient.localPlayer.GetComponent<PlayerScript>().hideButtons();
                NetworkClient.localPlayer.GetComponent<PlayerScript>().DiscardCard(NetworkClient.localPlayer.GetComponent<PlayerScript>(), index, NetworkClient.localPlayer.GetComponent<PlayerScript>().cardSlots);
                break;
            case false:
                trgbntActive("trglose1");
                break;
        }
    }
    public void loseqstrGP() { // lose a quarter your strength points and gain power = 12 * lost points
        Debug.Log("lose a quarter your strength points and gain power = X * lost points");
        NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyPower(10 * Mathf.RoundToInt(NetworkClient.localPlayer.GetComponent<PlayerScript>().Strength / 2), NetworkClient.localPlayer.GetComponent<PlayerScript>());
        NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyStats("strength", -1 * Mathf.RoundToInt(NetworkClient.localPlayer.GetComponent<PlayerScript>().Strength / 2), NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void loseHGP() { // lose half your strength points and gain power = 20 * lost points
        Debug.Log("lose half your strength points and gain power = X * lost points");
        NetworkClient.localPlayer.GetComponent<PlayerScript>().AddPoints(Mathf.RoundToInt(NetworkClient.localPlayer.GetComponent<PlayerScript>().Strength / 2), NetworkClient.localPlayer.GetComponent<PlayerScript>());
        NetworkClient.localPlayer.GetComponent<PlayerScript>().ModifyStats("strength", NetworkClient.localPlayer.GetComponent<PlayerScript>().Strength, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void trgAloseP() { // Target all players make them lose power = 2*(GAINER) the amount of strength points you have
        Debug.Log("Target all players make them lose power = 2*(GAINER) the amount of strength points you have");
        //ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }
    public void GPpeqstr() { //Gain performance points = strength points 
        Debug.Log("Gain performance points = strength points");
       // ModifyStats("strength", 1, NetworkClient.localPlayer.GetComponent<PlayerScript>());
    }

     //-----------------------------------------cunning-----------------------------------------


    //---------------------------------General Methods-------------------------------------------

    public void trgbntActive(string meth) {
        NetworkClient.localPlayer.GetComponent<PlayerScript>().UnhideButtons();
        currentMethod = meth;
    }

    public void getTarget(PlayerScript tP) {
        targetPlayer = tP;
        readytrg = true;
        pullEff(currentMethod, currentID);

    }


}
