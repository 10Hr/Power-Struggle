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
    public bool hovered;
    public bool selected = false;
    public bool prevSelected = false;
    public int sortingDefault;

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
    //THIS IS CALLED FROM A CLIENT RPC IN PLAYERMANAGER
    public void Flip()
    {
        Sprite currentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        if (!hasAuthority)
        {
            //Debug.Log("No Authority");
        }
        else
        {
            if (currentSprite = cardBack)
               gameObject.GetComponent<SpriteRenderer>().sprite = cardFront;
            
            //Debug.Log("I have Authority");
        }
    }

    public void Enlarge() {
        if (hovered) {
            this.transform.localScale = new Vector3(1.25f, 1.25f, 0);
            this.GetComponent<SpriteRenderer>().sortingOrder = 9;
        }
        if (!hovered) {
            this.transform.localScale = new Vector3(1f, 1f, 0);
            this.GetComponent<SpriteRenderer>().sortingOrder = sortingDefault;
        }
    }
    
    public void OnMouseEnter() {
        hovered = true;
    }
    public void OnMouseExit() {
        hovered = false;
    }
    [ClientRpc]
    public void OnMouseDown()
    {
        prevSelected = selected;
        selected = !selected;
        
    }
}
