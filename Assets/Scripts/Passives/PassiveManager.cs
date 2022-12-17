using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using System.IO;
using UnityEngine.UI;
public class PassiveManager : NetworkBehaviour
{
    public List<Passive> passives = new List<Passive>();
    public List<Passive> choices = new List<Passive>(); // passives player can choose from after random selection
    public List<Passive> possible = new List<Passive>(); // all passives to be chosen from; 
    private delegate void GetEffects();
    private List<GetEffects> effects = new List<GetEffects>();
    public GameObject txtChoice1;
    public GameObject txtChoice2;
    public GameObject txtChoice3;

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
            passives[passives.Count - 1].PassiveType = data[0];
            passives[passives.Count - 1].PassiveName = data[1];
            passives[passives.Count - 1].PassiveDescription = data[2];
            passives[passives.Count - 1].PassiveEffect = data[3];
            if (passives[passives.Count - 1].PassiveDescription.Contains(".")) 
                passives[passives.Count - 1].PassiveDescription = passives[passives.Count - 1].PassiveDescription.Replace(".", ",");
        }
        input.Close();
    }

    public void selectPassive(string highest) {

        possible.Add(passives[0]);
        possible.Add(passives[1]);
        possible.Add(passives[2]);
        foreach (Passive p in passives) 
            if (p.PassiveType == highest) 
                possible.Add(p);

        for (int i = 0; i < 3; i++) {
            int rand = Random.Range(0, 6 - i);
            choices.Add(possible[rand]); // bad
            possible.RemoveAt(rand);
        }
        Debug.Log("Passives added");


        //select passive
        //add to player
        //add to player manager


       // pullEff();

        txtChoice1.GetComponent<Text>().text = choices[0].PassiveName; //problem chold
        txtChoice2.GetComponent<Text>().text = choices[1].PassiveName;
        txtChoice3.GetComponent<Text>().text = choices[2].PassiveName;

    }

    public Passive getChoiceList(int l) {
        return choices[l];
    }

   void pullEff() { // waste of time
        for (int i = 0; i < effects.Count; i++) 
            for (int j = 0; j < 3; j++) 
                if (choices[j].PassiveEffect == effects[i].Method.Name) 
                    effects[i]();     

    }
    
    void createEffectList()
    {
        // create a list of delegate objects as placeholders for the methods.
        // note the methods must all be of type void with no parameters
        // that is they must all have the same signature.
        Debug.Log("creating effect list");
                effects.Add(mitigateLosses); // how to add methods based on name of effect
                effects.Add(copyCat);
                effects.Add(veteran);

                effects.Add(encore);
                effects.Add(lastingEffects);
                effects.Add(naturalAlly);

                effects.Add(tactician);
                effects.Add(precise);
                effects.Add(seeDeck);

                effects.Add(unrelenting);
                effects.Add(displayOfSkill);
                effects.Add(unstable);

                effects.Add(shadyBusiness);
                effects.Add(blackMarket);
                effects.Add(wireTapping);

    /* needs PassiveEffect to return a methodName
            for (int i = 0; i < effects.Count; i++)
            {
                effects.Add(passives[i].PassiveEffect);
            }


        */
     }


     // effect methods

     //-----------------------------------------default-----------------------------------------


     void mitigateLosses() {
         Debug.Log("mitigateLosses");
     }
     void copyCat() {
         Debug.Log("CopyCat");
     }
     void veteran() {
         Debug.Log("Veteran");
     }


     //-----------------------------------------charisma-----------------------------------------


     void encore() {
         Debug.Log("Encore");
     }
    void lastingEffects() {
        Debug.Log("lastingEffects");
    }
    void naturalAlly() {
        Debug.Log("naturalAlly");
    }

    //-----------------------------------------intelligence-----------------------------------------


    void tactician() {
        Debug.Log("tactician");
    }
    void precise() {
        Debug.Log("precise");
    }
    void seeDeck() {
        Debug.Log("seeDeck");
    }

    //-----------------------------------------strength-----------------------------------------


    void unrelenting() {
        Debug.Log("unrelenting");
    }
    void displayOfSkill() {
        Debug.Log("displayOfSkill");
    }
    void unstable() {
        Debug.Log("unstable");
    }

    //-----------------------------------------cunning-----------------------------------------


    void shadyBusiness() {
        Debug.Log("shadyBusiness");
    }
    void blackMarket() {
        Debug.Log("blackMarket");
    }
    void wireTapping() {
        Debug.Log("wireTapping");
    }
}