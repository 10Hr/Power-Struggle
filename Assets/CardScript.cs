using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Mirror;

public class CardScript : NetworkBehaviour
{

    private string title;
    private string effect;
    private string stat;

    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    public string Effect
    {
        get { return effect; }
        set { effect = value; }
    }

    public string Stat
    {
        get { return stat; }
        set { stat = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
