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



    // properties
    private string PassiveType
    {
        get { return passiveType; }
        set { passiveType = value; }
    }
    private string PassiveName
    {
        get { return passiveName; }
        set { passiveName = value; }
    }
    private string PassiveDescription
    {
        get { return passiveDescription; }
        set { passiveDescription = value; }
    }

    
    //Type, Name, Desc
    // add thing to turn periods to commas in description
    public void getPassivesFromFile() {

    }
    void effect() {
        //do stuff
    }

    void Start() {}
    void Update() {}
}
