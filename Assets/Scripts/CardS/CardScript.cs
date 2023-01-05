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
    public float defaultY;

    public GameState gameState;

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
        defaultY = gameObject.transform.localPosition.y;
        gameObject.GetComponent<SpriteRenderer>().sprite = cardBack;
        sortingDefault = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        gameState = GameObject.Find("FSM").GetComponent<GameState>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = cardBack;

        Enlarge();
    }

    public void Flip()
    {
        Sprite currentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        if (this.gameObject.tag == "CardSlot")
        {
            if (currentSprite = cardBack)
                gameObject.GetComponent<SpriteRenderer>().sprite = cardFront;
        }
    }

    public void Enlarge() {
        if (hovered && cardBack != null && this.gameObject.tag == "CardSlot") {
            gameObject.transform.localScale = new Vector3(75f, 75f, 0);
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, defaultY + 100, 0);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 7;
        }
        if (!hovered && cardBack != null) {
            transform.localScale = new Vector3(45f, 45f, 0);
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, defaultY, 0);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingDefault;
        }
    }
    
    public void OnMouseEnter() {
        hovered = true;
    }
    public void OnMouseExit() {
        hovered = false;
    }

    public void OnMouseDown()
    {
        if (cardBack != null && this.gameObject.tag == "CardSlot" && gameState.currentState == GameStates.LoadEnemyCards)
        {
            prevSelected = selected;
            selected = !selected;
        }
        else if ((cardBack != null && this.gameObject.tag == "CardSlot" && gameState.currentState == GameStates.Turn) && gameState.currentPlayer.netId == NetworkClient.localPlayer.netId && selected)
        {
          gameState.currentPlayer.deck.pullEff(title);
          gameState.turn++;
        }
    }
}
