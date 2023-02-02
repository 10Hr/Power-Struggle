using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Mirror;

public class DeckScript : NetworkBehaviour
{
    //public List<string[]> cards = new List<string[]>();
    public List<string[]> cardData = new List<string[]>();
    public List<int> cardDataIDs = new List<int>();

    private delegate void GetEffects();
    private List<GetEffects> effects = new List<GetEffects>();

    public PlayerScript targetPlayer;
    public PlayerScript currentPlayer;
    public MessageLogManager logger;

    public List<PlayerScript> enemies;

    public string currentMethod;
    public string currentID;
    public int index = 0;

    bool readytrg = false;

    private NetworkIdentity thisID;

    public Dictionary<int, string> nameDict = new Dictionary<int, string>();
    public Dictionary<int, string> typeDict = new Dictionary<int, string>();

    // Start is called before the first frame update
    private void Start()
    {
        
        logger = GameObject.Find("LogManager").GetComponent<MessageLogManager>(); 
        LoadDicts();   
    }

    public void CreateDeck(string highest) {

        //reset deck in case of switching

        cardDataIDs.Clear();

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
                cardDataIDs.Add(int.Parse(data[4]));
            }

        path = null;
        line = null;
        input = null;

        path = Application.dataPath + " /StreamingAssets/DefaultDeck.txt";
        input = new StreamReader(path);
        while ((line = input.ReadLine()) != null)
        {
            string[] data = line.Split(',');
            cardDataIDs.Add(int.Parse(data[4]));
        }

        input.Close();

            // when writing a new card
            // type,title,cost,Description
        createEffectList();
        //LoadDicts();
    }

    public void LoadDicts() {


        for (int i = 1; i < 6; i++) {
            string path = null;
            string line = null;
            StreamReader input = null;
            switch (i)
            {
                case 1:
                    path = Application.dataPath + " /StreamingAssets/cardsCunning.txt";
                    break;

                case 2:
                    path = Application.dataPath + " /StreamingAssets/cardsCharisma.txt";
                    break;

                case 3:
                    path = Application.dataPath + " /StreamingAssets/cardsIntelligence.txt";
                    break;

                case 4:
                    path = Application.dataPath + " /StreamingAssets/cardsStrength.txt";
                    break;
                case 5:
                    path = Application.dataPath + " /StreamingAssets/DefaultDeck.txt";
                    break;
            }

            input = new StreamReader(path);
            line = null;
            while ((line = input.ReadLine()) != null)
            {
                string[] data = line.Split(',');
                nameDict.Add(int.Parse(data[4]), data[1]);
                typeDict.Add(int.Parse(data[4]), data[0]);
            }
            input.Close();  //close the file
        }
        

    }

    public string sendCardName(int id) {
        return nameDict[id];
    }
    public string sendCardType(int id) {
        return typeDict[id];
    } 
   
    public void pullEff(string title, string id) {
        currentID = id;
        currentPlayer = NetworkClient.localPlayer.GetComponent<PlayerScript>();
        enemies = currentPlayer.sendPlayerData();
        index = 0;
            foreach (GameObject g in currentPlayer.cardSlots)
            {
                if (g.GetComponent<CardScript>().ID == currentID || g.GetComponent<CardScript>().ID == "1000")
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
        effects.Clear();

    //-----------------------------------------default-----------------------------------------
        effects.Add(def1);
        effects.Add(def2);
        effects.Add(def3);
        effects.Add(def4);
        effects.Add(def5);
        effects.Add(def6);
        effects.Add(def7);
        effects.Add(def8);
        effects.Add(def9);
        effects.Add(def10);
        effects.Add(def11);
        effects.Add(def12);
        effects.Add(def13);

        //-----------------------------------------strength-----------------------------------------
        effects.Add(str1);
        effects.Add(str2);
        effects.Add(str3);
        effects.Add(str4);
        effects.Add(str5);
        effects.Add(str6);
        effects.Add(str7);
        effects.Add(str8);
        effects.Add(str9);
        effects.Add(str10);
        effects.Add(str11);
        effects.Add(str12);

        //-----------------------------------------Intelligence-----------------------------------------
        effects.Add(int1);
        effects.Add(int2);
        effects.Add(int3);
        effects.Add(int4);
        effects.Add(int5);
        effects.Add(int6);
        effects.Add(int7);
        effects.Add(int8);
        effects.Add(int9);
        effects.Add(int10);


        //-----------------------------------------Charisma-----------------------------------------

        effects.Add(chr1);
        effects.Add(chr2);
        effects.Add(chr3);
        effects.Add(chr4);
        effects.Add(chr5);
        effects.Add(chr6);
        effects.Add(chr7);
        effects.Add(chr8);
        effects.Add(chr9);
        effects.Add(chr10);

        //-----------------------------------------Cunning-----------------------------------------

        effects.Add(cun1);
        effects.Add(cun2);
        effects.Add(cun3);
        effects.Add(cun4);
        effects.Add(cun5);
        effects.Add(cun6);
        effects.Add(cun7);
        effects.Add(cun8);
        effects.Add(cun9);
        effects.Add(cun10);
        effects.Add(cun11);
        effects.Add(cun12);
    }

    // CARD EFFECTS ARE CALLED IN CARDSCRIPT

    //-----------------------------------------default----------------------------------------- DONE
    #region defaultcards
    public void def1() { //Gain 2 point in strength
        Debug.Log("gain 2 strength point");
        currentPlayer.ModifyStats("strength", 2);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
    public void def2() { //Gain 2 point in charisma
        Debug.Log("gain 2 charisma point");
        currentPlayer.ModifyStats("charisma", 2);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
    public void def3() { //Gain 2 point in intelligence
        Debug.Log("gain 2 intelligence point");
        currentPlayer.ModifyStats("intelligence", 2);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
    public void def4() { //Gain 2 point in cunning
        Debug.Log("gain 2 cunning point");
        currentPlayer.ModifyStats("cunning", 2);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
    public void def5() //Target loses power = to 5x highest + 1/2 strength
    {
        Debug.Log("Target loses power = to 5x highest + 1/2 strength");

        switch (readytrg)
        {
            case true:
                readytrg = false;
                int highest;
                switch (currentPlayer.Highest)
                {
                    case "strength":
                        highest = currentPlayer.Strength;
                        break;
                    case "charisma":
                        highest = currentPlayer.Charisma;
                        break;
                    case "intelligence":
                        highest = currentPlayer.Intelligence;
                        break;
                    case "cunning":
                        highest = currentPlayer.Cunning;
                        break;
                    default:
                        highest = 0;
                        break;
                }
                targetPlayer.ModifyPower(-5 * (highest + Mathf.RoundToInt(currentPlayer.Strength / 2)));
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
        
                break;
            case false:
                trgbntActive("def5");
                break;
        }
    }
    public void def6() { //Lose 1 Intelligence, reveal all card in enemy hand
        Debug.Log("Lose 1 Intelligence, reveal all card in enemy hand");
        switch(readytrg) {
            case true:
                readytrg = false;
                RevealCards(targetPlayer, 6);
                currentPlayer.ModifyStats("intelligence", -1);
                currentPlayer.hideButtons();      
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
        
                break;
            case false:
                trgbntActive("def6");
            break;
        }
    }
    public void def7() {//Gain power equal to highest + 1/2 charisma * 5
        Debug.Log("Gain power equal to highest + 1/2 charisma * 5");
        int highest;
        switch (currentPlayer.Highest)
        {
            case "strength":
                highest = currentPlayer.Strength;
                break;
            case "charisma":
                highest = currentPlayer.Charisma;
                break;
            case "intelligence":
                highest = currentPlayer.Intelligence;
                break;
            case "cunning":
                highest = currentPlayer.Cunning;
                break;
            default:
                highest = 0;
                break;
        }
        currentPlayer.ModifyPower(5 * (highest + Mathf.RoundToInt(currentPlayer.Charisma / 2)));
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
    public void def8() { // Lose a point in cunning, gain 75 power
        Debug.Log("lose a point in cunning - gain 75 power");
        if (currentPlayer.Cunning > 0)
        {
            currentPlayer.ModifyStats("cunning", -1);
            currentPlayer.ModifyPower(75);
        }
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
    public void def9() //prevent any stat point loss after this card is played
    {
        Debug.Log("prevent any stat point loss after this card is played");
        currentPlayer.CmdDisableSLoss(true);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
    public void def10() //prevent any power loss after this card is played
    {
        currentPlayer.CmdDisablePLoss(true);
        Debug.Log("prevent any power loss after this card is played");
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
    public void def11()//Opponent loses 1 stat point of highest
    {
        Debug.Log("opponent loses stat point of their highest");

        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyStats(targetPlayer.Highest, -1);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
        
                break;
            case false:
                trgbntActive("def11");
                break;
        }
    }
    public void def12()
    {
        Debug.Log("opponent loses 50 power");

        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(-50);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
        
                break;
            case false:
                trgbntActive("def12");
                break;
        }
    }
    public void def13()
    {
        Debug.Log("gain 50 power");
        currentPlayer.ModifyPower(50);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
    #endregion
    //-----------------------------------------intelligence----------------------------------------- DONE
    #region intelligencecards
    public void int1()//Reveal 2 card in an opponents hand and gain 2 stat points
    {
        Debug.Log("Reveal 2 card in an opponents hand and gain 2 stat points");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                RevealCards(targetPlayer, 2);
                currentPlayer.AddPoints(2);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
                break;
            case false:
                trgbntActive("int1");
                break;
        }
    }
    public void int2()//Target a player they lose power equal to 15 x the amount of cards in their hand that are revealed
    {
        Debug.Log("Target a player they lose power equal to 15 x the amount of cards in their hand that are revealed");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(15 * Mathf.RoundToInt(GetRevealedIndexes(targetPlayer).Count / 2));
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
                break;
            case false:
                trgbntActive("int2");
                break;
        }
    }
    public void int3() //target player, gain half power they gained so far this turn
    {
        Debug.Log("target player, gain half power they gained so far this turn");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                currentPlayer.ModifyPower(Mathf.RoundToInt(targetPlayer.powerGained / 2));
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();

                break;
            case false:
                trgbntActive("int3");
                break;
        }
    }
    public void int4() // target player, discard all revealed cards, reveal 3 more
    {
        Debug.Log("target player, discard all revealed cards");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.DiscardRevealed(currentPlayer, GetEnemySlots(targetPlayer), targetPlayer, GetRevealedIndexes(targetPlayer));
                RevealCards(targetPlayer, 3);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();

                break;
            case false:
                trgbntActive("int4");
                break;
        }
    }
    public void int5() // gain stat points = to all revealed cards
    {
        Debug.Log("gain stat points = to all revealed cards");
        currentPlayer.AddPoints(GetRevealedIndexes(currentPlayer.enemy1).Count + GetRevealedIndexes(currentPlayer.enemy2).Count + GetRevealedIndexes(currentPlayer.enemy3).Count);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
    public void int6() //Look at cards in an opponents hand up to how many intelligence points you have (max 6)
    {
        Debug.Log("Look at cards in an opponents hand up to how many intelligence points you have (max 6)");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                RevealCards(targetPlayer, currentPlayer.Intelligence);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
                break;
            case false:
                trgbntActive("int6");
                break;
        }
    }
    public void int7() // Player loses stats for every 2 revealed cards in their hand
    {
        Debug.Log("Player loses stats for every 2 revealed cards in their hand");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyStats(targetPlayer.Highest, Mathf.RoundToInt(GetRevealedIndexes(targetPlayer).Count / 2));
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
                break;
            case false:
                trgbntActive("int7");
                break;
        }
    }
    public void int8() // Gain power equal to all revealed cards * 10
    {
        Debug.Log("Gain power equal to all revealed cards * 10");
        currentPlayer.ModifyPower(10 * (GetRevealedIndexes(currentPlayer.enemy1).Count + GetRevealedIndexes(currentPlayer.enemy2).Count + GetRevealedIndexes(currentPlayer.enemy3).Count));
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
    public void int9() //if you lose power so far this turn, gain it back, if not, lose 100
    {
        Debug.Log("if you lose power so far this turn, gain it back, if not, lose 100");

        if (currentPlayer.powerLost > 0)
            currentPlayer.ModifyPower(currentPlayer.powerLost);
        else
            currentPlayer.ModifyPower(-100);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }

    public void int10() // Discard all cards then draw 6
    {
        Debug.Log("Discard all cards, draw 6 more");
        currentPlayer.DiscardCard(0, currentPlayer.cardSlots);
        currentPlayer.DiscardCard(1, currentPlayer.cardSlots);
        currentPlayer.DiscardCard(2, currentPlayer.cardSlots);
        currentPlayer.DiscardCard(3, currentPlayer.cardSlots);
        currentPlayer.DiscardCard(4, currentPlayer.cardSlots);
        currentPlayer.DiscardCard(5, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
    #endregion
    //-----------------------------------------charisma-----------------------------------------
    #region charismacards
    public void chr1() { //Change your ally to all strength players for the rest of the turn
        currentPlayer.CmdSetAllyStat("strength");
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
        public void chr2()
    { //Change your ally to all intelligence players for the rest of the turn
        currentPlayer.CmdSetAllyStat("intelligence");
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
        public void chr3()
    { //Change your ally to all cunning players for the rest of the turn
        currentPlayer.CmdSetAllyStat("cunning");
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
        public void chr4() { //You gain 15 x charisma power and your allies gain half of that 
        currentPlayer.ModifyPower(currentPlayer.Charisma * 15);
        foreach (PlayerScript p in enemies)
        {
            if (p.Highest == currentPlayer.allyStat)
                p.ModifyPower(Mathf.RoundToInt((currentPlayer.Charisma * 15) / 2));
        }
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
        public void chr5() { //You and your allies gain half the power you lost before playing this card
        foreach (PlayerScript p in enemies)
        {
            if (p.Highest == currentPlayer.allyStat)
                p.ModifyPower(Mathf.RoundToInt(p.powerLost/2));
        }
        currentPlayer.ModifyPower(Mathf.RoundToInt(currentPlayer.powerLost / 2));
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
        public void chr6() { //All players that aren't your ally lose 50 power and gain 3 stat points of your current ally stat
        foreach (PlayerScript p in enemies)
        {
            if (p.Highest != currentPlayer.allyStat)
            {
                p.ModifyPower(-50);
                p.ModifyStats(currentPlayer.allyStat, 3);
            }
        }
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
        public void chr7() { //Players who aren't your ally can not target you for the rest of this turn
        currentPlayer.setUntargetable(true);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
        public void chr8() { //Target all allies they gain 25 power and lose 3 points of their highest stat
        foreach (PlayerScript p in enemies)
        {
            if (p.Highest != currentPlayer.allyStat)
            {
                p.ModifyPower(25);
                p.ModifyStats(currentPlayer.allyStat, -3);
            }
        }
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
        public void chr9() { //lose ally for turn
        Debug.Log("Lose ally for turn");
        currentPlayer.CmdSetAllyStat("");

    }
    public void chr10()
    { //All enemies gain 3 stats points of your current ally stat
        Debug.Log("All enemies gain 3 stats points of your current ally stat");
        foreach (PlayerScript p in enemies)
        {
            p.ModifyStats(currentPlayer.allyStat, 3);
        }
        currentPlayer.ModifyStats(currentPlayer.allyStat, 3);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
    #endregion
    //-----------------------------------------strength-----------------------------------------
    #region strengthcards
    public void str1() { //Gain 2 points
        Debug.Log("gain 2 points");
        currentPlayer.AddPoints(2);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
    public void str2() { // Target 1 player make them lose 1 point of your choice
        Debug.Log("Target 1 player make them lose 1 point of their highest stat");

        switch(readytrg) {
            case true:
                readytrg = false;
                targetPlayer.ModifyStats(targetPlayer.Highest, -1);
                currentPlayer.hideButtons();      
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
        
                break;
            case false:
                trgbntActive("str2");
            break;
        }
    } 
    public void str3() { //Gain 4 point in strength
        Debug.Log("gain 4 strength points");
        currentPlayer.ModifyStats("strength", 4);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
    public void str4() { // lose all stat points except strength, gain power = to them x 20
        Debug.Log("lose all stat points except strength, gain power = to them x 20");
        int X = 20; // power lost per strength gained
        int am = 0; // amount of strength points player wants to gain

        // need to make input for am
        am += currentPlayer.Charisma;
        am += currentPlayer.Intelligence;
        am += currentPlayer.Cunning;
        currentPlayer.ModifyStats("charisma", -currentPlayer.Charisma);
        currentPlayer.ModifyStats("intelligence", -currentPlayer.Intelligence);
        currentPlayer.ModifyStats("cunning", -currentPlayer.Cunning);
        currentPlayer.ModifyPower(X * am);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        

    }
    public void str5() { //Target 1 player make them lose (GAINER) power, gain gain power = .5 of what player lost
        Debug.Log("Target 1 player make them lose (GAINER) power, gain gain power = .5 of what player lost");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(-3 * currentPlayer.Strength);
                currentPlayer.ModifyPower(Mathf.RoundToInt(3 * currentPlayer.Strength / 2));
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
        
                break;
            case false:
                trgbntActive("str5");
                break;
        }
    }
    public void str6() { //Target 1 player make them lose (GAINER) power, gain 1 strength point
        Debug.Log("Target 1 player make them lose (GAINER) power, gain 1 strength point");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(-5 * currentPlayer.Strength);
                currentPlayer.ModifyStats("strength", 1);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
        
                break;
            case false:
                trgbntActive("str6");
                break;
        }
    }
    public void str7() { // lose half of your strength points and gain power = 10 x lost points
        Debug.Log("lose half of your strength points and gain power = 10 x lost points");
        currentPlayer.ModifyPower(10 * Mathf.RoundToInt(currentPlayer.Strength / 2));
        currentPlayer.ModifyStats("strength", -1 * Mathf.RoundToInt(currentPlayer.Strength / 2));
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
    public void str8() { // lose all of your strength andgain available points = to that
        Debug.Log("lose all of your strength andgain available points = to that");
        int str = currentPlayer.Strength;
        currentPlayer.ModifyStats("strength", -currentPlayer.Strength);
        currentPlayer.AddPoints(Mathf.RoundToInt(str / 2));
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
    public void str9() { // Target all players make them lose power = 2*(GAINER) the amount of strength points you have
        Debug.Log("Target all players make them lose power = 2*(GAINER) the amount of strength points you have");
        foreach (PlayerScript p in enemies)
        {
            p.ModifyPower(-5*currentPlayer.Strength);
        }
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
    public void str10()//Target all players including yourself - they lose 2 stat points of their highest
    {
        Debug.Log("Target all players including yourself - they lose 2 stat points of their highest");
        foreach (PlayerScript p in enemies)
        {
            p.ModifyStats(p.Highest, -2);
        }
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
    public void str11()//Lose all of your power - gain strength = to that value / 25
    {
        Debug.Log("Lose all of your power - gain strength = to that value / 25");
        currentPlayer.ModifyStats("strength", Mathf.RoundToInt(currentPlayer.Power / 25));
    }
    public void str12()
    { //Select new passive
        Debug.Log("Select new passive");
        currentPlayer.passiveManager.CmdSelectPassive(currentPlayer.Highest, currentPlayer);
        currentPlayer.passiveOption1.SetActive(true);
        currentPlayer.passiveOption2.SetActive(true);
        currentPlayer.passiveOption3.SetActive(true);
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
    #endregion
    //-----------------------------------------cunning-----------------------------------------
    #region cunningcards

    public void cun1() { //Gain strength equal to cunning
            Debug.Log("Gain strength equal to cunning");
            currentPlayer.ModifyStats("strength", currentPlayer.Cunning);
            currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
        public void cun2() { //Gain intelligence equal to cunning
            Debug.Log("Gain strength equal to cunning");
            currentPlayer.ModifyStats("intelligence", currentPlayer.Cunning);
            currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
            currentPlayer.CmdturnIncrease();
        
    }
        public void cun3() { //Gain charisma equal to cunning
            Debug.Log("Gain strength equal to cunning");
            currentPlayer.ModifyStats("charisma", currentPlayer.Cunning);
            currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
            currentPlayer.CmdturnIncrease();
        
    }
        public void cun4() { // Target an opponent - they gain 4 cunning
            Debug.Log("Target an opponent - they gain 4 cunning");
            switch (readytrg)
            {
                case true:
                    readytrg = false;
                    targetPlayer.ModifyStats("cunning", 4);
                    currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
        
                break;
                case false:
                    trgbntActive("cun4");
                    break;
            }
        
        }
        public void cun5() { //Target an opponent - they gain 4 strength
            Debug.Log("Target an opponent - they gain 4 strength");
            switch (readytrg) {
                case true:
                    readytrg = false;
                    targetPlayer.ModifyStats("strength", 4);
                    currentPlayer.hideButtons();
                    currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
        
                break;
                case false:
                    trgbntActive("cun5");
                    break;
            }
        }
        public void cun6() { //Target an opponent - they gain 4 charisma
            Debug.Log("Target an opponent - they gain 4 charisma");
            switch (readytrg) {
                case true:
                    readytrg = false;
                    targetPlayer.ModifyStats("charisma", 4);
                    currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
        
                break;
                case false:
                    trgbntActive("cun6");
                    break;
            }
        }
        public void cun7() { //Target an opponent - they gain 4 intelligence
            Debug.Log("Target an opponent - they gain 4 intelligence");
            switch (readytrg) {
                case true:
                    readytrg = false;
                    targetPlayer.ModifyStats("intelligence", 4);
                    currentPlayer.hideButtons();
                currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease();
        
                break;
                case false:
                    trgbntActive("cun7");
                    break;
            }
        }
        public void cun8() { // Become untargetable for the rest of this turn
            Debug.Log("Become untargetable for the rest of this turn");
            currentPlayer.setUntargetable(true);
            currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
        // need to handle untargetable when turn ends when turn ends
    }
        public void cun9() { //Select a passive from your lowest stat
            Debug.Log("Select a passive from your lowest stat");

        currentPlayer.passiveManager.CmdSelectPassive(currentPlayer.lowest, currentPlayer);
        currentPlayer.passiveOption1.SetActive(true);
        currentPlayer.passiveOption2.SetActive(true);
        currentPlayer.passiveOption3.SetActive(true);

        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
        public void cun10() { //Gain power equal to 5x all your stat points
            Debug.Log("Gain power equal to 5x all your stat points");
            currentPlayer.ModifyPower((currentPlayer.Strength + 
                                       currentPlayer.Cunning + 
                                       currentPlayer.Charisma + 
                                       currentPlayer.Intelligence) * 5);                                                                
            currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
        
    }
        public void cun11()
    { //All opponents lose power equal to 5x all their stat points
        Debug.Log("All opponents lose power equal to 5x all their stat points");
        foreach (PlayerScript p in enemies)
        {
            p.ModifyPower((p.Strength + p.Cunning + p.Charisma + p.Intelligence) * 5);
        }
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
      
    }
    public void cun12()
    { //Relocate all your stat points
        Debug.Log("Relocate all your stat points");
        currentPlayer.ResetStats();
        currentPlayer.DiscardCard(index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease();
    }
    #endregion
    //---------------------------------General Methods-------------------------------------------

    public void trgbntActive(string meth) {
        NetworkClient.localPlayer.GetComponent<PlayerScript>().UnhideButtons();
        currentMethod = meth;
    }

    public void getTarget(PlayerScript tP) {
        targetPlayer = tP;
        readytrg = true;
        if (currentMethod == "SeeDeck")
            SeeDeck();
        else
            pullEff(currentMethod, currentID);

    }

    public void RevealCards(PlayerScript p, int amount)
    {
        logger.AppendMessage(string.Format("{0} revealed {1} cards in {2}'s hand", currentPlayer.playerName, amount, p.playerName));
        foreach (GameObject g in GetEnemySlots(p))
        {
            if (!g.GetComponent<CardScript>().revealed && amount > 0)
            {
                g.GetComponent<CardScript>().revealed = true;
                amount--;
            }
        }
    }
    public GameObject[] GetEnemySlots(PlayerScript p)
    {
        if (p.netId == currentPlayer.enemy1.netId)
        {
            return currentPlayer.enemySlots1;
        }
        else if (p.netId == currentPlayer.enemy2.netId)
        {
            return currentPlayer.enemySlots2;
        }
        else
        {
            return currentPlayer.enemySlots3;
        }
    }
    public List<int> GetRevealedIndexes(PlayerScript target)
    {
        List<int> indexes = new List<int>();
        int index = 0;
        foreach (GameObject g in GetEnemySlots(target))
        {
            if (g.GetComponent<CardScript>().revealed)
            {
                indexes.Add(index);
            }
            index++;
        }
        return indexes;
    }

    //Passive Methods
    public void SeeDeck()
    {
        switch (readytrg)
        {
            case true:
                readytrg = false;
                currentPlayer = NetworkClient.localPlayer.GetComponent<PlayerScript>();
                RevealCards(targetPlayer, 6);
                NetworkClient.localPlayer.GetComponent<PlayerScript>().CmdSelectedTrg(true);
                NetworkClient.localPlayer.GetComponent<PlayerScript>().hideButtons();
                break;
            case false:
                trgbntActive("SeeDeck");
                break;
        }
    }
}
