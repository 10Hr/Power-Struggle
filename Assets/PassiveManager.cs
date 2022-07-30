using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;
using UnityEngine.UI;
public class PassiveManager : NetworkBehaviour
{
    private List<Passive> passives = new List<Passive>();
    private List<Passive> choices = new List<Passive>();


    //private List<> effects = new List<>();
   // private GameObject passiveChoice1;
    //private GameObject passiveChoice2;
    private GameObject passiveChoice3;


    void Awake() { getPassivesFromFile(); 
    
    //passiveChoice1 = GameObject.Find("passiveChoice1");
    //passiveChoice2 = GameObject.Find("passiveChoice2");
    passiveChoice3 = GameObject.Find("passiveChoice3");

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
            passives.Add(new Passive());
            passives[passives.Count - 1].PassiveType = data[0];
            passives[passives.Count - 1].PassiveName = data[1];
            passives[passives.Count - 1].PassiveDescription = data[2];

            if (passives[passives.Count - 1].PassiveDescription.Contains(".")) 
                passives[passives.Count - 1].PassiveDescription = passives[passives.Count - 1].PassiveDescription.Replace(".", ",");
        }
        input.Close();
    }

    public void selectPassive(string highest) {
        //Debug.Log(highest);

        int rnd = Random.Range(0, 6);
        choices.Add(passives[0]);
        choices.Add(passives[1]);
        choices.Add(passives[2]);
        foreach (Passive p in passives) 
            if (p.PassiveType == highest)
                choices.Add(p);

        for (int i = 0; i < passives.Count; i++) {
          // choices.Add(Random.Range(0, 6));
        }

        effect(choices[rnd].PassiveName);

     


       // passiveChoice1.GetComponent<Text>().text = "Name: " + choices[0].PassiveName + "\n" + "Description: " + choices[0].PassiveDescription;
        //passiveChoice2.GetComponent<Text>().text = "Name: " + choices[1].PassiveName + "\n" + "Description: " + choices[1].PassiveDescription;
        passiveChoice3.GetComponent<Text>().text = "Name: " + choices[0].PassiveName + "\n" + "Description: " + choices[0].PassiveDescription +
                                                   "\n\nName: " + choices[1].PassiveName + "\n" + "Description: " + choices[1].PassiveDescription +
                                                   "\n\nName: " + choices[2].PassiveName + "\n" + "Description: " + choices[2].PassiveDescription;

        //select passive
        //add to player
        //add to player manager



    }

    void effect(string eff) {
        //do stuff 

      //  foreach(String p in effects)
          //  eff
      //  }

    



    }

    private static void getPassiveEffect() {
        //get passive effect
        //add to player
        //add to player manager


    }
}