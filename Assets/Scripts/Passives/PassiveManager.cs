using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using System.IO;
using TMPro;
using UnityEngine.UI;
public class PassiveManager : NetworkBehaviour
{
    public List<Passive> passives = new List<Passive>();
    public List<Passive> choices = new List<Passive>(); // passives player can choose from after random selection
    public List<Passive> possible = new List<Passive>(); // all passives to be chosen from; 
    private delegate void GetEffects();
    private List<GetEffects> effects = new List<GetEffects>();
    public GameObject bntChoice1;
    public GameObject bntChoice2;
    public GameObject bntChoice3;

    void Awake() { 
    
        getPassivesFromFile();
        createEffectList();

    }
    //Type, Name, Desc
    // add thing to turn periods to commas in description
    public void getPassivesFromFile() {
        string path = null;
        string line = null;
        StreamReader input = null;

        path = Application.dataPath + " /StreamingAssets/Passives.txt";
        input = new StreamReader(path);
        line = null;

        while ((line = input.ReadLine()) != null) {
            string[] data = line.Split(',');
            Passive p = new Passive();
            passives.Add(p);
            passives[passives.Count - 1].passiveType = data[0];
            passives[passives.Count - 1].passiveName = data[1];
        }
        input.Close();
    }

    [Command (requiresAuthority = false)]
    public void CmdSelectPassive(string highest, PlayerScript player) {
        possible.Clear();
        choices.Clear();
        foreach (Passive p in passives) 
            if (p.passiveType == highest) 
                possible.Add(p);

        for (int i = 0; i < 3; i++) {
            choices.Add(possible[i]);
        }

        RpcSetLabels(player.connectionToClient, choices[0].passiveName, choices[1].passiveName, choices[2].passiveName);

        possible.Clear();
        choices.Clear();
    }

    //try with seperate passives isntead of list
    [TargetRpc]
    public void RpcSetLabels(NetworkConnection conn, string p1, string p2, string p3)
    {
        bntChoice1.GetComponent<TextMeshProUGUI>().text = p1;
        bntChoice2.GetComponent<TextMeshProUGUI>().text = p2;
        bntChoice3.GetComponent<TextMeshProUGUI>().text = p3;
    }

   void pullEff() { // waste of time
        for (int i = 0; i < effects.Count; i++) 
            for (int j = 0; j < 3; j++) 
                if (choices[j].passiveName == effects[i].Method.Name) 
                    effects[i]();     

    }
    
    void createEffectList()
    {
        // create a list of delegate objects as placeholders for the methods.
        // note the methods must all be of type void with no parameters
        // that is they must all have the same signature.
        Debug.Log("creating effect list");
                effects.Add(StrongAllies);
                effects.Add(SmartAllies);
                effects.Add(ShadyAllies);

                effects.Add(Tactician);
                effects.Add(SeeDeck);

                effects.Add(BlackMarket);
                effects.Add(WireTapping);
     }


     // effect methods
     //-----------------------------------------charisma-----------------------------------------


     void StrongAllies() {
         Debug.Log("Encore");
     }
    void SmartAllies() {
        Debug.Log("lastingEffects");
    }
    void ShadyAllies() {
        Debug.Log("naturalAlly");
    }

    //-----------------------------------------intelligence-----------------------------------------


    void Tactician() {
        Debug.Log("tactician");
    }
    void SeeDeck() {
        Debug.Log("seeDeck");
        //add disabled bool to card script
        //add disable check when selecting card
    }

    //-----------------------------------------strength-----------------------------------------
    //-----------------------------------------cunning-----------------------------------------
    void BlackMarket() {
        Debug.Log("blackMarket");
        //complicated... do last...
    }
    void WireTapping() {
        Debug.Log("wireTapping");
        // complicated... do when events added. DISABLED FOR ALPHA
    }
}