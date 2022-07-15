using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Mirror;

public class CardScript : NetworkBehaviour
{

    public Sprite cardFront;
    public Sprite cardBack;

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
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //THIS IS CALLED FROM A CLIENT RPC IN PLAYERMANAGER
    public void flip()
    {
        Sprite currentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        if (!hasAuthority)
        {
            //Debug.Log("No Authority");
        }
        else
        {
            if (currentSprite = cardBack)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = cardFront;
            }
            //Debug.Log("I have Authority");
        }
    }
}
