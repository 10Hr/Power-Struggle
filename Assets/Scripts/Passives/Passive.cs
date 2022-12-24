using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Passive : NetworkBehaviour
{
    [SyncVar]
    private string passiveType;
    [SyncVar]
    private string passiveName;
    [SyncVar]
    private string passiveDescription;
    [SyncVar]
    private string passiveEffect;
    
    // properties
    public string PassiveType
    {
        get { return passiveType; }
        set { passiveType = value; }
    }
    public string PassiveName
    {
        get { return passiveName; }
        set { passiveName = value; }
    }
    public string PassiveDescription
    {
        get { return passiveDescription; }
        set { passiveDescription = value; }
    }
    public string PassiveEffect
    {
        get { return passiveEffect; }
        set { passiveEffect = value; }
    }




}
