using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Passive : NetworkBehaviour
{

    private string passiveType;
    private string passiveName;
    private string passiveDescription;

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



}
