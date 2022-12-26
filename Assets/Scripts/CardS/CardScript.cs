using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Mirror;

public class CardScript : NetworkBehaviour
{

    public Sprite cardFront;
    public Sprite cardBack;

    public Sprite sprCharisma;
    public Sprite sprCunning;
    public Sprite sprIntelligence;
    public Sprite sprStrength;

    private string type = "";
    private string title = "";
    private string cost = "";
    private string description = "";
    public bool hovered;
    public bool selected = false;
    public bool prevSelected = false;
    public int sortingDefault;

    public string Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    public string Title
    {
        get { return title; }
        set { title = value; }
    }

        public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public string Type
    {
        get { return type; }
        set 
        { 
            type = value;
            switch (type)
            {
                case "charisma":
                    cardBack = sprCharisma;
                    break;
                case "cunning":
                    cardBack = sprCunning;
                    break;
                case "intelligence":
                    cardBack = sprIntelligence;
                    break;
                case "strength":
                    cardBack = sprStrength;
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = cardBack;
    }

    // Update is called once per frame
    void Update()
    {
        //switch (type)
        //{
        //    case "charisma":
        //        cardBack =
        //        break;
        //    case "cunning":
        //        break;
        //    case "strength":
        //        break;
        //    case "intelligence":
        //        break;
        //}
        gameObject.GetComponent<SpriteRenderer>().sprite = cardBack;
    }

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
        if (hovered && cardBack != null) {
            gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 0);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 9;
        }
        if (!hovered && cardBack != null) {
            transform.localScale = new Vector3(1f, 1f, 0);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingDefault;
        }
    }
    
    public void OnMouseEnter() {
        hovered = true;
    }
    public void OnMouseExit() {
        hovered = false;
    }

    [Command(requiresAuthority = false)]
    public void OnMouseDown()
    {
        if (cardBack != null)
        {
            prevSelected = selected;
            selected = !selected;
        }
    }
}
