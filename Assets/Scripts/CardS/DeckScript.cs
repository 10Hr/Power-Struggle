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
   
    public void pullEff(string title, string id) {
        currentID = id;
        currentPlayer = NetworkClient.localPlayer.GetComponent<PlayerScript>();
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
        effects.Add(iflostPgainPrethalfelselose100);
        effects.Add(trggainhalfP);
        effects.Add(trgdisallrev);
        effects.Add(trganyrevhndgain50P);
        effects.Add(trglosePeq20xrev);
        effects.Add(trgLS2R);
        effects.Add(gainP10XRAllE);
        effects.Add(losePMult25RAllE);
        effects.Add(selectNP);


        //-----------------------------------------Charisma-----------------------------------------

        effects.Add(chgalystr);
            effects.Add(chgalyint);
            effects.Add(chgalycun);
            effects.Add(gain10xchrPalygainhalf);
            effects.Add(bgainhalfPlostbef);
            effects.Add(allNalylose50Pgain3stat);
            effects.Add(pNalyuntrg);
            effects.Add(trgalygain25Plose3stat);
            effects.Add(trgallcuraly);

        //-----------------------------------------Cunning-----------------------------------------

        effects.Add(gainstreqcun);
        effects.Add(gaininteqcun);
        effects.Add(gainchreqcun);
        effects.Add(trggain4cun);
        effects.Add(trggain4str);
        effects.Add(trggain4chr);
        effects.Add(trggain4int);
        effects.Add(bcuntrg);
        effects.Add(selpaslowstat);
        effects.Add(gainPeq5xallstat);
        effects.Add(relocateAllStats);


    }

    // CARD EFFECTS ARE CALLED IN CARDSCRIPT

    //-----------------------------------------default-----------------------------------------
    #region defaultcards
    public void gainstr1() { //Gain 1 point in strength
        Debug.Log("gain 1 strength point");
        currentPlayer.ModifyStats("strength", 1, currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
    public void gainchr1() { //Gain 1 point in charisma
        Debug.Log("gain 1 charisma point");
        currentPlayer.ModifyStats("charisma", 1, currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
    public void gainint1() { //Gain 1 point in intelligence
        Debug.Log("gain 1 intelligence point");
        currentPlayer.ModifyStats("intelligence", 1, currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
    public void gaincun1() { //Gain 1 point in cunning
        Debug.Log("gain 1 cunning point");
        currentPlayer.ModifyStats("cunning", 1, currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
    public void trgLPSX10()
    {
        Debug.Log("Target loses power = to 10x strength");

        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(-10 * currentPlayer.Strength, targetPlayer);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trgLPSX10");
                break;
        }
    }
    public void trgRevI() { //Target player, reveal cards in their hand up to your intelligence
        Debug.Log("Target player, reveal cards in their hand up to your intelligence");
        switch(readytrg) {
            case true:
                readytrg = false;
                //reveal cards in their hand up to your intelligence
                RevealCards(targetPlayer, currentPlayer.Intelligence);
                currentPlayer.hideButtons();      
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trgRevI");
            break;
        }
    }
    public void gainPCHX10() {//Gain power equal to 10 x Charisma
        Debug.Log("charisma,Gain power equal to 10 x Charisma");
        currentPlayer.ModifyPower(10 * currentPlayer.Charisma, currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
    public void loseCUGP() { // Lose a point in cunning, gain 75 power
        Debug.Log("lose a point in cunning - gain 75 power");
        if (currentPlayer.Cunning > 0)
        {
            currentPlayer.ModifyStats("cunning", -1, currentPlayer);
            currentPlayer.ModifyPower(75, currentPlayer);
        }
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
    public void prevSLoss()
    {
        Debug.Log("prevent any stat point loss after this card is played");
        currentPlayer.CmdDisableSLoss(currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);

    }
    public void prevPLoss()
    {
        currentPlayer.CmdDisablePLoss(currentPlayer);
        Debug.Log("prevent any power loss after this card is played");
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);

    }
    public void trgLoseHighest()
    {
        Debug.Log("opponent loses stat point of their highest");

        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyStats(targetPlayer.Highest, -1, targetPlayer);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trgLoseHighest");
                break;
        }

    }
    public void trgLoseP50()
    {
        Debug.Log("opponent loses 50 power");

        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(-50, targetPlayer);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trgLoseP50");
                break;
        }
    }
    public void gainPower50()
    {
        Debug.Log("gain 50 power");
        currentPlayer.ModifyPower(50, currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
    #endregion
    //-----------------------------------------intelligence-----------------------------------------
    #region intelligencecards
    public void iflostPgainPrethalfelselose100()
    {
        Debug.Log("CHANGE TO NET");

        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
    public void trggainhalfP()
    {
        Debug.Log("CHANGE TO NET");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyStats(targetPlayer.Highest, -1, targetPlayer);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trggainhalfP");
                break;
        }

    }
    public void trgdisallrev()
    {
        Debug.Log("target player, discard all revealed cards");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.DiscardRevealed(currentPlayer, GetEnemySlots(targetPlayer), targetPlayer, GetRevealedIndexes(targetPlayer));
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trgdisallrev");
                break;
        }

    }
    public void trganyrevhndgain50P()
    {
        Debug.Log("Later");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyStats(targetPlayer.Highest, -1, targetPlayer);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trganyrevhndgain50P");
                break;
        }

    }
    public void trglosePeq20xrev()
    {
        Debug.Log("Player loses power = to 20*revealed cards");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(-20 * GetRevealedIndexes(targetPlayer).Count, targetPlayer);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trglosePeq20xrev");
                break;
        }

    }
    public void trgLS2R()
    {
        Debug.Log("PLayer loses stats for every 2 revealed cards in their hand");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyStats(targetPlayer.Highest, Mathf.RoundToInt(GetRevealedIndexes(targetPlayer).Count/2), targetPlayer);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trgLS2R");
                break;
        }

    }
    public void gainP10XRAllE()
    {
        Debug.Log("Gain power equal to all revealed cards * 10");
        currentPlayer.ModifyPower(10 * (GetRevealedIndexes(currentPlayer.enemy1).Count + GetRevealedIndexes(currentPlayer.enemy2).Count + GetRevealedIndexes(currentPlayer.enemy3).Count), currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);

    }
    public void losePMult25RAllE()
    {
        Debug.Log("Later");
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);

    }
    public void selectNP()
    {
        Debug.Log("Select a new passive");
        currentPlayer.passiveOption1.SetActive(true);
        currentPlayer.passiveOption2.SetActive(true);
        currentPlayer.passiveOption3.SetActive(true);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);

    }
    #endregion
    //-----------------------------------------charisma-----------------------------------------
    #region charismacards
    public void chgalystr() { //Change your ally to all strength players
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void chgalyint() { //Change your ally to all intelligence players
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void chgalycun() { //Change your ally to all cunning players
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void gain10xchrPalygainhalf() { //You gain 10 x charisma power and your allies gain half of that
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void bgainhalfPlostbef() { //You and your allies gain half the power you lost before playing this card
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void allNalylose50Pgain3stat() { //All players that aren't your ally lose 50 power and gain 3 stat points of your current ally stat
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void pNalyuntrg() { //Players who aren't your ally can not target you for the rest of this turn
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void trgalygain25Plose3stat() { //Target an ally they gain 25 power and lose 3 points of their highest stat
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void trgallcuraly() { //gain 3 stats points of your current ally stat
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }

    #endregion
    //-----------------------------------------strength-----------------------------------------
    #region strengthcards
    public void gainstr2() { //Gain 2 point in strength
        Debug.Log("gain 2 strength points");
        currentPlayer.ModifyStats("strength", 2, currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
    public void trglose1() { // Target 1 player make them lose 1 point of your choice
        Debug.Log("Target 1 player make them lose 1 point of their highest stat");

        switch(readytrg) {
            case true:
                readytrg = false;
                targetPlayer.ModifyStats(targetPlayer.Highest, -1, targetPlayer);
                currentPlayer.hideButtons();      
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trglose1");
            break;
        }
        
    } 
    public void gainstr6() { //Gain 4 point in strength
        Debug.Log("gain 4 strength points");
        currentPlayer.ModifyStats("strength", 4, currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
    public void losePG1str() { // lose power and gain 1 strength point per X power lost
        Debug.Log("lose power and gain 1 strength point per 30 power lost");
        int X = -30; // power lost per strength gained
        int am = 0; // amount of strength points player wants to gain

        am += currentPlayer.Charisma;
        am += currentPlayer.Intelligence;
        am += currentPlayer.Cunning;

        // need to make input for am
        if (currentPlayer.Power > am * -X)
        {
            currentPlayer.ModifyStats("strength", am, currentPlayer);
            currentPlayer.ModifyPower(X * am, currentPlayer);
        }
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);

    }
    public void trglosePGP() { //Target 1 player make them lose (GAINER) power, gain gain power = .5 of what player lost
        Debug.Log("Target 1 player make them lose (GAINER) power, gain gain power = .5 of what player lost");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(-3 * currentPlayer.Strength, targetPlayer);
                currentPlayer.ModifyPower(Mathf.RoundToInt(3 * currentPlayer.Strength / 2), currentPlayer);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trglosePGP");
                break;
        }
    }
    public void trglosePG1str() { //Target 1 player make them lose (GAINER) power, gain 1 strength point
        Debug.Log("Target 1 player make them lose (GAINER) power, gain 1 strength point");
        switch (readytrg)
        {
            case true:
                readytrg = false;
                targetPlayer.ModifyPower(-5 * currentPlayer.Strength, targetPlayer);
                currentPlayer.ModifyStats("strength", 1, currentPlayer);
                currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
            case false:
                trgbntActive("trglosePG1str");
                break;
        }
    }
    public void loseqstrGP() { // lose a quarter your strength points and gain power = 12 * lost points
        Debug.Log("lose a quarter your strength points and gain power = X * lost points");
        currentPlayer.ModifyPower(10 * Mathf.RoundToInt(currentPlayer.Strength / 2), currentPlayer);
        currentPlayer.ModifyStats("strength", -1 * Mathf.RoundToInt(currentPlayer.Strength / 2), currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
    public void loseHGP() { // lose half your strength points and gain power = 20 * lost points
        Debug.Log("lose half your strength points and gain power = X * lost points");
        currentPlayer.AddPoints(Mathf.RoundToInt(currentPlayer.Strength / 2), currentPlayer);
        currentPlayer.ModifyStats("strength", currentPlayer.Strength, currentPlayer);
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);

    }
    public void trgAloseP() { // Target all players make them lose power = 2*(GAINER) the amount of strength points you have
        Debug.Log("Target all players make them lose power = 2*(GAINER) the amount of strength points you have");
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);

        //ModifyStats("strength", 1, currentPlayer);
    }
    public void GPpeqstr() { //Gain performance points = strength points 
        Debug.Log("Gain performance points = strength points");
        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);

        // ModifyStats("strength", 1, currentPlayer);
    }
    #endregion
    //-----------------------------------------cunning-----------------------------------------
    #region cunningcards

    public void gainstreqcun() { //Gain strength equal to cunning
            Debug.Log("Gain strength equal to cunning");
            currentPlayer.ModifyStats("strength", currentPlayer.Cunning, currentPlayer);
            currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void gaininteqcun() { //Gain intelligence equal to cunning
            Debug.Log("Gain strength equal to cunning");
            currentPlayer.ModifyStats("intelligence", currentPlayer.Cunning, currentPlayer);
            currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
            currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void gainchreqcun() { //Gain charisma equal to cunning
            Debug.Log("Gain strength equal to cunning");
            currentPlayer.ModifyStats("charisma", currentPlayer.Cunning, currentPlayer);
            currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
            currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void trggain4cun() { // Target an opponent - they gain 4 cunning
            Debug.Log("Target an opponent - they gain 4 cunning");
            switch (readytrg)
            {
                case true:
                    readytrg = false;
                    targetPlayer.ModifyStats("cunning", 4, targetPlayer);
                    currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
                case false:
                    trgbntActive("trggain4cun");
                    break;
            }
        
        }
        public void trggain4str() { //Target an opponent - they gain 4 strength
            Debug.Log("Target an opponent - they gain 4 strength");
            switch (readytrg) {
                case true:
                    readytrg = false;
                    targetPlayer.ModifyStats("strength", 4, targetPlayer);
                    currentPlayer.hideButtons();
                    currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
                case false:
                    trgbntActive("trggain4str");
                    break;
            }
        }
        public void trggain4chr() { //Target an opponent - they gain 4 charisma
            Debug.Log("Target an opponent - they gain 4 charisma");
            switch (readytrg) {
                case true:
                    readytrg = false;
                    targetPlayer.ModifyStats("charisma", 4, targetPlayer);
                    currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
                case false:
                    trgbntActive("trggain4chr");
                    break;
            }
        }
        public void trggain4int() { //Target an opponent - they gain 4 intelligence
            Debug.Log("Target an opponent - they gain 4 intelligence");
            switch (readytrg) {
                case true:
                    readytrg = false;
                    targetPlayer.ModifyStats("intelligence", 4, targetPlayer);
                    currentPlayer.hideButtons();
                currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
                currentPlayer.CmdturnIncrease(currentPlayer);
                break;
                case false:
                    trgbntActive("trggain4int");
                    break;
            }
        }
        public void bcuntrg() { // Become untargetable for the rest of this turn
            Debug.Log("Become untargetable for the rest of this turn");
            currentPlayer.setUntargetable(currentPlayer);
            currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
        // need to handle untargetable when turn ends when turn ends
    }
        public void selpaslowstat() { //Select a passive from your lowest stat
            Debug.Log("Select a passive from your lowest stat");

        currentPlayer.passiveOption1.SetActive(true);
        currentPlayer.passiveOption2.SetActive(true);
        currentPlayer.passiveOption3.SetActive(true);

        currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void gainPeq5xallstat() { //Gain power equal to 5x all your stat points
            Debug.Log("Gain power equal to 5x all your stat points");
            currentPlayer.ModifyPower((currentPlayer.Strength + 
                                                                                         currentPlayer.Cunning + 
                                                                                         currentPlayer.Charisma + 
                                                                                         currentPlayer.Intelligence) * 5, 
                                                                                         currentPlayer);                                                                
            currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);
    }
        public void relocateAllStats() { //Relocate all your stat points
            Debug.Log("Relocate all your stat points");
        currentPlayer.ResetStats(currentPlayer);
            currentPlayer.DiscardCard(currentPlayer, index, currentPlayer.cardSlots);
        currentPlayer.CmdturnIncrease(currentPlayer);

    }
    #endregion
    //---------------------------------General Methods-------------------------------------------

    public void trgbntActive(string meth) {
        currentPlayer.UnhideButtons();
        currentMethod = meth;
    }

    public void getTarget(PlayerScript tP) {
        targetPlayer = tP;
        readytrg = true;
        pullEff(currentMethod, currentID);

    }

    public void RevealCards(PlayerScript p, int amount)
    {
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
}
