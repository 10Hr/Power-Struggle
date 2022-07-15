using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;
public class PassiveManager : NetworkBehaviour
{
    // Start is called before the first frame update


    private string passiveType;
    private string passiveName;
    private string passiveDescription;
    private List<GameObject> passives = new List<GameObject>();




    
    //Type, Name, Desc
    // add thing to turn periods to commas in description
    public void getPassivesFromFile() {
        string path = null;
        string line = null;
        StreamReader input = null;

        path = Application.dataPath + " /StreamingAssets/Passives.txt";
        input = new StreamReader(path);
        line = null;

    while ((line = input.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    passives.Add(new GameObject("passive"));
                }
                input.Close();

    }
    void effect() {
        //do stuff
    }

    void Start() {}
    void Update() {}
}